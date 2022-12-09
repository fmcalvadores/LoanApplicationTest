using Loan.Application.Commons;
using Loan.Application.DTO;
using Loan.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Loan.Application.Services
{
    public class ValidationService : BaseService, IValidationService
    {
        private readonly IOptions<LocalNumbersOptions> _localNumbersOptions;
        private readonly IOptions<IndustryOptions> _industryOptions;
        private readonly IOptions<LoanAmountOptions> _loanAmountOptions;
        private readonly IOptions<CitizenshipOptions> _citizenshipOptions;
        private readonly IOptions<TradingTimeOptions> _tradingTimeOptions;
        private readonly IOptions<CountryCodeOptions> _countryCodeOptions;
        private readonly IOptions<BusinessNumberOptions> _businessNumberOptions;



        public ValidationService(ILogger<PreAssessmentService> logger,
            IOptions<LocalNumbersOptions> localNumbersOptions,
            IOptions<IndustryOptions> industryOptions,
            IOptions<LoanAmountOptions> loanAmountOptions,
            IOptions<CitizenshipOptions> citizenshipOptions,
            IOptions<TradingTimeOptions> tradingTimeOptions,
            IOptions<CountryCodeOptions> countryCodeOptions,
            IOptions<BusinessNumberOptions> businessNumberOptions
            ) : base(logger)
        {

            _localNumbersOptions = localNumbersOptions;
            _industryOptions = industryOptions;
            _loanAmountOptions = loanAmountOptions;
            _citizenshipOptions = citizenshipOptions;
            _tradingTimeOptions = tradingTimeOptions;
            _countryCodeOptions = countryCodeOptions;
            _businessNumberOptions = businessNumberOptions;
        }

        public ValidationResult ValidateFirstName(string fname)
        {
            var response = new ValidationResult();
            try
            {
                if (string.IsNullOrEmpty(fname))
                {
                    response.Message = "Please enter your First Name";
                    response.Decision = Constants.UNQUALIFIED;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidateFirstName: {0}", e.Message);
                response.Message = "Error calling ValidateFirstName";
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }

        public ValidationResult ValidateLastName(string lname)
        {
            var response = new ValidationResult();
            try
            {
                if (string.IsNullOrEmpty(lname))
                {
                    response.Message = "Please enter your Last Name";
                    response.Decision = Constants.UNQUALIFIED;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidateLastName: {0}", e.Message);
                response.Message = "Error calling ValidateLastName";
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }

        public ValidationResult ValidateEmail(string email)
        {
            var response = new ValidationResult();
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    response.Message = "Please enter your Email Address";
                    response.Decision = Constants.UNQUALIFIED;
                }
                else
                {
                    if (!IsValidEmail(email))
                    {
                        response.Message += "Invalid Email Address.";
                        response.Decision = Constants.UNQUALIFIED;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidateEmail: {0}", e.Message);
                response.Message = "Error calling ValidateEmail";
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }

        private bool IsValidEmail(string email)
        {
            //string regex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            string regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }

        public ValidationResult ValidatePhoneNumber(string pnumber)
        {
            var response = new ValidationResult();
            try
            {
                if (string.IsNullOrEmpty(pnumber))
                {
                    response.Message = "Please enter your Phone Number";
                    response.Decision = Constants.UNQUALIFIED;
                }
                else
                {

                    var pnumberLocals = _localNumbersOptions.Value.LocalsNumber;

                    if (!pnumberLocals.Any(pnumber.StartsWith))
                    {
                        response.Message += "Invalid Phone Number.";
                        response.Decision = Constants.UNQUALIFIED;
                    }
                    else
                    {
                        foreach (var numLocal in pnumberLocals)
                        {
                            if (pnumber.StartsWith(numLocal))
                            {
                                pnumber = pnumber.Replace(numLocal, "");
                                break;
                            }
                        }

                        if (pnumber.Count() != _localNumbersOptions.Value.PhoneNumberMaxLength && !pnumber.All(char.IsDigit))
                        {
                            response.Message += "Invalid Phone Number.";
                            response.Decision = Constants.UNQUALIFIED;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidatePhoneNumber: {0}", e.Message);
                response.Message = "Error calling ValidatePhoneNumber";
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }

        public async Task<ValidationResult> ValidatBusinessNumber(string bnumber, bool isExisting)
        {
            var response = new ValidationResult();
            try
            {
                if (!isExisting)
                {
                    await Task.Delay(_businessNumberOptions.Value.DelayCount);
                }

                if (string.IsNullOrEmpty(bnumber) || bnumber.Count() != _businessNumberOptions.Value.MaxLength || !bnumber.All(char.IsDigit))
                {
                    response.Message = "Invalid Bussiness Number.";
                    response.Decision = Constants.UNQUALIFIED;
                }

            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidatBusinessNumber: {0}", e.Message);
                response.Message = "Error calling ValidatBusinessNumber";
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }

        public ValidationResult ValidateLoanAmount(double amount)
        {
            var response = new ValidationResult();
            try
            {
                var minLoan = _loanAmountOptions.Value.MinAmount;
                var maxLoan = _loanAmountOptions.Value.MaxAmount;

                if (!(minLoan <= amount && amount <= maxLoan))
                {
                    response.Message = "Invalid Loan Amount.";
                    response.Decision = Constants.UNQUALIFIED;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidateLoanAmount: {0}", e.Message);
                response.Message = "Error calling ValidateLoanAmount";
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }

        public ValidationResult ValidateCitizenship(string citizenship)
        {
            var response = new ValidationResult();
            try
            {
                var citizenshipList = _citizenshipOptions.Value.Options;

                if (!citizenshipList.Contains(citizenship, StringComparer.OrdinalIgnoreCase))
                {
                    response.Message = "Invalid Citizenship.";
                    response.Decision = Constants.UNQUALIFIED;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidateCitizenship: {0}", e.Message);
                response.Message = "Error calling ValidateCitizenship";
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }

        public ValidationResult ValidateTradingTime(int tradingTime)
        {
            var response = new ValidationResult();
            try
            {
                int maxTime = _tradingTimeOptions.Value.MaxTime;
                int minTime = _tradingTimeOptions.Value.MinTime;

                if (!(minTime <= tradingTime && tradingTime <= maxTime))
                {
                    response.Message = "Invalid Time Trading value.";
                    response.Decision = Constants.UNQUALIFIED;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidateTradingTime: {0}", e.Message);
                response.Message = "Error calling ValidateTradingTime";
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }

        public ValidationResult ValidateCountryCode(string countryCode)
        {
            var response = new ValidationResult();
            try
            {
                string defaultCountryCode = _countryCodeOptions.Value.CountryCode.ToLower();

                if (!defaultCountryCode.Equals(countryCode.ToLower()))
                {
                    response.Message = "Invalid Country Code.";
                    response.Decision = Constants.UNQUALIFIED;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidateCountryCode: {0}", e.Message);
                response.Message = "Error calling ValidateCountryCode";
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }


        public ValidationResult ValidateIndustry(string industry)
        {
            var response = new ValidationResult();
            try
            {
                var bannedIndustryList = _industryOptions.Value.BannedIndustries;
                var allowedIndustryList = _industryOptions.Value.AllowedIndustries;

                if (bannedIndustryList.Contains(industry, StringComparer.OrdinalIgnoreCase))
                {
                    response.Message = "Selected Industry is Banned";
                    response.Decision = Constants.UNQUALIFIED;
                }
                else
                {
                    if (allowedIndustryList.Contains(industry, StringComparer.OrdinalIgnoreCase))
                    {
                        response.Message = "Selected Industry is Allowed.";
                        response.Decision = Constants.QUALIFIED;
                    }
                    else
                    {
                        response.Message = "Selected Industry is Unknown.";
                        response.Decision = Constants.UNKNOWN;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidateIndustry: {0}", e.Message);
                response.Message = "Error calling ValidateIndustry";
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }

        public ValidationResult ValidateApplication(string filepath, ApplicantDTO dto)
        {
            var response = new ValidationResult();
            try
            {
                if (File.Exists(filepath))
                {
                    var qualifiedList = File.ReadAllLines(filepath);
                    foreach (var qualified in qualifiedList)
                    {
                        var entity = qualified.Split(",");
                        if (entity.Contains(dto.BusinessNumber))
                        {
                            response.Decision = entity[10];
                            response.Message = entity[11];
                            return response;
                        }
                    }
                }

                response.Decision = string.Empty;

            }
            catch (Exception e)
            {
                _logger.LogError("Error calling ValidateIndustry: {0}", e.Message);
                response.Message = "Error calling ValidateIndustry";
                response.Decision = Constants.UNKNOWN;
            }

            return response;
        }
    }
}
