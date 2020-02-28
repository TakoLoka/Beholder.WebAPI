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
using WebAPI.Services.Cache;

namespace WebAPI.Controllers
{
    public class DocumentationController : Controller
    {
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly ICacheService _cacheService;

        public DocumentationController(IHostingEnvironment hostEnvironment, ICacheService cacheService)
        {
            _hostEnvironment = hostEnvironment;
            _cacheService = cacheService;
        }
        public IActionResult Index()
        {
            DocumentationModel model = JsonConvert.DeserializeObject<DocumentationModel>(System.IO.File.ReadAllText(_hostEnvironment.WebRootPath + "/Docs/battlemap_doc.json"));
            return View(model);
        }
    }
}