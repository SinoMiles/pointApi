using System.ComponentModel.DataAnnotations;
using System;

namespace Point.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public int UserId { get; set; } // 报名人的唯一标识符
        public string NickName { get; set; }
        public string Image { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; } // 报名日期
        public int LotteryActivityId { get; set; }//活动id
        public bool Winner { get; set; }
        public string Code { get; set; }//中奖核销码
        public bool Verify { get; set; }//是否进行核销

    }
}
