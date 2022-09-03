using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class CreditApprovedHigh : ICreditApproved
    {
        double GetCreditApproved(CreditApplication creditApplication)
        {
            return creditApplication.Salary * 0.3;
        }
    }
}
