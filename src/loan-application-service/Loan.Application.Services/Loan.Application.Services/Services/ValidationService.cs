using Loan.Application.Commons;
using Loan.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Application.Services
{
    public class ValidationService : BaseService, IValidationService
    {
        public ValidationService(ILogger<PreAssessmentService> logger) : base(logger) { }

        public async Task<ValidationResult> ValidateFirstName(string fname, string fldName)
        {
            var response = new ValidationResult();
            try
            {
                if (string.IsNullOrEmpty(fname))
                {
                    response.Message = "Please enter your First Name";
                    response.Rule = fldName;
                    response.Decision = Constants.UNQUALIFIED;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidateFirstName: {0}", e.Message);
                response.Message = "Error calling ValidateFirstName";
                response.Rule = fldName;
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }

        public async Task<ValidationResult> ValidateLastName(string lname, string fldName)
        {
            var response = new ValidationResult();
            try
            {
                if (string.IsNullOrEmpty(lname))
                {
                    response.Message = "Please enter your Last Name";
                    response.Rule = fldName;
                    response.Decision = Constants.UNQUALIFIED;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidateLastName: {0}", e.Message);
                response.Message = "Error calling ValidateLastName";
                response.Rule = fldName;
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }

        public async Task<ValidationResult> ValidateEmail(string email, string fldName)
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResult> ValidatBusinessNumber(string bnumber, string fldName)
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResult> ValidateCitizenship(string citizenship, string fldName)
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResult> ValidateCountryCode(string countryCode, string fldName)
        {
            throw new NotImplementedException();
        }


        public async Task<ValidationResult> ValidateIndustry(string industry, string fldName)
        {
            throw new NotImplementedException();
        }


        public async Task<ValidationResult> ValidateLoanAmount(float amount, string fldName)
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResult> ValidatePhoneNumber(string pnumber, string fldName)
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResult> ValidateTradingTime(DateTime tradingTime, string fldName)
        {
            throw new NotImplementedException();
        }
    }
}
