using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace youtube_subs.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel (ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string VideoUrl { get ; set ; }

        public void OnGet ()
        {
        }

        public async Task<IActionResult> OnPostAsync ()
        {
            if (VideoUrl?.StartsWith  ("https://www.youtube.com/watch?v=") == true)
                return RedirectToPage ("Subtitles", new { VideoId = VideoUrl.Substring (32) }) ;

            return Page () ;
        }
    }
}
