using System;

namespace Loan.Application.DTO
{
    public class ApplicantDTO
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string BusinessNumber { get; set; }

        public string LoanAmount { get; set; }

        public int Citizenship { get; set; }

        public DateTime TimeTrading { get; set; }

        public string CountryCode { get; set; }

        public int Industry { get; set; }
    }
}

