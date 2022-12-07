using Loan.Application.Commons;
using Loan.Application.DTO.DTO;
using Loan.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loan.Application.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanApplicationController: ControllerBase
    {
        private readonly ILogger<LoanApplicationController> _logger;
        private readonly IPreAssessmentService _preAssessmentService;

        public LoanApplicationController(ILogger<LoanApplicationController> logger, IPreAssessmentService preAssessmentService)
        {
            _logger = logger;
            _preAssessmentService = preAssessmentService;
        }

        [HttpGet("hello")]
        public IActionResult GetHelloMessage()
        {
            return Content("Welcome to Loan Application API!");
        }

        [HttpPost("preassessment"), Produces("application/json")]
        public async Task<IActionResult> PreAssessment([FromBody] ApplicantDTO dto)
        {
            var response = await _preAssessmentService.AssessApplicant(dto);
            if (response.Decision == Constants.UNKNOWN || response.Decision == Constants.UNQUALIFIED)
            {
                return BadRequest(new APIResult
                {
                    Decision = response.Decision,
                    ValidationResults = response.ValidationResults
                });
            }

            return Ok(new APIResult
            {
                Decision = response.Decision,
                ValidationResults = response.ValidationResults
            });
        }
    }
}
