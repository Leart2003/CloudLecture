using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IVideoService
    {
        Task<Video> UploadVideo(Stream stream, string fileName, string contentType, string title, string description, int userId);

        Task<Video?> ConvertVideo(int videoId);

        Task<List<Video>> GetAllVideos();
        Task<Video?> GetVideoById(int id);

        Task<bool> DeleteVideo(int id);
    }
}
