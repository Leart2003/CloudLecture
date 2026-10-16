using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class VideoService : IVideo
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public VideoService(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<Video> UploadVideo(Stream stream, string fileName, string contentType, string title, string description)
    {
        var container = new BlobContainerClient(
            _config["AzureStorage:ConnectionString"],
            _config["AzureStorage:UploadsContainer"]);

        await container.CreateIfNotExistsAsync();

        var blobName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var blob = container.GetBlobClient(blobName);

        await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType });

        var video = new Video
        {
            Title = title,
            Description = description,
            OriginalUrl = blob.Uri.ToString(),
            Status = "Processing",
            UploadedByUserId = 0 // temporary, until authentication is added
        };

        _context.Videos.Add(video);
        await _context.SaveChangesAsync();

        return video;
    }
}