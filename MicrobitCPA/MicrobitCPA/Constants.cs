using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobitCPA
{
    public static class Constants
    {
        public static readonly string AuthenticationTokenEndpoint = "https://api.cognitive.azure.cn/emotion/v1.0/recognize?";

        public static readonly string BingSpeechApiKey = "<INSERT_API_KEY_HERE>";
        public static readonly string SpeechRecognitionEndpoint = "https://speech.platform.bing.com/recognize";
        public static readonly string AudioContentType = @"audio/wav; codec=""audio/pcm""; samplerate=16000";

        public static readonly string BingSpellCheckApiKey = "<INSERT_API_KEY_HERE>";
        public static readonly string BingSpellCheckEndpoint = "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/";

        public static readonly string TextTranslatorApiKey = "<INSERT_API_KEY_HERE>";
        public static readonly string TextTranslatorEndpoint = "https://api.microsofttranslator.com/v2/http.svc/Translate";

        //Azure for China
        //"https://api.cognitive.azure.cn/emotion/v1.0/recognize?"; 
        //"9c1e3b2860d8440c981ff1077c4cb6c2";

        public static readonly string EmotionApiEndpoint = "https://westus.api.cognitive.microsoft.com/emotion/v1.0";

        public static readonly string EmotionApiKey = "d219be8c554b4163b67803a36472c37c";

        public static readonly string AudioFilename = "Todo.wav";
    }
}
