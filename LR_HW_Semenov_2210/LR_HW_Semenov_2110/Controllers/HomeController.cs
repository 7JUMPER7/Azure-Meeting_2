using ClassLib;
using LR_HW_Semenov_2110.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LR_HW_Semenov_2110.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QueueManager queue;

        public HomeController(ILogger<HomeController> logger, QueueManager queue)
        {
            _logger = logger;
            this.queue = queue;

        }

        public async Task<IActionResult> Index()
        {
            return View(await queue.GetLot());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Lot lot)
        {
            await queue.AddLot(lot);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Buy(string messageId, string popReceipt)
        {
            if(await queue.RemoveLot(messageId, popReceipt))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Add");
        }

        public byte[] ImageToByteArray(IFormFile image)
        {
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
