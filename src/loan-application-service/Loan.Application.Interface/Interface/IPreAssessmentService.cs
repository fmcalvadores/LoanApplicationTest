using Loan.Application.Commons;
using Loan.Application.DTO;
using System.Threading.Tasks;

namespace Loan.Application.Interfaces
{
    public interface IPreAssessmentService
    {
        Task<Result> AssessApplicant(ApplicantDTO dto);
    }
}
