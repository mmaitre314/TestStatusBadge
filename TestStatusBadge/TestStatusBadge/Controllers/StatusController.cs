using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace TestStatusBadge.Controllers
{
    public class StatusController : ApiController
    {
        // GET: api/status/{account}/{project}
        public async Task<HttpResponseMessage> GetAsync(String account, String project)
        {
            int passedTestsCount = 0;
            int failedTestsCount = 0;

            // Query AppVeyor REST API for test results
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = await client.GetAsync("https://ci.appveyor.com/api/projects/" + account + "/" + project))
                {
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsAsync<JToken>();

                    // Aggregate test results
                    foreach (JToken job in json["build"]["jobs"])
                    {
                        passedTestsCount += job.Value<int>("passedTestsCount");
                        failedTestsCount += job.Value<int>("failedTestsCount");
                    }
                }
            }

            // Choose SVG text and color based on test results
            String svgColor;
            String svgText;
            if (failedTestsCount > 0)
            {
                svgColor = "#FF4242"; // Some test failed -> red
                svgText = "Test failing";
            }
            else if (passedTestsCount == 0)
            {
                svgColor = "#FF8000"; // No tests passed -> orange
                svgText = "No test";
            }
            else
            {
                svgColor = "#42CC42"; // All tests passed -> green
                svgText = "Test passing";
            }

            // Create the SVG image
            String svg =
            "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"102px\" height=\"18px\" style=\"shape-rendering:geometricPrecision; text-rendering:geometricPrecision; image-rendering:optimizeQuality; fill-rule:evenodd; clip-rule:evenodd\">" +
            "  <rect fill=\"" + svgColor + "\" width=\"102px\" height=\"18\" rx=\"2\" ry=\"2\"/>" +
            "  <g transform=\"scale(0.045)\">" +
            "    <path fill=\"#fff\" d=\"M242 48c86,0 155,69 155,154 0,86 -69,155 -155,155 -85,0 -154,-69 -154,-155 0,-85 69,-154 154,-154zm38 184c-17,22 -48,26 -69,9 -21,-16 -24,-47 -7,-69 18,-21 49,-25 70,-9 21,17 24,48 6,69zm-82 101l59 -57c-22,5 -45,1 -63,-14 -21,-16 -30,-43 -27,-68l-53 58c0,0 -7,-13 -9,-37l93 -73c28,-20 66,-21 93,0 30,24 36,68 14,101l-68 97c-10,0 -30,-3 -39,-7z\"/>" +
            "  </g>" +
            "  <text x=\"22\" y=\"13\" fill=\"#fff\" font-family=\"DejaVu Sans,Verdana,Geneva,sans-serif\" font-size=\"11px\">" + svgText + "</text>" +
            "</svg>";

            // Return the SVG image to the caller
            var message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(svg);
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("image/svg+xml");
            return message;
        }
    }
}
