using System;
using System.Collections.Generic;
using System.Text;

namespace Loan.Application.Commons
{
    public class ValidationResult
    {
        public string Rule { get; set; }

        public string Message { get; set; }

        public string Decision { get; set; }

        public ValidationResult()
        {
            Rule = "";
            Message = "";
            Decision = "";

        }

    }
}
