using System;
using WazeCredit.Models;

namespace WazeCredit.Service
{

    public class CreditValidationChecker : IValidationChecker
    {
        public string ErrorMessage => "You did not meet Age/Salary Requirement";

       public bool ValidatorLogic(CreditApplication model)
        {

            if(model.Salary < 10000)
            {
                return false;
            }
            if(DateTime.Now.AddYears(-18) < model.DOB)
            {
                return false;
            }
            return true;
        }
    }
}
