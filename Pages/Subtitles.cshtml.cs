using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace youtube_subs
{
    [ResponseCache (Location = ResponseCacheLocation.Any, Duration = 2592000)]
    public class SubtitlesModel : PageModel
    {
        [BindProperty (SupportsGet = true)]
        public string VideoId { get ; set ; }

        [BindProperty (SupportsGet = true)]
        public string Lang { get ; set ; }

        public string[] Lines { get ; set ; }

        public async Task OnGetAsync ()
        {
            if (VideoId == null)
                throw new ArgumentNullException (nameof (VideoId)) ;

            if (!IsAbsolutelySafe (VideoId))
                throw new ArgumentException (nameof (VideoId)) ;

            if (Lang == null)
            {
                // TODO: parse Accept-Language?
                Lang = "en" ;
            }
            else
            if (!IsAbsolutelySafe (Lang))
                throw new ArgumentException (nameof (Lang)) ;

            var temp = Path.GetTempPath () ;
            var guid = Guid.NewGuid ().ToString () ;

            using (var ydl = new Process
            {
                StartInfo  = new ProcessStartInfo ("/bin/youtube-dl",
                    $"--skip-download --sub-lang {Lang} --write-auto-sub -o \"{temp}{guid}\" https://www.youtube.com/watch?v={VideoId}"),
                EnableRaisingEvents = true,
            })
            {
                var tcs = new TaskCompletionSource<int> () ;
                ydl.Exited += (sender, e) => tcs.TrySetResult (ydl.ExitCode) ;
                ydl.Start () ;

                using (HttpContext.RequestAborted.Register (() => tcs.TrySetCanceled (HttpContext.RequestAborted)))
                    if (await tcs.Task != 0)
                        throw new Exception ($"youtube-dl: {ydl.ExitCode}") ;
            }

            foreach (var file in Directory.EnumerateFiles (temp, guid + "*"))
            {
                Lines = System.IO.File.ReadAllLines (file) ;
                return ;
            }

            throw new Exception ("downloaded subtitle file not found") ;
        }

        private bool IsAbsolutelySafe (string s)
        {
            foreach (var ch in s)
                if (!(ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z' || ch == '_' || ch == '-'))
                    return false ;

            return true ;
        }
    }
}