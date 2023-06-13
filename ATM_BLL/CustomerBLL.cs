//bsef19m001
//Atif Iqbal Butt
//EAD Assignment # 01

//Admin login: admin
//Admin pincode: 123

/************************** CustomerBLL file (Business Login Layer) **************************/


using ATM_DAL;
using ATM_BO;
namespace ATM_BLL
{
    public static class CustomerBLL
    {
        public static bool WithdrawMoneyBLL(LoginBO login, double amount)
        {
            //check if user has enough money to withdraw
            CustomerBO c = CustomerDAL.ValidateMoneyDB(login);
            if (amount > 0 && amount <= c.balance)
            {
                c.balance -= amount;
                CustomerDAL.WithdrawDB(c, amount);
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool IsTransferMoneyValid(LoginBO login, double balance)
        {
            CustomerBO c = CustomerDAL.ValidateMoneyDB(login);
            if ((balance % 500 == 0 && balance > 0) && balance <= c.balance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static CustomerBO GetReceipt(LoginBO login)
        {
            CustomerBO c = CustomerDAL.GetReceiptDB(login);
            return c;
        }
        public static void CashTransferBLL(CustomerBO c, int id)
        {
            CustomerDAL.CashTransferDB(c, id);
        }

        //returns the info about recipient acc on cash transfer
        public static CustomerBO GetAccConfirmationBLL(CustomerBO c)
        {
            c = CustomerDAL.GetAccConfirmationDB(c);
            return c;
        }

        public static bool DepositCashBLL(CustomerBO c)
        {
            if (c.balance > 0)
            {
                CustomerDAL.DepositCashDB(c);
                return true;
            }
            else
                return false;
        }

        public static bool CheckAccountStatusBLL(int loginID)
        {
            string status = CustomerDAL.CheckAccountStatusDAL(loginID);
            if (status == "active")
                return true;
            else
                return false;
        }

        public static bool IsAccountActiveBLL(int accNo)
        {
            string status = CustomerDAL.IsAccountActiveDAL(accNo);
            if (status == "active")
                return true;
            else
                return false;
        }

        //returns true if user has not surpassed the daily withdrawal limit
        //also returns the amount which he/she can actually withdraw within the 
        //restriction of 20,000 per day limit
        public static (bool, double) CheckWithdrawLimitBAL(TransacRecordBO transObj)
        {
            double totalAmount;
            totalAmount = CustomerDAL.CheckWithdrawLimitDAL(transObj);

            //it checks the transaction record of a customer in a particular date
            //if record do not exist, then it means that he/she is going to withdraw
            //his/her first cash of the day
            if (transObj.amount > 20000)
            {
                totalAmount = 20000;
                return (false, totalAmount);
            }
            else
            {
                if ((totalAmount + transObj.amount) > 20000)
                {
                    return (false, (20000 - totalAmount));
                }
                else
                {
                    return (true, totalAmount);
                }
            }
        }
    }
}
