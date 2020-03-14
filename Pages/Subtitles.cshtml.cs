using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace youtube_subs.Pages
{
    [ResponseCache (Location = ResponseCacheLocation.Any, Duration = 2592000)]
    public class SubtitlesModel : VttPageModelBase
    {
        [BindProperty (SupportsGet = true)]
        public string VideoId { get ; set ; }

        [BindProperty (SupportsGet = true)]
        public string Lang { get ; set ; }

        public string Title { get ; set ; }

        public List<SelectListItem> Langs { get ; set ; } = new List<SelectListItem> () ;

        public List<string> Lines { get ; set ; }

        public async Task OnGetAsync ()
        {
            if (VideoId == null)
                throw new ArgumentNullException (nameof (VideoId)) ;

            if (!VideoId.IsAbsolutelySafe ())
                throw new ArgumentException (nameof (VideoId)) ;

            if (Lang == null)
                throw new ArgumentNullException (nameof (Lang)) ;

            if (!Lang.IsAbsolutelySafe ())
                throw new ArgumentException (nameof (Lang)) ;

            if (!Request.Cookies.TryGetValue ($"vtt-{VideoId}-url",   out var vttUrl) ||
                !Request.Cookies.TryGetValue ($"vtt-{VideoId}-langs", out var vttLangs))
            {
                var (vtts, title) = await Helpers.GetManifestDataAsync (VideoId, HttpContext.RequestAborted) ;

                if (!vtts.TryGetValue (Lang, out vttUrl))
                    throw new ArgumentOutOfRangeException (nameof (Lang)) ;

                // set list of languages from vtts.Keys
                foreach (var item in Languages.Items)
                    if (vtts.ContainsKey (item.Value))
                        Langs.Add (item) ;

                AdjustVttCookies (vtts, VideoId, Lang, title) ;
            }
            else
            {
                // set list of languages from vttLangs
                foreach (var item in Languages.Items)
                    if (vttLangs.Contains (item.Value))
                        Langs.Add (item) ;

                Title = Request.Cookies["title"] ?? $"#{VideoId}" ;
            }

            using (var client = new HttpClient ())
            using (HttpContext.RequestAborted.Register (client.CancelPendingRequests))
            using (var stream = await client.GetStreamAsync (vttUrl))
            using (var reader = new StreamReader (stream))
            {
                Lines = new List<string> () ;

                while (true)
                {
                    var line  = await reader.ReadLineAsync () ;
                    if (line == null)
                        return ;

                    Lines.Add (line) ;
                }
            }
        }

        public IActionResult OnPost ()
        {
            Response.Cookies.Delete ($"vtt-{VideoId}-url") ;
            return RedirectToPage   ("Subtitles", new { VideoId, Lang }) ;
        }
    }
}