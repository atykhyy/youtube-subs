﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace youtube_subs
{
    public static class Languages
    {
        public static SelectListItem[] Items => new[]
        {
            new SelectListItem ("English", "en"),
            new SelectListItem ("Chinese (traditional)", "zh-Hant"),
            new SelectListItem ("Chinese (simplified)",  "zh-Hans"),

            // curl -Lk https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes > List_of_ISO_639-1_codes
            // for /F %i in (languages.txt) do @for /F %j in ^
            //   ('xpath -h "/html/body/div[3]/div[3]/div[4]/div/table[2]/tbody/tr[td[5]//a/text()='%i']" "td[3]/a/text()" ^< List_of_ISO_639-1_codes') do^
            //   @echo.            new SelectListItem ("%j", "%i"),
            new SelectListItem ("Afrikaans", "af"),
            new SelectListItem ("Albanian", "sq"),
            new SelectListItem ("Amharic", "am"),
            new SelectListItem ("Arabic", "ar"),
            new SelectListItem ("Armenian", "hy"),
            new SelectListItem ("Azerbaijani", "az"),
            new SelectListItem ("Bengali", "bn"),
            new SelectListItem ("Basque", "eu"),
            new SelectListItem ("Belarusian", "be"),
            new SelectListItem ("Bosnian", "bs"),
            new SelectListItem ("Bulgarian", "bg"),
            new SelectListItem ("Burmese", "my"),
            new SelectListItem ("Catalan", "ca"),
            new SelectListItem ("Corsican", "co"),
            new SelectListItem ("Croatian", "hr"),
            new SelectListItem ("Czech", "cs"),
            new SelectListItem ("Danish", "da"),
            new SelectListItem ("Dutch", "nl"),
            new SelectListItem ("English", "en"),
            new SelectListItem ("Esperanto", "eo"),
            new SelectListItem ("Estonian", "et"),
            new SelectListItem ("Finnish", "fi"),
            new SelectListItem ("French", "fr"),
            new SelectListItem ("Galician", "gl"),
            new SelectListItem ("Georgian", "ka"),
            new SelectListItem ("German", "de"),
            new SelectListItem ("Greek", "el"),
            new SelectListItem ("Gujarati", "gu"),
            new SelectListItem ("Haitian", "ht"),
            new SelectListItem ("Hausa", "ha"),
            new SelectListItem ("Hindi", "hi"),
            new SelectListItem ("Hungarian", "hu"),
            new SelectListItem ("Icelandic", "is"),
            new SelectListItem ("Igbo", "ig"),
            new SelectListItem ("Indonesian", "id"),
            new SelectListItem ("Irish", "ga"),
            new SelectListItem ("Italian", "it"),
            new SelectListItem ("Japanese", "ja"),
            new SelectListItem ("Javanese", "jv"),
            new SelectListItem ("Kannada", "kn"),
            new SelectListItem ("Kazakh", "kk"),
            new SelectListItem ("Central", "km"),
            new SelectListItem ("Kinyarwanda", "rw"),
            new SelectListItem ("Korean", "ko"),
            new SelectListItem ("Kurdish", "ku"),
            new SelectListItem ("Kirghiz", "ky"),
            new SelectListItem ("Lao", "lo"),
            new SelectListItem ("Latin", "la"),
            new SelectListItem ("Latvian", "lv"),
            new SelectListItem ("Lithuanian", "lt"),
            new SelectListItem ("Luxembourgish", "lb"),
            new SelectListItem ("Macedonian", "mk"),
            new SelectListItem ("Malagasy", "mg"),
            new SelectListItem ("Malay", "ms"),
            new SelectListItem ("Malayalam", "ml"),
            new SelectListItem ("Maltese", "mt"),
            new SelectListItem ("Maori", "mi"),
            new SelectListItem ("Marathi", "mr"),
            new SelectListItem ("Mongolian", "mn"),
            new SelectListItem ("Nepali", "ne"),
            new SelectListItem ("Norwegian", "no"),
            new SelectListItem ("Chichewa", "ny"),
            new SelectListItem ("Oriya", "or"),
            new SelectListItem ("Pashto", "ps"),
            new SelectListItem ("Persian", "fa"),
            new SelectListItem ("Polish", "pl"),
            new SelectListItem ("Portuguese", "pt"),
            new SelectListItem ("Punjabi", "pa"),
            new SelectListItem ("Romanian", "ro"),
            new SelectListItem ("Russian", "ru"),
            new SelectListItem ("Samoan", "sm"),
            new SelectListItem ("Gaelic", "gd"),
            new SelectListItem ("Serbian", "sr"),
            new SelectListItem ("Shona", "sn"),
            new SelectListItem ("Sindhi", "sd"),
            new SelectListItem ("Sinhala", "si"),
            new SelectListItem ("Slovak", "sk"),
            new SelectListItem ("Slovenian", "sl"),
            new SelectListItem ("Somali", "so"),
            new SelectListItem ("Southern", "st"),
            new SelectListItem ("Spanish", "es"),
            new SelectListItem ("Sundanese", "su"),
            new SelectListItem ("Swahili", "sw"),
            new SelectListItem ("Swedish", "sv"),
            new SelectListItem ("Tajik", "tg"),
            new SelectListItem ("Tamil", "ta"),
            new SelectListItem ("Tatar", "tt"),
            new SelectListItem ("Telugu", "te"),
            new SelectListItem ("Thai", "th"),
            new SelectListItem ("Turkish", "tr"),
            new SelectListItem ("Turkmen", "tk"),
            new SelectListItem ("Ukrainian", "uk"),
            new SelectListItem ("Urdu", "ur"),
            new SelectListItem ("Uighur", "ug"),
            new SelectListItem ("Uzbek", "uz"),
            new SelectListItem ("Vietnamese", "vi"),
            new SelectListItem ("Welsh", "cy"),
            new SelectListItem ("Western", "fy"),
            new SelectListItem ("Xhosa", "xh"),
            new SelectListItem ("Yiddish", "yi"),
            new SelectListItem ("Yoruba", "yo"),
            new SelectListItem ("Zulu", "zu"),

            // languages not present in the 639-2 column
            new SelectListItem ("Pilipino", "fil"),
            new SelectListItem ("Hawaiian", "haw"),
            new SelectListItem ("Hmong",    "hmn"),
        } ;
    }
}
