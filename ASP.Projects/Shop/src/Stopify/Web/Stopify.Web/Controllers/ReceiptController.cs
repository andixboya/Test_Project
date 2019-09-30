using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Services.Models;
using Stopify.Web.ViewModels.Receipt.Details;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stopify.Web.Controllers
{
    public class ReceiptController : Controller
    {

        private readonly IReceiptService receiptService;
        public ReceiptController(IReceiptService receiptService)
        {
            this.receiptService = receiptService;
        }

        [HttpGet]
        [Route("/Receipt/Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            ReceiptServiceModel receiptServiceModel = await this.receiptService.GetAll()
                .SingleOrDefaultAsync(receipt => receipt.Id == id);

            ReceiptDetailsViewModel receiptDetailsViewModel = receiptServiceModel.To<ReceiptDetailsViewModel>();

            return this.View(receiptDetailsViewModel);
        }
    }
}
