using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class VideoVersion
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public string Quality { get; set; } = string.Empty; // 480p, 720p, 1080p
        public string Url { get; set; } = string.Empty;

        public Video Video { get; set; } = null!;
    }
}
