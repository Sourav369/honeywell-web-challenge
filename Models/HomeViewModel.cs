using System.Collections.Generic;

namespace honeywell_web_challenge.Models
{
    public class HomeViewModel
    {
        public List<MediaFileViewModel> MediaFiles { get; set; } = new();
        
        // "catalogue" or "upload" - controls which view section is showing
        public string ActiveView { get; set; } = "catalogue";

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
    }
}
