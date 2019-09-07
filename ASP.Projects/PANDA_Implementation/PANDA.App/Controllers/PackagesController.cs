

namespace PANDA.App.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PANDA.App.Models.Package;
    using Panda.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PANDA.Data;



    public class PackagesController : Controller
    {

        private readonly PandaDbContextThree context;

        public PackagesController(PandaDbContextThree context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PackageCreateInputModel();
            model.Users = this.context.Users.Select(u => u.UserName).ToList();

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Create(PackageCreateInputModel inputModel)
        {
            
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Packages/Create");
            }

            var userId = this.context.Users.FirstOrDefault(u => u.UserName == inputModel.Recipient).Id;

            var package = new Package()
            {
                RecipientId = userId,
                Description = inputModel.Description,
                ShippingAddress = inputModel.ShippingAddress,
                Status = this.context.PackageStatuses.FirstOrDefault(s => s.Name == "Pending"),
                Weight = inputModel.Weight,
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(20)
            };

            this.context.Packages.Add(package);
            this.context.SaveChanges();

            return this.Redirect("/Packages/Pending");
        }

        [HttpGet]
        public IActionResult Delivered()
        {
            var viewModel = new PackageDeliveredViewModel();

            viewModel.Packages = this.context.Packages.Where(p => p.Status.Name == "Delivered")
                .Select(p => new SinglePackageDeliveredViewModel()
                {
                    Description = p.Description,
                    RecipientName = p.Recipient.UserName,
                    ShippingAddress = p.ShippingAddress,
                    Weight = p.Weight
                }).ToList();

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult Pending()
        {
            var viewModel = new PackageDeliveredViewModel();

            viewModel.Packages = this.context.Packages.Where(p => p.Status.Name == "Pending")
                .Select(p => new SinglePackageDeliveredViewModel()
                {
                    Description = p.Description,
                    RecipientName = p.Recipient.UserName,
                    ShippingAddress = p.ShippingAddress,
                    Weight = p.Weight,
                    Id=p.Id

                }).ToList();

            return this.View(viewModel);
        }

    }
}
