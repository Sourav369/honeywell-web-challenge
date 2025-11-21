using Microsoft.AspNetCore.Mvc;
using honeywell_web_challenge.Services;

namespace honeywell_web_challenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private const long MaxUploadBytes = 200L * 1024L * 1024L; // 200 MB

        public UploadController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        // POST: /api/upload
        [HttpPost]
        [RequestSizeLimit(MaxUploadBytes)]
        public async Task<IActionResult> Upload([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(new { message = "No files were provided for upload." });
            }

            // Optional extra check for size per file (on top of RequestSizeLimit)
            if (files.Any(f => f.Length > MaxUploadBytes))
            {
                // 413 – Payload Too Large
                return StatusCode(StatusCodes.Status413PayloadTooLarge,
                    new { message = "One or more files exceed the maximum size of 200MB." });
            }

            try
            {
                await _mediaService.SaveFilesAsync(files);
                return Ok(new { message = "Files uploaded successfully." });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. non-MP4 extension
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log and hide internal details from user
                // In a real-world app you'd log via ILogger<UploadController>
                Console.WriteLine(ex);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred while uploading files." });
            }
        }
    }
}
