using Loan.Application.Commons;
using Loan.Application.DTO;
using Loan.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Application.Services
{
    public class PreAssessmentService : BaseService, IPreAssessmentService
    {
        private readonly IValidationService _validationService;

        public PreAssessmentService(ILogger<PreAssessmentService> logger, IValidationService validationService) : base(logger)
        {
            _validationService = validationService;
        }
        public async Task<Result> AssessApplicant(ApplicantDTO dto)
        {
            var result = new Result();
            var validationList = new List<ValidationResult>();
            try
            {
                var validationResult = new ValidationResult();

                validationResult = await _validationService.ValidateFirstName(dto.FirstName, "FirstName");
                if (validationResult.Decision == Constants.UNQUALIFIED) { validationList.Add(validationResult); }

                validationResult = await _validationService.ValidateLastName(dto.LastName,"LastName");
                if (validationResult.Decision == Constants.UNQUALIFIED) { validationList.Add(validationResult); }

                result.Decision = (validationList.Count() <= 0) ? Constants.QUALIFIED : Constants.UNQUALIFIED;
                result.ValidationResults = (validationList.Count() <= 0) ? null : validationList;
                
            }
            catch (Exception e)
            {
                _logger.LogError("Error calling AssessApplicant: {0}", e.Message);
                result.Decision = Constants.UNKNOWN;
                validationList.Add(new ValidationResult()
                {
                    Rule = "",
                    Decision = Constants.UNKNOWN,
                    Message = "Error calling AssessApplicant"
                });
                result.ValidationResults = validationList;
            }

            return result;
        }
    }
}
