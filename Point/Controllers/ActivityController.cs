namespace Point.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GWeb.Models;
    using global::Point.Models;
    using AutoMapper;
    using System;
    using global::Point.Controllers.dto;
    using GWeb.library;

    namespace Point.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class ActivitiesController : ControllerBase
        {
            private readonly girlContext _context;
            private readonly IMapper _mapper;

            public ActivitiesController(girlContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            // 获取所有活动
            [HttpGet]
            public async Task<ActionResult<ApiResponse<List<ActivitySummaryDto>>>> GetActivities([FromQuery] GetActivitiesDto dto)
            {
                try
                {
                    var query = _context.Activities
                        .Include(a => a.Organizer) // 包含活动组织者
                        .Include(a => a.Participants); // 包含报名人

                    // 根据搜索名称过滤活动
                    //query = query.WhereIf(a => a.Title.Contains(dto.SearchName), !string.IsNullOrEmpty(dto.SearchName));

                    // 分页
                    var activities = await query
                        .Skip((dto.Page - 1) * dto.PageSize)
                        .Take(dto.PageSize)
                        .ToListAsync();

                    var activitySummaries = _mapper.Map<List<ActivitySummaryDto>>(activities);

                    // 计算并设置距离
                    foreach (var summary in activitySummaries)
                    {
                        summary.Distance = DistanceCalculator.CalculateDistance(new GeoLocation { Latitude = summary.Latitude, Longitude = summary.Longitude }, new GeoLocation { Latitude = dto.UserLatitude, Longitude = dto.UserLongitude });
                    }

                    return new ApiResponse<List<ActivitySummaryDto>> { Success = true, Data = activitySummaries };
                }
                catch (Exception ex)
                {
                    return new ApiResponse<List<ActivitySummaryDto>> { Success = false, ErrorMessage = ex.Message };
                }
            }
            // GET: api/Activities/5
            [HttpGet("{id}")]
            public async Task<ActionResult<ApiResponse<Activity>>> GetActivity(int id)
            {
                var activity = await _context.Activities
                                              .Include(a => a.Participants)
                                              .FirstOrDefaultAsync(a => a.ActivityId == id);

                if (activity == null)
                {
                    return Ok(new ApiResponse<Activity>
                    {
                        Success = false,
                        ErrorMessage = "Activity not found"
                    });
                }

                return Ok(new ApiResponse<Activity>
                {
                    Success = true,
                    Data = activity
                });
            }

            [HttpPost]
            public IActionResult CreateActivity([FromBody] CreateActivityDto createActivityDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var activity = _mapper.Map<Activity>(createActivityDto);

                _context.Activities.Add(activity);
                _context.SaveChanges();

                var response = new ApiResponse<Activity>
                {
                    Success = true,
                    Data = activity
                };

                return Ok(response);
            }

            // PUT: api/Activities/5
            [HttpPut("{id}")]
            public async Task<ActionResult<ApiResponse<Activity>>> PutActivity(int id, Activity activity)
            {
                if (id != activity.ActivityId)
                {
                    return Ok(new ApiResponse<Activity>
                    {
                        Success = false,
                        ErrorMessage = "Activity ID mismatch"
                    });
                }

                _context.Entry(activity).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(id))
                    {
                        return Ok(new ApiResponse<Activity>
                        {
                            Success = false,
                            ErrorMessage = "Activity not found"
                        });
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(new ApiResponse<Activity>
                {
                    Success = true,
                    Data = activity
                });
            }

            // DELETE: api/Activities/5
            [HttpDelete("{id}")]
            public async Task<ActionResult<ApiResponse<Activity>>> DeleteActivity(int id)
            {
                var activity = await _context.Activities.FindAsync(id);
                if (activity == null)
                {
                    return Ok(new ApiResponse<Activity>
                    {
                        Success = false,
                        ErrorMessage = "Activity not found"
                    });
                }

                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<Activity>
                {
                    Success = true,
                    Data = activity
                });
            }

            // POST: api/Activities/5/Participants
            [HttpPost("{id}/Participants")]
            public async Task<ActionResult<ApiResponse<Participant>>> AddParticipant(int id)
            {
                var activity = await _context.Activities
                                              .FirstOrDefaultAsync(a => a.ActivityId == id);

                if (activity == null)
                {
                    return Ok(new ApiResponse<Participant>
                    {
                        Success = false,
                        ErrorMessage = "Activity not found"
                    });
                }
                int userId = int.Parse(UserHelper.getUser(Request, _context));
                var userInfo = await _context.Users.Where(b => b.Id == userId).FirstOrDefaultAsync();
                Participant participant = new Participant();
                participant.NickName = userInfo.NickName;
                participant.Image = userInfo.HeadImgUrl;
                participant.UserId = userId;
                participant.LotteryActivityId = id;
                _context.Participants.Add(participant);
                await _context.SaveChangesAsync();
                return Ok(new ApiResponse<Participant>
                {
                    Success = true,
                    Data = participant
                });
            }

            private bool ActivityExists(int id)
            {
                return _context.Activities.Any(e => e.ActivityId == id);
            }
        }
        public class GeoLocation
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public class DistanceCalculator
        {
            private const double EarthRadiusKm = 6371;

            public static double CalculateDistance(GeoLocation location1, GeoLocation location2)
            {
                double dLat = DegreeToRadian(location2.Latitude - location1.Latitude);
                double dLon = DegreeToRadian(location2.Longitude - location1.Longitude);

                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                           Math.Cos(DegreeToRadian(location1.Latitude)) * Math.Cos(DegreeToRadian(location2.Latitude)) *
                           Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                double distance = EarthRadiusKm * c; // Distance in km
                return distance;
            }

            private static double DegreeToRadian(double angle)
            {
                return Math.PI * angle / 180.0;
            }
        }
    }

}

