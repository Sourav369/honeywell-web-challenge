using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using honeywell_web_challenge.Models;

namespace honeywell_web_challenge.Services
{
    public interface IMediaService
    {
        IEnumerable<MediaFileViewModel> GetAllMediaFiles();
        Task SaveFilesAsync(IEnumerable<IFormFile> files);
    }
}
