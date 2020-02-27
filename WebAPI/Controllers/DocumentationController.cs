using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.Models.Documentation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;

namespace WebAPI.Controllers
{
    public class DocumentationController : Controller
    {
        private readonly IHostingEnvironment _hostEnvironment; 
        private readonly IDistributedCache _distributedCache;

        public DocumentationController(IHostingEnvironment hostEnvironment, IDistributedCache distributedCache)
        {
            _hostEnvironment = hostEnvironment;
            _distributedCache = distributedCache;
        }
        public IActionResult Index()
        {
            DocumentationModel model = JsonConvert.DeserializeObject<DocumentationModel>(System.IO.File.ReadAllText(_hostEnvironment.WebRootPath + "/Docs/battlemap_doc.json"));
            return View(model);
        }
        [Route("test/redis")]
        public IActionResult TestRedis()
        {
            var cacheKey = "TheTime";
            var existingTime = _distributedCache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(existingTime))
            {
                return Ok("Fetched from cache : " + existingTime);
            }
            else
            {
                existingTime = DateTime.UtcNow.ToString();
                _distributedCache.SetString(cacheKey, existingTime);
                return Ok("Added to cache : " + existingTime);
            }
        }
    }
}