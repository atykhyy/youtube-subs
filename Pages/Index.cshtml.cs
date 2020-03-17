using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace youtube_subs.Pages
{
    [DataContract]
    public class LanguagePreference
    {
        [DataMember (Name = "code", IsRequired = true)]
        public string Code   { get ; set ; }

        [DataMember (Name = "prefer", EmitDefaultValue = false)]
        public bool   Prefer { get ; set ; }
    }

    [DataContract]
    public class Preferences
    {
        [DataMember (Name = "languages")]
        public LanguagePreference[] LanguagePreferences { get ; set ; }
    }

    public class IndexModel : VttPageModelBase
    {
        private readonly IConfiguration      m_config ;
        private readonly ILogger<IndexModel> m_logger ;

        private readonly static Newtonsoft.Json.JsonSerializer JsonSerializer = Newtonsoft.Json.JsonSerializer.CreateDefault () ;

        public IndexModel (IConfiguration config, ILogger<IndexModel> logger)
        {
            m_config = config ;
            m_logger = logger ;
        }

        [BindProperty]
        public string VideoUrl { get ; set ; }

        [BindProperty]
        public string UseLang { get ; set ; }

        [BindProperty]
        public LanguagePreference Lang1 { get ; set ; }

        [BindProperty]
        public LanguagePreference Lang2 { get ; set ; }

        [BindProperty]
        public LanguagePreference Lang3 { get ; set ; }

        public override void OnPageHandlerExecuting (Microsoft.AspNetCore.Mvc.Filters.PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting (context) ;

            if (context.HandlerMethod?.MethodInfo?.Name == nameof (OnPostSave))
                return ;

            var prefCookie  = Request.Cookies[nameof (Preferences)] ;
            if (prefCookie != null)
            {
                Preferences preferences ;
                try
                {
                    preferences = Newtonsoft.Json.JsonConvert.DeserializeObject<Preferences> (prefCookie) ;
                }
                catch
                {
                    return ;
                }

                var lps  = preferences.LanguagePreferences ;
                if (lps != null)
                {
                    if (lps.Length > 0) Lang1 = lps[0] ;
                    if (lps.Length > 1) Lang2 = lps[1] ;
                    if (lps.Length > 2) Lang3 = lps[2] ;
                }
            }
        }

        public void OnGet ()
        {
        }

        public IActionResult OnPostSave ()
        {
            Response.Cookies.Append (nameof (Preferences), Newtonsoft.Json.JsonConvert.SerializeObject (new Preferences
            {
                LanguagePreferences = new[]
                {
                    Lang1,
                    Lang2,
                    Lang3,
                },
            }), new Microsoft.AspNetCore.Http.CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays (180),
            }) ;

            return RedirectToPage () ;
        }

        public async Task<IActionResult> OnPostReadAsync ()
        {
            var videoId  = Helpers.GetVideoId (VideoUrl) ;
            if (videoId == null || !videoId.IsAbsolutelySafe ())
                throw new Exception ("Invalid URL format") ;

            var (vtts, title) = await Helpers.GetManifestDataAsync (videoId, HttpContext.RequestAborted) ;

            if (UseLang == null || UseLang == "*")
            do
            {
                var videoDataUrl  = $"https://www.googleapis.com/youtube/v3/videos?id={videoId}&key={m_config["GoogleApiKey"]}&part=snippet&fields=items/snippet/defaultAudioLanguage" ;

                using (var client = new HttpClient ())
                using (HttpContext.RequestAborted.Register (client.CancelPendingRequests))
                using (var stream = await client.GetStreamAsync (videoDataUrl))
                using (var reader = new Newtonsoft.Json.JsonTextReader (new System.IO.StreamReader (stream)))
                {
                    var videoData = JsonSerializer.Deserialize<YoutubeVideoData> (reader) ;
                    var defLang1  = videoData?.Infos?.FirstOrDefault ()?.Snippet?.DefaultAudioLanguage ;
                    if (defLang1 != null)
                    {
                        var defLang2 = defLang1.Split ('-')[0] ;
                        if (Lang1.Prefer && (Lang1.Code == defLang1 || Lang1.Code == defLang2))
                        {
                            UseLang = Lang1.Code ;
                            break ;
                        }

                        if (Lang2.Prefer && (Lang2.Code == defLang1 || Lang2.Code == defLang2))
                        {
                            UseLang = Lang2.Code ;
                            break ;
                        }

                        if (Lang3.Prefer && (Lang3.Code == defLang1 || Lang3.Code == defLang2))
                        {
                            UseLang = Lang3.Code ;
                            break ;
                        }
                    }
                }

                if (Lang1.Code != null && vtts.ContainsKey (Lang1.Code))
                {
                    UseLang = Lang1.Code ;
                    break ;
                }

                if (Lang2.Code != null && vtts.ContainsKey (Lang2.Code))
                {
                    UseLang = Lang2.Code ;
                    break ;
                }

                if (Lang3.Code != null && vtts.ContainsKey (Lang3.Code))
                {
                    UseLang = Lang3.Code ;
                    break ;
                }

                // no language could be selected based on preferences, try Accept-Language
                if (Request.Headers.TryGetValue ("Accept-Language", out var acceptLangs))
                {
                    var list    = new List<KeyValuePair<double, string>> () ;
                    var headers = new Microsoft.AspNetCore.Http.Headers.RequestHeaders (Request.Headers) ;
                    foreach (var acceptLang in headers.AcceptLanguage)
                        if (acceptLang.Value.HasValue)
                            list.Add (new KeyValuePair<double, string> (acceptLang.Quality ?? 1, acceptLang.Value.Value)) ;

                    list.Sort ((a, b) => b.Key.CompareTo (a.Key)) ;

                    foreach (var kv in list)
                        if (vtts.ContainsKey (kv.Value))
                        {
                            UseLang = kv.Value ;
                            goto selected ;
                        }
                }

                // no language could be selected based on preferences or Accept-Language, fall back
                if (vtts.ContainsKey ("en"))
                {
                    UseLang = "en" ;
                }
                else
                    UseLang = vtts.First ().Key ;
            }
            while (false) ;
            else
            if (!vtts.ContainsKey   (UseLang))
                throw new Exception ("This video has no subtitles in " + UseLang) ;

        selected:
            AdjustVttCookies        (vtts, videoId, UseLang, title) ;
            return RedirectToPage   ("Subtitles", new { VideoId = videoId, Lang = UseLang }) ;
        }
    }
}
