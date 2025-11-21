namespace honeywell_web_challenge.Models
{
    public class MediaFileViewModel
    {
        public string FileName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;   // /media/xxx.mp4
        public long SizeBytes { get; set; }

        // Convenience property for display (MB with 2 decimals)
        public string SizeInMb =>
            (SizeBytes / (1024.0 * 1024.0)).ToString("0.00");
    }
}
