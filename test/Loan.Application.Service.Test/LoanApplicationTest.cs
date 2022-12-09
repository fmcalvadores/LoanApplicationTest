using Loan.Application.Commons;
using Loan.Application.DTO;
using Loan.Application.Interfaces;
using Loan.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.IO;
using Xunit;

namespace Loan.Application.Service.Test
{
    public class LoanApplicationTest
    {

        protected IValidationService _validationService;
        protected IPreAssessmentService _preAssessmentService;
        protected IOptions<BusinessNumberOptions> _businessNumberOptions;
        protected IOptions<CitizenshipOptions> _citizenshipOptions;
        protected IOptions<CountryCodeOptions> _countryCodeOptions;
        protected IOptions<IndustryOptions> _industryOptions;
        protected IOptions<LoanAmountOptions> _loanAmountOptions;
        protected IOptions<LocalNumbersOptions> _localNumbersOptions;
        protected IOptions<TradingTimeOptions> _tradingTimeOptions;

        public LoanApplicationTest()
        {
            var preAssessmentLoger = Mock.Of<ILogger<PreAssessmentService>>();
            var valLoger = Mock.Of<ILogger<ValidationService>>();
            var businessNumberOptions = Mock.Of<IOptions<BusinessNumberOptions>>();
            var citizenshipOptions = Mock.Of<IOptions<CitizenshipOptions>>();
            var countryCodeOptions = Mock.Of<IOptions<CountryCodeOptions>>();
            var industryOptions = Mock.Of<IOptions<IndustryOptions>>();
            var loanAmountOptions = Mock.Of<IOptions<LoanAmountOptions>>();
            var localNumbersOptions = Mock.Of<IOptions<LocalNumbersOptions>>();
            var tradingTimeOptions = Mock.Of<IOptions<TradingTimeOptions>>();

            _validationService = new ValidationService(
                preAssessmentLoger,
                localNumbersOptions,
                industryOptions,
                loanAmountOptions,
                citizenshipOptions,
                tradingTimeOptions,
                countryCodeOptions,
                businessNumberOptions);

            _preAssessmentService = new PreAssessmentService(preAssessmentLoger, _validationService);
        }

        [Fact(DisplayName = "LoanApplicationTest.ValidateFirstNameNull")]
        public void ValidateFirstNameNull()
        {
            Console.WriteLine("LoanApplicationTest ValidateFirstNameNull...");
            var validation = _validationService.ValidateFirstName(null);
            Assert.Equal(Constants.UNQUALIFIED, validation.Decision);
        }

        [Fact(DisplayName = "LoanApplicationTest.ValidateFirstNameEmpty")]
        public void ValidateFirstNameEmpty()
        {
            Console.WriteLine("LoanApplicationTest ValidateFirstNameEmpty...");
            var validation = _validationService.ValidateFirstName("");
            Assert.Equal(Constants.UNQUALIFIED, validation.Decision);
        }

        [Fact(DisplayName = "LoanApplicationTest.ValidateFirstName")]
        public void ValidateFirstName()
        {
            Console.WriteLine("LoanApplicationTest ValidateFirstName...");
            var validation = _validationService.ValidateFirstName("Francis");
            Assert.Equal(Constants.QUALIFIED, validation.Decision);
        }

        [Fact(DisplayName = "LoanApplicationTest.ValidateFirstNameNumber")]
        public void ValidateFirstNameNumber()
        {
            Console.WriteLine("LoanApplicationTest ValidateFirstNameNumber...");
            var validation = _validationService.ValidateFirstName("1");
            Assert.Equal(Constants.QUALIFIED, validation.Decision);
        }

        [Fact(DisplayName = "LoanApplicationTest.ValidateLastNameNull")]
        public void ValidateLastNameNull()
        {
            Console.WriteLine("LoanApplicationTest ValidateLastNameNull...");
            var validation = _validationService.ValidateLastName(null);
            Assert.Equal(Constants.UNQUALIFIED, validation.Decision);
        }

        [Fact(DisplayName = "LoanApplicationTest.ValidateLastNameEmpty")]
        public void ValidateLastNameEmpty()
        {
            Console.WriteLine("LoanApplicationTest ValidateLastNameEmpty...");
            var validation = _validationService.ValidateLastName(" ");
            Assert.Equal(Constants.QUALIFIED, validation.Decision);
        }

        [Fact(DisplayName = "LoanApplicationTest.ValidateEmailNull")]
        public void ValidateEmailNull()
        {
            Console.WriteLine("LoanApplicationTest ValidateEmailNull...");
            var validation = _validationService.ValidateEmail(null);
            Assert.Equal(Constants.UNQUALIFIED, validation.Decision);
        }

        [Fact(DisplayName = "LoanApplicationTest.ValidateEmailEmpty")]
        public void ValidateEmailEmpty()
        {
            Console.WriteLine("LoanApplicationTest ValidateEmailEmpty...");
            var validation = _validationService.ValidateEmail("");
            Assert.Equal(Constants.UNQUALIFIED, validation.Decision);
        }

        [Fact(DisplayName = "LoanApplicationTest.ValidateEmailInvalid")]
        public void ValidateEmailInvalid()
        {
            Console.WriteLine("LoanApplicationTest ValidateEmailInvalid...");
            var validation = _validationService.ValidateEmail("email@email");
            Assert.Equal(Constants.UNQUALIFIED, validation.Decision);
        }

        [Fact(DisplayName = "LoanApplicationTest.ValidateEmailValid")]
        public void ValidateEmailValid()
        {
            Console.WriteLine("LoanApplicationTest ValidateEmailValid...");
            var validation = _validationService.ValidateEmail("email@email.com");
            Assert.Equal(Constants.QUALIFIED, validation.Decision);
        }

        [Fact(DisplayName = "LoanApplicationTest.ValidateApplication")]
        public void ValidateApplication()
        {
            Console.WriteLine("LoanApplicationTest ValidateApplication...");
            var filePath = Path.Combine("D:\\projects\\LoanApplication\\src\\loan-application-api\\Loan.Application.Web.API", "qualified-account.csv");
            var dto = new ApplicantDTO()
            {
                FirstName = "Francis",
                LastName = "Calvadores",
                EmailAddress = "email@email.com",
                PhoneNumber = "+61412345678",
                BusinessNumber = "50110219462",
                LoanAmount = 13,
                Citizenship = "Citizen",
                TimeTrading = 2,
                CountryCode = "AU",
                Industry = "Manufacturing"
            };
            var validation = _validationService.ValidateApplication(filePath, dto);
            Assert.Equal(Constants.QUALIFIED, validation.Decision);
        }

        [Fact(DisplayName = "LoanApplicationTest.RunAssessmentTest")]
        public void RunAssessmentTest()
        {
            var dto = new ApplicantDTO()
            {
                FirstName = "Francis",
                LastName = "Calvadores",
                EmailAddress = "email@email.com",
                PhoneNumber = "+61412345678",
                BusinessNumber = "50110219462",
                LoanAmount = 13,
                Citizenship = "Citizen",
                TimeTrading = 2,
                CountryCode = "AU",
                Industry = "Manufacturing"
            };
            var test = _preAssessmentService.AssessApplicant(dto);
            Assert.Equal(Constants.QUALIFIED, test.Result.Decision);
        }


    }
}
