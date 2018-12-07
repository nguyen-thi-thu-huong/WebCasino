﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebCasino.Service.Abstract;
using WebCasino.Web.Areas.Administration.Models;

namespace WebCasino.Web.Areas.Administration.Controllers
{
	[Area("Administration")]
	public class UserAdministrationController : Controller
	{
		private readonly IUserService userService;
        private readonly ITransactionService transactionService;

        public UserAdministrationController(IUserService service, ITransactionService transactionService)
		{
			this.userService = service ?? throw new System.ArgumentNullException(nameof(service));
            this.transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }

		public async Task<IActionResult> Index(UsersIndexViewModel model)
		{
			if (string.IsNullOrWhiteSpace(model.SearchText))
			{
				model.Users = await this.userService.GetAllUsers(model.Page, 10);
				model.TotalPages = (int)Math.Ceiling(await this.userService.Total() / (double)10);
			}
			else
			{
				model.Users = this.userService.ListByContainingText(model.SearchText, model.Page, 10);
				model.TotalPages = (int)Math.Ceiling(this.userService.TotalContainingText(model.SearchText) / (double)10);
			}

			return View(model);
		}

        [HttpGet]
        public async Task<IActionResult> Details(UserViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SearchText))
            {
                model.Transactions = await this.transactionService.RetrieveUserTransaction(model.Id, model.Page, 10);
                model.User = model.Transactions.Select(u => u.User).First();

                model.TotalPages = (int)Math.Ceiling(await this.transactionService.Total() / (double)10);
            }
            else
            {
                model.Transactions = await this.transactionService.RetrieveUserSearchTransaction(model.SearchText,model.Id, model.Page, 10);
                model.TotalPages = (int)Math.Ceiling(await this.transactionService.TotalContainingText(model.SearchText) / (double)10);

            }

            return View(model);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUser(string id)
        {

            var removedTransaction = await this.userService.LockUser(id);

            this.TempData["Deleted"] = "You Lock this user";

            return RedirectToAction("History", "Transactions");
        }


    }
}