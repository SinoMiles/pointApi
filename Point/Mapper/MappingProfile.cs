using System;
using AutoMapper;
using Point.Controllers.dto;
using Point.Models;

namespace Point.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivitySummaryDto>();
            CreateMap<CreateActivityDto, Activity>();
            CreateMap<ActivityStepDto, ActivityStep>();
        }
    }
}

