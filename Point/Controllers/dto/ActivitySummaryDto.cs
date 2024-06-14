using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GWeb.Models;
using Point.Models;

public class ActivitySummaryDto
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public int RegisteredParticipants { get; set; }
    public int MaxParticipants { get; set; }
    public DateTime ActivityTime { get; set; }
    public string Address { get; set; }
    public string OrganizerName { get; set; }
    public string OrganizerImageUrl { get; set; }
    public double Distance { get; set; } // 距离，单位为 KM
    public double Latitude { get; set; }  // 纬度
    public double Longitude { get; set; }  // 经度
    public User Organizer { get; set; }  // 组织者
    public ICollection<Participant> Participants { get; set; } // 参与者列表
}
