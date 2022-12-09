using Loan.Application.Commons;
using Loan.Application.DTO;
using Loan.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "qualified-accounts.csv"); //File location for qualified accounts
                bool isExisting = false;
                //validate if payload (businessNumber) is already verified or not
                validationResult = _validationService.ValidateApplication(filePath, dto);
                if (validationResult.Decision != string.Empty)
                {
                    result.Decision = validationResult.Decision;
                    result.ValidationResults = (string.IsNullOrEmpty(validationResult.Message) ? null : DeconstructMessage(validationResult.Message));
                    isExisting = true;
                    //return result;
                }

                //validate FirstName
                validationResult = _validationService.ValidateFirstName(dto.FirstName);
                if (validationResult.Decision != Constants.QUALIFIED)
                {
                    validationResult.Rule = "FirstName";
                    validationList.Add(validationResult);
                }

                //validate LastName
                validationResult = _validationService.ValidateLastName(dto.LastName);
                if (validationResult.Decision != Constants.QUALIFIED)
                {
                    validationResult.Rule = "LastName";
                    validationList.Add(validationResult);
                }

                //validate EmailAddress
                validationResult = _validationService.ValidateEmail(dto.EmailAddress);
                if (validationResult.Decision != Constants.QUALIFIED)
                {
                    validationResult.Rule = "EmailAddress";
                    validationList.Add(validationResult);
                }

                //validate PhoneNumber
                validationResult = _validationService.ValidatePhoneNumber(dto.PhoneNumber);
                if (validationResult.Decision != Constants.QUALIFIED)
                {
                    validationResult.Rule = "PhoneNumber";
                    validationList.Add(validationResult);
                }

                //validate BusinessNumber
                validationResult = await _validationService.ValidatBusinessNumber(dto.BusinessNumber, isExisting);
                if (validationResult.Decision != Constants.QUALIFIED)
                {
                    validationResult.Rule = "BusinessNumber";
                    validationList.Add(validationResult);
                }

                //validate LoanAmount
                validationResult = _validationService.ValidateLoanAmount(dto.LoanAmount);
                if (validationResult.Decision != Constants.QUALIFIED)
                {
                    validationResult.Rule = "LoanAmount";
                    validationList.Add(validationResult);
                }

                //validate Citizenship
                validationResult = _validationService.ValidateCitizenship(dto.Citizenship);
                if (validationResult.Decision != Constants.QUALIFIED)
                {
                    validationResult.Rule = "Citizenship";
                    validationList.Add(validationResult);
                }

                //validate TimeTrading
                validationResult = _validationService.ValidateTradingTime(dto.TimeTrading);
                if (validationResult.Decision != Constants.QUALIFIED)
                {
                    validationResult.Rule = "TimeTrading";
                    validationList.Add(validationResult);
                }

                //validate CountryCode
                validationResult = _validationService.ValidateCountryCode(dto.CountryCode);
                if (validationResult.Decision != Constants.QUALIFIED)
                {
                    validationResult.Rule = "CountryCode";
                    validationList.Add(validationResult);
                }

                //validate Industry
                validationResult = _validationService.ValidateIndustry(dto.Industry);
                if (validationResult.Decision != Constants.QUALIFIED)
                {
                    validationResult.Rule = "Industry";
                    validationList.Add(validationResult);
                }



                //Filtering the final result and save to csv file
                if (validationList.Count() <= 0)
                {
                    result.Decision = Constants.QUALIFIED;
                    result.ValidationResults = null;

                }
                else
                {
                    result.Decision = Constants.UNQUALIFIED;
                    result.ValidationResults = validationList;
                }

                SaveQualifiedToFile(filePath, dto, (validationList.Count() <= 0 ? Constants.QUALIFIED : Constants.UNQUALIFIED), validationList, isExisting);

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

        //Temporary Storage Location for Examination purposes only
        private void SaveQualifiedToFile(string filePath, ApplicantDTO dto, string verdict, List<ValidationResult> validationResults, bool isExisting)
        {
            var message = FormatMessage(validationResults);
            if (!File.Exists(filePath))
            {
                using var stream = File.CreateText(filePath);
                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", "FIRST NAME", "LAST NAME", "EMAIL ADDRESS", "PHONE NUMBER", "BUSINESS NUMBER", "LOAN AMOUNT", "CITIZENSHIP", "TIME TRADING", "COUNTRY CODE", "INDUSTRY", "VERDICT", "MESSAGE", "DATE REQUESTED"));
                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", dto.FirstName, dto.LastName, dto.EmailAddress, dto.PhoneNumber, dto.BusinessNumber, dto.LoanAmount, dto.Citizenship, dto.TimeTrading, dto.CountryCode, dto.Industry, verdict, message, DateTime.Now));
            }
            else
            {
                if (isExisting)
                {
                    var qualifiedList = File.ReadAllLines(filePath);
                    var updatedList = new List<string>();
                    for (var i = 0; i <= qualifiedList.Count() - 1; i++)
                    {
                        var row = qualifiedList[i].Split(",");
                        if (row.Contains(dto.BusinessNumber))
                        {
                            updatedList.Add(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", dto.FirstName, dto.LastName, dto.EmailAddress, dto.PhoneNumber, dto.BusinessNumber, dto.LoanAmount, dto.Citizenship, dto.TimeTrading, dto.CountryCode, dto.Industry, verdict, message, DateTime.Now));
                        }
                        else
                        {
                            updatedList.Add(qualifiedList[i]);

                        }
                    }
                    File.WriteAllLines(filePath, updatedList);
                }
                else
                {
                    File.AppendAllText(filePath, string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}" + Environment.NewLine, dto.FirstName, dto.LastName, dto.EmailAddress, dto.PhoneNumber, dto.BusinessNumber, dto.LoanAmount, dto.Citizenship, dto.TimeTrading, dto.CountryCode, dto.Industry, verdict, message, DateTime.Now));
                }

            }
        }

        private string FormatMessage(List<ValidationResult> validationResults)
        {
            var message = string.Empty;
            if (validationResults.Count() > 0)
            {
                foreach (var result in validationResults)
                {
                    message += string.Format("{0}|{1}|{2}&", result.Rule, result.Message, result.Decision);
                }
                message = message.Remove(message.Length - 1, 1);
            }
            return message;
        }

        private List<ValidationResult> DeconstructMessage(string message)
        {
            var list = new List<ValidationResult>();
            var messageArr = message.Split("&");
            foreach (var msg in messageArr)
            {
                var validationRes = msg.Split("|");
                list.Add(new ValidationResult()
                {
                    Rule = validationRes[0],
                    Message = validationRes[1],
                    Decision = validationRes[2],
                });
            }

            return list;
        }
    }
}
