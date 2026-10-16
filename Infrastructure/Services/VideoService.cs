using Application.Interfaces;
using Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Services;

public class VideoService : IVideoService
{
    
    private readonly ApplicationDbContext _context;

    public VideoService(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<Video> UploadVideo(Stream stream, string fileName, string contentType, string title, string description, int userId)
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
            UploadedByUserId = userId
        };

        _context.Videos.Add(video);
        await _context.SaveChangesAsync();

        return video;
    }
    public async Task<Video?> ConvertVideo(int videoId)
    {
        var video = await _context.Videos
            .Include(v => v.Versions)
            .FirstOrDefaultAsync(v => v.Id == videoId);

        if (video is null) return null;

        var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "processed", video.Id.ToString());
        Directory.CreateDirectory(outputFolder);

        var qualities = new Dictionary<string, int>
    {
        { "480p", 480 },
        { "720p", 720 },
        { "1080p", 1080 }
    };

        foreach (var quality in qualities)
        {
            var outputPath = Path.Combine(outputFolder, $"{quality.Key}.mp4");

            var arguments = $"-y -i \"{video.OriginalUrl}\" -vf scale=-2:{quality.Value} -c:v libx264 -preset fast -c:a aac \"{outputPath}\"";

            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true
            });

            await process!.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                video.Status = "Failed";
                await _context.SaveChangesAsync();
                return video;
            }

            video.Versions.Add(new VideoVersion
            {
                Quality = quality.Key,
                Url = outputPath
            });
        }

        video.Status = "Ready";
        await _context.SaveChangesAsync();
        return video;
    }

    public async Task<List<Video>> GetAllVideos()
    {
      return await  _context.Videos
            .Include(v => v.Versions)
            .Where(v => v.Status == "Ready")
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();
    }

    public Task<Video?> GetVideoById(int id)
    {
        return _context.Videos
            .Include(v => v.Versions)
            .FirstOrDefaultAsync(v => v.Id == id);
    }
}
