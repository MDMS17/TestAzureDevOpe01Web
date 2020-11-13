using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestAzureDevOps01Web.Models;

namespace TestAzureDevOps01Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public async Task<List<KeyValuePair<string, int>>> GetVotes()
        {
            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();
            HttpClient httpClient = new HttpClient();
            string proxyUrl = GlobalVariables.ApiUrl;
            using (HttpResponseMessage response = await httpClient.GetAsync(proxyUrl))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var r = await response.Content.ReadAsStringAsync();
                    List<Tuple<string, int>> r2 = JsonConvert.DeserializeObject<List<Tuple<string, int>>>(r);
                    r2.ForEach(x => result.Add(new KeyValuePair<string, int>(x.Item1, x.Item2)));
                }
            }

            return result;
        }
        //Update or add new
        [HttpPost]
        public async Task<IActionResult> UpdateOrAddVote(string itemName)
        {
            HttpClient httpClient = new HttpClient();
            string proxyUrl = GlobalVariables.ApiUrl + @"/" + itemName;

            StringContent putContent = new StringContent($"{{ 'name' : '{itemName}' }}", Encoding.UTF8, "application/json");
            putContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpResponseMessage response = await httpClient.PutAsync(proxyUrl, putContent))
            {
                return new ContentResult()
                {
                    StatusCode = (int)response.StatusCode,
                    Content = await response.Content.ReadAsStringAsync()
                };
            }
        }

        // DELETE
        [HttpPost]
        public async Task<IActionResult> DeleteVote(string itemName)
        {
            HttpClient httpClient = new HttpClient();
            string proxyUrl = GlobalVariables.ApiUrl + @"/" + itemName;

            using (HttpResponseMessage response = await httpClient.DeleteAsync(proxyUrl))
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return this.StatusCode((int)response.StatusCode);
                }
            }

            return new OkResult();
        }
    }
}
