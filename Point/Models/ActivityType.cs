using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point.Models
{
    public class ActivityType
    {
        [Key]
        public int TypeId { get; set; }  // 活动类型ID

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }  // 活动类型名称

        [MaxLength(200)]
        public string Description { get; set; }  // 活动类型描述

        public int? ParentTypeId { get; set; }  // 父类型ID
        [ForeignKey("ParentTypeId")]
        public ActivityType ParentType { get; set; }  // 父类型

        public ICollection<ActivityType> SubTypes { get; set; }  // 子类型列表

    }
}

