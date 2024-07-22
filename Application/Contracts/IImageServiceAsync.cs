using Microsoft.AspNetCore.Http;

namespace Application.Contracts;

public interface IImageServiceAsync
{
    Task<string[]> GetImagesAsBase64Async(string username);
    Task<string> GetImageAsBase64Async(string username, string imageName);
    Task<string[]> GetUserImageNamesAsync(string userName);
    Task<string[]> SaveImagesAsync(string username, IFormFileCollection? images);
}
