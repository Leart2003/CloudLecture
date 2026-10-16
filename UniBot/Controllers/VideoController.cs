using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
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
    }
}
