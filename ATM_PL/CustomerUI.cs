//bsef19m001
//Atif Iqbal Butt
//EAD Assignment # 01

//Admin login: admin
//Admin pincode: 123

/************************** CustomerUI file (Presentation Layer) **************************/


using ATM_BLL;
using ATM_BO;
namespace ATM_PL
{
    public static class CustomerUI
    {

        //It displays all the options in customer menu
        //userLogin obj contains info about login and pincode
        public static void CustomerMenu(LoginBO userLogin)
        {
            Console.WriteLine();
            Console.WriteLine("----------- Welcome to the Customer menu -----------");
            Console.WriteLine();
            Console.WriteLine();

            //This BO contains all info about a customer
            CustomerBO c = new CustomerBO();
            do
            {
                c.loginID = userLogin.id;

                //if customer account is disabled then he/she will be presented
                //with different menu than normal user
                //this function checks if customer acc is active or not
                bool IsActive = CustomerBLL.CheckAccountStatusBLL(c.loginID);

                int choice = 0;
                if (!IsActive)
                {

                    //If acc is disabled, then customer can only view his/her balance
                    Console.WriteLine("Your account is disabled! Please Contact with your Bank");
                    Console.WriteLine();
                    Console.WriteLine("You can only view your balance");
                    do
                    {
                        Console.WriteLine("Enter 4 to view Balance");
                        Console.WriteLine("Enter 5 to exit");
                        Console.WriteLine();
                        try
                        {
                            Console.Write("Enter any of the above numbers: ");
                            choice = int.Parse(Console.ReadLine());
                        }
                        catch (Exception ex)
                        { Console.WriteLine(ex.Message); }
                        if (choice == 4)
                        {
                            DisplayBalance();
                        }
                        else if (choice == 5)
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine();
                            Exit();
                            Console.WriteLine("Thanks for using our ATM service ;)");
                            Console.WriteLine("Have a good day ahead!");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input!");
                            Console.WriteLine();
                        }
                    } while (true);
                    break;
                }
                else
                {
                    //Following menu is for Active Users
                    Console.WriteLine();
                    Console.WriteLine("1----Withdraw Cash");
                    Console.WriteLine("2----Cash Transfer");
                    Console.WriteLine("3----Deposit Cash");
                    Console.WriteLine("4----Display Balance");
                    Console.WriteLine("5----Exit");
                    Console.WriteLine();
                    Console.WriteLine("Please select one of the above options");
                    int option = 0;
                    try
                    {
                        option = Convert.ToInt32(Console.ReadLine());

                        if (option == 1)
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("===== Withdraw Menu =====");
                            Console.WriteLine();
                            Console.WriteLine();
                            WithDrawCash();
                        }
                        else if (option == 2)
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("===== Cash Transfer Menu =====");
                            Console.WriteLine();
                            Console.WriteLine();
                            CashTransfer();
                        }
                        else if (option == 3)
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("===== Deposit Cash Menu =====");
                            Console.WriteLine();
                            Console.WriteLine();
                            DepositCash();
                        }
                        else if (option == 4)
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("===== Display Balance Menu =====");
                            Console.WriteLine();
                            Console.WriteLine();
                            DisplayBalance();
                        }
                        else if (option == 5)
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine();
                            Exit();
                            Console.WriteLine("Thanks for using our ATM service ;)");
                            Console.WriteLine("Have a good day ahead!");
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Invalid input!");
                            Console.WriteLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine(ex.Message);
                        Console.WriteLine();
                    }
                }
            } while (true);


            void WithDrawCash()
            {
                Console.WriteLine();
                Console.WriteLine("a) Fash Cash");
                Console.WriteLine("b) Normal Cash");
                Console.WriteLine();
                Console.Write("Please select mode of withdrawal: ");
                char option = ' ';
                try
                {
                    option = Convert.ToChar(Console.ReadLine());
                }
                catch (Exception ex)
                { Console.WriteLine(ex.Message); }
                if (option == 'a' || option == 'A')
                {
                    double money = 0;
                    Console.WriteLine();
                    Console.WriteLine("1----500");
                    Console.WriteLine("2----1000");
                    Console.WriteLine("3----2000");
                    Console.WriteLine("4----5000");
                    Console.WriteLine("5----10000");
                    Console.WriteLine("6----15000");
                    Console.WriteLine("7----20000");
                    Console.WriteLine();
                    Console.Write("Select one of the dominations of money: ");
                    int denom = 0; //denominations of money
                    try
                    {
                        denom = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    if (denom == 1)
                        money = 500;
                    else if (denom == 2)
                        money = 1000;
                    else if (denom == 3)
                        money = 2000;
                    else if (denom == 4)
                        money = 5000;
                    else if (denom == 5)
                        money = 10000;
                    else if (denom == 6)
                        money = 15000;
                    else if (denom == 7)
                        money = 20000;
                    else
                        Console.WriteLine("Invalid input!");
                    if (money != 0)
                    {
                        Console.WriteLine("Are you sure to want to withdraw Rs." + money + " (Y/N)? ");
                        char choice = Convert.ToChar(Console.ReadLine());
                        if (choice == 'Y' || choice == 'y')
                        {
                            //Before withdraw, first check if user surpasses the
                            //daily withdrawal limit of 20,000 or not 
                            if (CheckWithdrawLimit(userLogin, money))
                            {
                                bool check = CustomerBLL.WithdrawMoneyBLL(userLogin, money);
                                if (check)
                                {
                                    Console.WriteLine("Cash withdrawn successfully!");
                                    Console.Write("Do you wish to print a receipt (Y/N) ");
                                    char receiptOpt = Convert.ToChar(Console.ReadLine());
                                    if (receiptOpt == 'Y' || receiptOpt == 'y')
                                    {
                                        PrintWithdrawReceipt(money);
                                    }
                                    else if (receiptOpt == 'N' || receiptOpt == 'n')
                                    {
                                        Console.WriteLine("Cancelling operation...");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Input!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Insufficient balance!");
                                    Console.WriteLine("Transaction failed!");
                                }
                            }

                        }
                        else if (choice == 'N' || choice == 'n')
                        {
                            Console.WriteLine("Cancelling operation...");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input!");
                        }
                    }
                }
                else if (option == 'b' || option == 'B')
                {
                    double money = 0;
                    Console.Write("Enter the Withdrawal amount: ");
                    try
                    {
                        money = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine();
                        Console.WriteLine("Are you sure to want to withdraw Rs." + money + " (Y/N)? ");
                        char choice = Convert.ToChar(Console.ReadLine());
                        if (choice == 'Y' || choice == 'y')
                        {
                            //Before withdraw, first check if user surpasses the
                            //daily withdrawal limit of 20,000 or not
                            if (CheckWithdrawLimit(userLogin, money))
                            {
                                bool check = CustomerBLL.WithdrawMoneyBLL(userLogin, money);
                                if (check)
                                {
                                    Console.WriteLine("Cash withdrawn successfully!");
                                    Console.Write("Do you wish to print a receipt (Y/N) ");
                                    char receiptOpt = Convert.ToChar(Console.ReadLine());
                                    if (receiptOpt == 'Y' || receiptOpt == 'y')
                                    {
                                        PrintWithdrawReceipt(money);
                                    }
                                    else if (receiptOpt == 'N' || receiptOpt == 'n')
                                    {
                                        Console.WriteLine("Cancelling operation...");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Input!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Insufficient balance!");
                                    //Console.WriteLine("Invalid Amount!");
                                    Console.WriteLine("Transaction failed!");
                                }
                            }
                        }

                        else if (choice == 'N' || choice == 'n')
                        {
                            Console.WriteLine("Cancelling operation...");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input!");
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine();
                        Console.WriteLine(ex.Message);
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }
            }

            
            void CashTransfer()
            {
                double amount = 0;
                Console.Write("Enter amount in multiples of 500: ");
                try
                {
                    amount = Convert.ToInt32(Console.ReadLine());
                }
                catch(Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                }

                //Check if the customer is entering money multiples of 500 or not
                bool check = CustomerBLL.IsTransferMoneyValid(userLogin, amount);
                if (!check)
                    Console.WriteLine("Invalid Amount!");
                else
                {
                    Console.Write("Enter the account number to which you want to transfer: ");
                    c.accNo = Convert.ToInt32(Console.ReadLine());

                    //Get the info of acc from the DB about the recipient acc holder
                    c = CustomerBLL.GetAccConfirmationBLL(c);
                    c.balance = amount;
                    if (c.name != null)
                    {
                        Console.Write("You wish to deposit Rs. " + amount + " in the account " +
                            "held by " + c.name + " ; If this information is correct please re-enter" +
                            " the account number: ");
                        int onRepeatAccNo = 0;
                        try
                        {
                            onRepeatAccNo = Convert.ToInt32(Console.ReadLine());
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine();
                            Console.WriteLine(ex.Message);
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                        if (c.accNo != onRepeatAccNo)
                        {
                            Console.WriteLine("Invalid Entry!");
                        }
                        else
                        {

                            //This checks if recipient acc is disabled
                            //customer will be unable to transfer cash to 
                            //disabled account
                            if(CustomerBLL.IsAccountActiveBLL(c.accNo))
                            {
                                CustomerBLL.CashTransferBLL(c, userLogin.id);
                                Console.WriteLine();
                                Console.WriteLine("Cash Transferred Successfully!");
                                Console.WriteLine("Transaction Confirmed!");
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Recipient Account is disabled, cannot transfer money!");
                                Console.WriteLine("Transaction Failed!");
                                Console.WriteLine();
                            }
                            
                        }
                    }
                    else
                    {
                        Console.WriteLine("Account holder does not exist!");
                    }
                }
            }
            void DepositCash()
            {
                Console.Write("Enter the amount to deposit: ");
                try
                {
                    int amount = Convert.ToInt32(Console.ReadLine());
                    c.balance = amount;
                    Console.WriteLine();

                    //if money is negative then it return false
                    if (CustomerBLL.DepositCashBLL(c))
                    {
                        Console.WriteLine("Cash Deposited Successfully!");
                        Console.Write("Do you wish to print a receipt (Y/N) ");
                        char receiptOpt = Convert.ToChar(Console.ReadLine());
                        if (receiptOpt == 'Y' || receiptOpt == 'y')
                        {
                            PrintDepositReceipt(amount);
                        }
                        else if (receiptOpt == 'N' || receiptOpt == 'n')
                        {
                            Console.WriteLine("Cancelling operation...");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Amount!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                }
            }
            void DisplayBalance()
            {
                c = CustomerBLL.GetReceipt(userLogin);
                Console.WriteLine();
                Console.WriteLine("Account #" + c.accNo);
                Console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy"));
                Console.WriteLine("Balance: " + c.balance);
                Console.WriteLine();
            }
            void Exit()
            {
                Console.WriteLine();
                Console.WriteLine("Exiting...");
                Console.WriteLine();
            }
            void PrintWithdrawReceipt(double money)
            {
                c = CustomerBLL.GetReceipt(userLogin);
                Console.WriteLine();
                Console.WriteLine("Account #" + c.accNo);
                Console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy"));
                Console.WriteLine("Withdrawn: " + money);
                Console.WriteLine("Balance: " + c.balance);
                Console.WriteLine();
            }
            void PrintDepositReceipt(double money)
            {
                c = CustomerBLL.GetReceipt(userLogin);
                Console.WriteLine();
                Console.WriteLine("Account #" + c.accNo);
                Console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy"));
                Console.WriteLine("Amount transferred: " + money);
                Console.WriteLine("Balance: " + c.balance);
                Console.WriteLine();
            }

            //this checks status on passing loginID
            bool CheckAccountStatus(int loginID)
            {
                bool check = CustomerBLL.CheckAccountStatusBLL(loginID);
                return check;
            }

            //this checks the daily withdrawal limit
            bool CheckWithdrawLimit(LoginBO userLogin, double money)
            {
                bool CanWithdraw; double remAmount;
                TransacRecordBO transObj = new TransacRecordBO();
                transObj.userID = userLogin.id;
                transObj.amount = money;
                transObj.date = DateTime.Now.Date;
                (CanWithdraw, remAmount) = CustomerBLL.CheckWithdrawLimitBAL(transObj);
                if (!CanWithdraw)
                {
                    Console.WriteLine();
                    Console.WriteLine("Transaction Failed!");
                    Console.WriteLine("You can not cross daily withdrawal limit of 20000");
                    Console.WriteLine("You can only withdraw Rs. " + remAmount + " or less");
                    Console.WriteLine();
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }
    }
}
