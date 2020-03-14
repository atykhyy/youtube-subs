using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace youtube_subs
{
    public static class Extensions
    {
        public static bool IsAbsolutelySafe (this string s)
        {
            foreach (var ch in s)
                if (!(ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z' || ch == '_' || ch == '-'))
                    return false ;

            return true ;
        }
    }

    public static class Helpers
    {
        public static async Task<(int, string, string)> ExecAsync (string executable, string arguments, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<int> () ;
            var psi = new ProcessStartInfo (executable, arguments) ;
            psi.UseShellExecute        = false ;
            psi.RedirectStandardError  = true  ;
            psi.RedirectStandardOutput = true  ;

            var p = new Process
            {
                StartInfo           = psi,
                EnableRaisingEvents = true,
            } ;

            var error  = new StringBuilder () ;
            var output = new StringBuilder () ;

            p.Exited             += (sender, e) => tcs.TrySetResult (p.ExitCode) ;
            p.ErrorDataReceived  += (sender, e) => error.Append     (e.Data) ;
            p.OutputDataReceived += (sender, e) => output.Append    (e.Data) ;

            p.Start () ;
            p.BeginErrorReadLine  () ;
            p.BeginOutputReadLine () ;

            using (cancellationToken.Register (() =>
            {
                tcs.TrySetCanceled (cancellationToken) ;
                p.Kill () ;
            }))
                await tcs.Task ;

            return (p.ExitCode, output.ToString (), error.ToString ()) ;
        }

        public static async Task<(Dictionary<string, string>, string)> GetManifestDataAsync (string videoId, CancellationToken cancellationToken)
        {
            var (exitCode, output, error) = await ExecAsync ("/bin/youtube-dl", $"-sj --write-auto-sub https://www.youtube.com/watch?v={videoId}", cancellationToken) ;
            if  (exitCode != 0)
                throw new Exception ($"youtube-dl: {exitCode} => {error}") ;

            var manifest = Newtonsoft.Json.JsonConvert.DeserializeObject<YoutubeVideoManifest> (output) ;
            if((manifest?.AutomaticCaptions?.Count ?? 0) == 0)
                throw new Exception ("This video has no subtitles") ;

            var vtts = new Dictionary<string, string> () ;
            foreach (var kv in manifest.AutomaticCaptions)
                if (kv.Value != null)
                    foreach (var item in kv.Value)
                        if (item.Ext == "vtt")
                            vtts[kv.Key] = item.Url ;

            if (vtts.Count == 0)
                throw new Exception ("This video has no subtitles in VTT format") ;

            return (vtts, manifest.Title) ;
        }
    }
}
