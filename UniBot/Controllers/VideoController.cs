using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace CloudLecture.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;
        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost("{id}convert")]
        [HttpPost("{id}/convert")]
        public async Task<IActionResult> ConvertVideo(int id)
        {
            var video = await _videoService.ConvertVideo(id);

            if (video is null)
                return NotFound();

            return Ok(new
            {
                video.Id,
                video.Status,
                Versions = video.Versions.Select(v => new { v.Quality, v.Url })
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var videos = await _videoService.GetAllVideos();

            return Ok(videos.Select(v => new
            {
                v.Id,
                v.Title,
                v.Description,
                v.CreatedAt,
                Qualities = v.Versions.Select(x => x.Quality)
            }));
        }

        [HttpGet("{id}/stream/{quality}")]
        public async Task<IActionResult> Stream(int id, string quality)
        {
            var video = await _videoService.GetVideoById(id);

            if (video is null)
                return NotFound();

            var version = video.Versions.FirstOrDefault(v => v.Quality == quality);

            if (version is null || !System.IO.File.Exists(version.Url))
                return NotFound("This quality is not available");

            return PhysicalFile(version.Url, "video/mp4", enableRangeProcessing: true);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var video = await _videoService.GetVideoById(id);

            if (video is null)
                return NotFound();

            return Ok(new
            {
                video.Id,
                video.Title,
                video.Description,
                video.Status,
                Qualities = video.Versions.Select(v => v.Quality)
            });
        }
    }
}
