using Microsoft.AspNetCore.Mvc;
using honeywell_web_challenge.Services;

namespace honeywell_web_challenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        // 200 MB max upload size (for the request)
        private const long MaxUploadBytes = 200L * 1024L * 1024L;

        public UploadController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        // POST: /api/upload
        // Allow multipart/form-data bodies up to 200 MB for THIS endpoint only
        [HttpPost]
        [RequestSizeLimit(MaxUploadBytes)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxUploadBytes)]
        public async Task<IActionResult> Upload([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(new { message = "No files were provided for upload." });
            }

            // Total request size check (as per requirements)
            long totalSize = files.Sum(f => f.Length);
            if (totalSize > MaxUploadBytes)
            {
                // 413 – Payload Too Large
                return StatusCode(StatusCodes.Status413PayloadTooLarge,
                    new { message = "Total upload size must be 200 MB or less." });
            }

            try
            {
                await _mediaService.SaveFilesAsync(files);
                return Ok(new { message = "Files uploaded successfully." });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. non-MP4 extension from the service
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception in a real app
                Console.WriteLine(ex);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred while uploading files." });
            }
        }
    }
}
