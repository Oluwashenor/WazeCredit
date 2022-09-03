using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class CreditApprovedLow : ICreditApproved
    {
        double GetCreditApproved(CreditApplication creditApplication)
        {
            return creditApplication.Salary * 0.5;
        }
    }
}
