using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IVideoService
    {
        Task<Video> UploadVideo(Stream stream, string fileName, string contentType, string title, string description);
    }
}
