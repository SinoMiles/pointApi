using GWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Point.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityTypeController : ControllerBase
    {
        private readonly GWeb.Models.girlContext _context;

        public ActivityTypeController(girlContext context)
        {
            _context = context;
        }

        // GET: api/ActivityType
        // 获取所有活动类型及其子类型
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ActivityType>>>> GetActivityTypes()
        {
            var activityTypes = await _context.ActivityTypes
                .Include(at => at.SubTypes)
                .ToListAsync();

            return new ApiResponse<List<ActivityType>>
            {
                Success = true,
                Data = activityTypes,
                ErrorMessage = null
            };
        }

        // GET: api/ActivityType/5
        // 根据ID获取活动类型及其子类型
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ActivityType>>> GetActivityType(int id)
        {
            var activityType = await _context.ActivityTypes
                .Include(at => at.SubTypes)
                .FirstOrDefaultAsync(at => at.TypeId == id);

            if (activityType == null)
            {
                return new ApiResponse<ActivityType>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "活动类型未找到"
                };
            }

            return new ApiResponse<ActivityType>
            {
                Success = true,
                Data = activityType,
                ErrorMessage = null
            };
        }

        // POST: api/ActivityType
        // 创建新的活动类型
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ActivityType>>> CreateActivityType(ActivityType activityType)
        {
            _context.ActivityTypes.Add(activityType);
            await _context.SaveChangesAsync();

            return new ApiResponse<ActivityType>
            {
                Success = true,
                Data = activityType,
                ErrorMessage = null
            };
        }

        // PUT: api/ActivityType/5
        // 更新活动类型
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActivityType(int id, ActivityType activityType)
        {
            if (id != activityType.TypeId)
            {
                return BadRequest(new ApiResponse<ActivityType>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "ID不匹配"
                });
            }

            _context.Entry(activityType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityTypeExists(id))
                {
                    return NotFound(new ApiResponse<ActivityType>
                    {
                        Success = false,
                        Data = null,
                        ErrorMessage = "活动类型未找到"
                    });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new ApiResponse<ActivityType>
            {
                Success = true,
                Data = activityType,
                ErrorMessage = null
            });
        }

        // DELETE: api/ActivityType/5
        // 删除活动类型
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivityType(int id)
        {
            var activityType = await _context.ActivityTypes.FindAsync(id);
            if (activityType == null)
            {
                return NotFound(new ApiResponse<ActivityType>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "活动类型未找到"
                });
            }

            _context.ActivityTypes.Remove(activityType);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<ActivityType>
            {
                Success = true,
                Data = activityType,
                ErrorMessage = null
            });
        }

        private bool ActivityTypeExists(int id)
        {
            return _context.ActivityTypes.Any(e => e.TypeId == id);
        }
    }
}
