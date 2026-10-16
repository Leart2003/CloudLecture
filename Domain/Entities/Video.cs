using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Processing"; 
        public int UploadedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<VideoVersion> Versions { get; set; } = new();
    }
}
