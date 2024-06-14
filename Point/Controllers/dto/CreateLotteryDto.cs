using System.ComponentModel.DataAnnotations;
using System;

namespace Point.Controllers.dto
{
    public class CreateLotteryDto
    {
        public string Name { get; set; } // 活动名称
        public string image { get; set; }//预览图

        [Required]
        public string Description { get; set; } // 活动描述

        [Required]
        public DateTime StartDate { get; set; } // 活动开始日期

        [Required]
        public DateTime EndDate { get; set; } // 活动结束日期

        public int MaxParticipants { get; set; } // 最大参与人数
    }
}
