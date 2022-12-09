using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loan.Application.Web.Controllers
{
    public class LoanApplicationController : Controller
    {
        public IActionResult PreAssessment()
        {
            ViewData.Add("Test", "Test Data");
            return View();
        }
    }
}
