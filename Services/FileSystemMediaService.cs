using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using honeywell_web_challenge.Models;

namespace honeywell_web_challenge.Services
{
    public class FileSystemMediaService : IMediaService
    {
        private readonly string _mediaRoot;

        private static readonly string[] AllowedExtensions = [".mp4"];

        public FileSystemMediaService(IWebHostEnvironment env)
        {
            // wwwroot/media
            _mediaRoot = Path.Combine(env.WebRootPath, "media");

            if (!Directory.Exists(_mediaRoot))
            {
                Directory.CreateDirectory(_mediaRoot);
            }
        }

        public IEnumerable<MediaFileViewModel> GetAllMediaFiles()
        {
            var dirInfo = new DirectoryInfo(_mediaRoot);

            if (!dirInfo.Exists)
                yield break;

            foreach (var file in dirInfo.GetFiles("*.mp4").OrderBy(f => f.Name))
            {
                yield return new MediaFileViewModel
                {
                    FileName = file.Name,
                    Url = $"/media/{file.Name}",
                    SizeBytes = file.Length
                };
            }
        }

        public async Task SaveFilesAsync(IEnumerable<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                    continue;

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!AllowedExtensions.Contains(ext))
                {
                    throw new InvalidOperationException(
                        $"Only MP4 files are allowed. Invalid file: {file.FileName}");
                }

                var savePath = Path.Combine(_mediaRoot, Path.GetFileName(file.FileName));

                // Overwrite existing file if same name (allowed by requirements)
                await using var stream = new FileStream(savePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }
        }
    }
}
