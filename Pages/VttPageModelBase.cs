using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace youtube_subs.Pages
{
    public class VttPageModelBase : PageModel
    {
        protected void AdjustVttCookies (Dictionary<string, string> vtts, string videoId, string lang, string title)
        {
            foreach (var key in Request.Cookies.Keys)
                if (key.StartsWith ("vtt-"))
                    Response.Cookies.Delete (key) ;

            Response.Cookies.Append ($"vtt-{videoId}-langs", String.Join (',', vtts.Keys)) ;
            Response.Cookies.Append ($"vtt-{videoId}-url",   vtts[lang]) ;
            Response.Cookies.Append ($"vtt-{videoId}-title", title ?? $"#{videoId}") ;
        }
    }
}
