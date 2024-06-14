using System;
using System.Collections.Generic;

#nullable disable

namespace GWeb.Models
{
    public partial class Version
    {
        public int Id { get; set; }
        public int PlafformType { get; set; }
        public string Version1 { get; set; }
        public string Url { get; set; }
        public bool Forced { get; set; }
        public string Content { get; set; }
    }
}
