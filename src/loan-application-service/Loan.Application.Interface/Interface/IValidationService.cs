
using Loan.Application.Commons;
using Loan.Application.DTO;
using System.Threading.Tasks;

namespace Loan.Application.Interfaces
{
    public interface IValidationService
    {
        ValidationResult ValidateFirstName(string fname);

        ValidationResult ValidateLastName(string lname);

        ValidationResult ValidateEmail(string email);

        ValidationResult ValidatePhoneNumber(string pnumber);

        Task<ValidationResult> ValidatBusinessNumber(string bnumber, bool isExisting);

        ValidationResult ValidateLoanAmount(double amount);

        ValidationResult ValidateCitizenship(string citizenship);

        ValidationResult ValidateTradingTime(int tradingTime);

        ValidationResult ValidateCountryCode(string countryCode);

        ValidationResult ValidateIndustry(string industry);

        ValidationResult ValidateApplication(string filepath, ApplicantDTO dto);
    }
}
