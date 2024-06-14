using System;
using Aop.Api.Domain;
using GWeb.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point.Models
{
    public  class Activity
    {
        [Key]
        public int ActivityId { get; set; }  // 活动ID

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }  // 活动标题

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }  // 活动描述

        [Required]
        public DateTime StartTime { get; set; }  // 开始时间

        [Required]
        public DateTime EndTime { get; set; }  // 结束时间

        [Required]
        [MaxLength(100)]
        public string Location { get; set; }  // 活动地点

        [Required]
        public double Latitude { get; set; }  // 纬度

        [Required]
        public double Longitude { get; set; }  // 经度

        public int MaxParticipants { get; set; }  // 最大参与人数

        public int CurrentParticipants { get; set; }  // 当前参与人数

        [ForeignKey("ActivityType")]
        public int TypeId { get; set; }  // 活动类型ID
        public ActivityType ActivityType { get; set; }  // 活动类型

        [Required]
        public bool IsPaid { get; set; }  // 是否需要付费

        [Column(TypeName = "decimal(18,2)")]
        public decimal? FeeAmount { get; set; }  // 付费金额

        [Required]
        [MaxLength(100)]
        public string FeeDescription { get; set; }  // 费用说明

        [Required]
        public ActivityStatus Status { get; set; }  // 活动状态

        [ForeignKey("User")]
        public int OrganizerId { get; set; }  // 组织者ID
        public User Organizer { get; set; }  // 组织者

        public ICollection<Participant> Participants { get; set; } // 参与者列表

        public DateTime CreatedAt { get; set; } = DateTime.Now;  // 创建时间
        public DateTime UpdatedAt { get; set; } = DateTime.Now;  // 更新时间

        public int? ApprovedParticipantId { get; set; }  // 通过报名的用户ID
        [ForeignKey("ApprovedParticipantId")]
        public User ApprovedParticipant { get; set; }  // 通过报名的用户

        public bool IsApprovedParticipantVisible { get; set; }  // 是否显示通过报名的用户信息

        public ICollection<ActivityStep> ActivitySteps { get; set; }  // 活动流程步骤
    }

    public class ActivityStep
    {
        [Key]
        public int StepId { get; set; }  // 步骤ID

        [Required]
        public int ActivityId { get; set; }  // 活动ID
        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; }  // 关联的活动

        [Required]
        public DateTime StepStartTime { get; set; }  // 步骤开始时间

        [Required]
        public DateTime StepEndTime { get; set; }  // 步骤结束时间

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }  // 步骤描述

        public int StepOrder { get; set; }  // 步骤顺序
    }

    public enum ActivityStatus
    {
        Pending,
        Approved,
        Rejected,
        Completed
    }
}

