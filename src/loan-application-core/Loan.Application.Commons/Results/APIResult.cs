using System;
using System.Collections.Generic;
using System.Text;

namespace Loan.Application.Commons
{
    public class APIResult
    {
        public string Decision { get; set; }
        public List<ValidationResult> ValidationResults { get; set; }

        public APIResult()
        {
            Decision = "";
            ValidationResults = new List<ValidationResult>();
        }
    }
}
