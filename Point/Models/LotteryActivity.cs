using System.ComponentModel.DataAnnotations;
using System;
using GWeb.Models;

namespace Point.Models
{
    public class LotteryActivity
    {
        public int Id { get; set; } // 活动的唯一标识符

        [Required]
        public string Name { get; set; } // 活动名称
        public string image { get; set; }//预览图

        [Required]
        public string Description { get; set; } // 活动描述

        [Required]
        public DateTime StartDate { get; set; } // 活动开始日期

        [Required]
        public DateTime EndDate { get; set; } // 活动结束日期

        public int MaxParticipants { get; set; } // 最大参与人数

        public bool IsOpen { get; set; } // 活动是否开放
        //已报名人数
        public int HaveParticipants { get; set; }
        //创建时间
        public DateTime CreateTime { get; set; }

        // 导航属性，表示活动创建人
        public virtual User CreatorUser { get; set; } // 活动创建人的用户信息

    }
}
