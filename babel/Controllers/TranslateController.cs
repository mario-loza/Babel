using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;


namespace babel.Controllers
{
    [Produces("text/plain")]
    [Route("api/Translate")]
    public class TranslateController : Controller
    {
        [HttpPost]
        [Route("This")]
        public async Task<string> TranslateThisAsync([FromBody]JObject value)
        {
            dynamic jsonRequest = value;

            string translatedText = "";

            var url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
            + jsonRequest.sourceLang + "&tl=" + jsonRequest.targetLang + "&dt=t&q=" + jsonRequest.sourceText;

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                // ... Read the response as a string.
                var tr = content.ReadAsStringAsync().Result;
                // ... turn to an Jarray to be easier to select
                JArray ja = JsonConvert.DeserializeObject<JArray>(tr);
                // ... read the data we want
                translatedText = ja[0][0][0].ToString();
            }
            return translatedText;
        }
    }
}