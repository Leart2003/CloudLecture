using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudLecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideosController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [Authorize(Roles = "Professor")]
        [HttpPost("upload")]
        [RequestSizeLimit(524_288_000)] // 500 MB
        [RequestFormLimits(MultipartBodyLengthLimit = 524_288_000)]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string title, [FromForm] string? description)
        {
            if (file is null || file.Length == 0)
                return BadRequest("No file uploaded");

            if (!file.ContentType.StartsWith("video/"))
                return BadRequest("File must be a video");

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            using var stream = file.OpenReadStream();
            var video = await _videoService.UploadVideo(
                stream, file.FileName, file.ContentType, title, description ?? string.Empty, userId);

            return Ok(new { video.Id, video.Title, video.Status });
        }

        [Authorize(Roles = "Professor")]
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
    }
}