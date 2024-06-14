using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Point.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point.Controllers.dto
{
    public class CreateActivityDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [MaxLength(100)]
        public string Location { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public int MaxParticipants { get; set; }

        public int CurrentParticipants { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        public bool IsPaid { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? FeeAmount { get; set; }

        [Required]
        [MaxLength(100)]
        public string FeeDescription { get; set; }

        [Required]
        public ActivityStatus Status { get; set; }

        [Required]
        public int OrganizerId { get; set; }

        public int? ApprovedParticipantId { get; set; }

        public bool IsApprovedParticipantVisible { get; set; }

        public ICollection<ActivityStepDto> ActivitySteps { get; set; }
    }

    public class ActivityStepDto
    {
        [Required]
        public DateTime StepStartTime { get; set; }

        [Required]
        public DateTime StepEndTime { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        public int StepOrder { get; set; }
    }

}

