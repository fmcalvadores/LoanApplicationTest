using System.Collections.Generic;

namespace Loan.Application.Commons
{


    public class Result
    {
        public string Decision { get; set; }
        public List<ValidationResult> ValidationResults { get; set; }


        public Result()
        {
            Decision = "";
            ValidationResults = new List<ValidationResult>();

        }
    }
}
