using System;
namespace Point.Controllers.dto
{
    public class GetActivitiesDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchName { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
    }

}

