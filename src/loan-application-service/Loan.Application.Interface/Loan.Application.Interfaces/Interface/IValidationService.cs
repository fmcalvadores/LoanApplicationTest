using Loan.Application.Commons;
using System;
using System.Threading.Tasks;

namespace Loan.Application.Interfaces
{
    public interface IValidationService
    {
        Task<ValidationResult> ValidateFirstName(string fname, string fldName);

        Task<ValidationResult> ValidateLastName(string lname, string fldName);

        Task<ValidationResult> ValidateEmail(string email, string fldName);

        Task<ValidationResult> ValidatePhoneNumber(string pnumber, string fldName);

        Task<ValidationResult> ValidatBusinessNumber(string bnumber, string fldName);

        Task<ValidationResult> ValidateLoanAmount(float amount, string fldName);

        Task<ValidationResult> ValidateCitizenship(string citizenship, string fldName);

        Task<ValidationResult> ValidateTradingTime(DateTime tradingTime, string fldName);

        Task<ValidationResult> ValidateCountryCode(string countryCode, string fldName);

        Task<ValidationResult> ValidateIndustry(string industry, string fldName);
    }
}
