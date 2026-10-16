using Application.Interfaces;
using Domain.Entities;
using Infrastructure.DbContext;

namespace Infrastructure.Services;

public class VideoService : IVideoService
{
    private readonly ApplicationDbContext _context;

    public VideoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Video> UploadVideo(Stream stream, string fileName, string contentType, string title, string description)
    {
        var folder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        Directory.CreateDirectory(folder);

        var savedName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var path = Path.Combine(folder, savedName);

        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            await stream.CopyToAsync(fileStream);
        }

        var video = new Video
        {
            Title = title,
            Description = description,
            OriginalUrl = path,
            Status = "Processing",
            UploadedByUserId = 0
        };

        _context.Videos.Add(video);
        await _context.SaveChangesAsync();

        return video;
    }
}
