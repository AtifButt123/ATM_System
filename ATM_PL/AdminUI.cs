//bsef19m001
//Atif Iqbal Butt
//EAD Assignment # 01

//Admin login: admin
//Admin pincode: 123

/************************** AdminUI file (Presentation Layer) **************************/


using ATM_BLL;
using ATM_BO;
namespace ATM_PL
{
    public static class AdminUI
    {

        //presenting admin menu that contains all the necessary options
        public static void AdminMenu(LoginBO userLogin)
        {
            //this BO contains all necessary info for tranfering data b/w
            //adminBLL and adminDAL file
            AdminMenuBO adm = new AdminMenuBO();
            CustomerBO c = new CustomerBO();
            Console.WriteLine();
            Console.WriteLine("----------- Welcome to the Admin menu -----------");
            Console.WriteLine();
            Console.WriteLine();
            do
            {
                c.loginID = userLogin.id;
                Console.WriteLine();
                Console.WriteLine("1----Create New Account");
                Console.WriteLine("2----Delete Existing Account");
                Console.WriteLine("3----Update Account Information");
                Console.WriteLine("4----Search For Account");
                Console.WriteLine("5----View Reports");
                Console.WriteLine("6----Exit");
                Console.WriteLine();
                Console.WriteLine("Please select one of the above options");
                int option;
                try
                {
                    option = Convert.ToInt32(Console.ReadLine());

                    if (option == 1)
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("===== Create Account Menu =====");
                        Console.WriteLine();
                        Console.WriteLine();
                        CreateNewAcc();
                    }
                    else if (option == 2)
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("===== Delete Account Menu =====");
                        Console.WriteLine();
                        Console.WriteLine();
                        DeleteExistAcc();
                    }
                    else if (option == 3)
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("===== Update Account Info Menu =====");
                        Console.WriteLine();
                        Console.WriteLine();
                        UpdateAccInfo();
                    }
                    else if (option == 4)
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("===== Search For Account Menu =====");
                        Console.WriteLine();
                        Console.WriteLine();
                        SearchForAcc();
                    }
                    else if (option == 5)
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("===== View Reports Menu =====");
                        Console.WriteLine();
                        Console.WriteLine();
                        ViewReports();
                    }
                    else if (option == 6)
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
            } while (true);

            //Creating new account
            void CreateNewAcc()
            {
                Console.Write("Login: ");
                adm.username = Console.ReadLine();
                bool IsLetter = true;
                try
                {
                    Console.Write("Pin Code: ");
                    adm.pinCode = Console.ReadLine();
                    Console.WriteLine();
                    Console.WriteLine("Tip: Name should contain atleast three letters without spaces!");
                    Console.WriteLine("Tip: Spaces are allowed but not within first three letters!");
                    Console.WriteLine();
                    Console.Write("Holder name: ");
                    string holderName = Console.ReadLine();
                    while (true)
                    {
                        if(holderName[0] == ' ' || holderName[1] == ' ' || holderName[2]==' ')
                        {
                            IsLetter = false;
                        }                        
                        foreach (char c in holderName)
                        {
                            if (!char.IsLetter(c) && c!=' ')
                            {
                                IsLetter = false;
                                break;
                            }
                        }
                        if (IsLetter)
                        {
                            adm.holderName = holderName;
                            break;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Invalid name!");
                            Console.WriteLine("Name can only contain letters!");
                            Console.WriteLine();
                            Console.Write("Enter again: ");
                            holderName = Console.ReadLine();
                            IsLetter = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Unable to create new account");
                    Console.WriteLine();
                }
                Console.Write("Type (Savings, Current): ");
                string accType = Console.ReadLine();

                //keep on asking for correct account type
                while (true)
                {
                    if (accType.ToLower() == "savings" || accType.ToLower() == "current")
                    {
                        adm.accType = accType;
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Account Type is Invalid!");
                        Console.WriteLine();
                        Console.Write("Enter again: ");
                        accType = Console.ReadLine();
                    }
                }

                if (adm.accType.ToLower() != "savings" && adm.accType.ToLower() != "current")
                {
                    Console.WriteLine("Invalid Input, Acc type must be savings or current only!");
                }
                else
                {
                    Console.Write("Starting Balance: ");
                    try
                    {
                        adm.balance = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Status: ");
                        string status = Console.ReadLine();
                        while (true)
                        {

                            //keep on asking for correct status
                            if (status.ToLower() == "active" || status.ToLower() == "disabled")
                            {
                                adm.status = status;
                                break;
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Status is Invalid!");
                                Console.WriteLine();
                                Console.Write("Enter again: ");
                                status = Console.ReadLine();
                            }
                        }

                        try
                        {
                            if (string.IsNullOrEmpty(adm.username) || string.IsNullOrEmpty(adm.pinCode) || string.IsNullOrEmpty(adm.holderName))
                            {
                                Console.WriteLine();
                                Console.WriteLine("Unable to create new cccount");
                                Console.WriteLine("Information is incomplete!");
                                Console.WriteLine();
                            }
                            else
                            {
                                int accNo = AdminBLL.CreateAccBLL(adm);
                                Console.WriteLine();
                                Console.WriteLine("Account Successfully Created - the account " +
                                                  "number assigned is: " + accNo);
                            }
                        }
                        catch
                        {
                            Console.WriteLine();
                            Console.WriteLine("Unable to create new account!");
                            Console.WriteLine("Account with this login already Exist!");
                            Console.WriteLine();
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Unable to create new account");
                        Console.WriteLine();
                    }
                }
            }
            void DeleteExistAcc()
            {
                try
                {
                    Console.Write("Enter the Account Number to which you want to Delete: ");
                    int accNo = Convert.ToInt32(Console.ReadLine());

                    //check if acc exist or not
                    bool check = AdminBLL.DeleteExistAccBLL(accNo);
                    if (check)
                        Console.WriteLine("Account Deleted Successfully!");
                    else
                        Console.WriteLine("Account does not exist!");
                }
                catch
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Account Number!");
                    Console.WriteLine();
                }
            }

            void UpdateAccInfo()
            {
                bool check = false;
                int accNo = 0;
                AdminMenuBO updatedInfo = new AdminMenuBO();
                AdminMenuBO oldInfo = new AdminMenuBO();
                try
                {
                    Console.Write("Enter the Account Number to which you want to Update: ");
                    accNo = Convert.ToInt32(Console.ReadLine());
                    oldInfo.accountNo = accNo;
                    (oldInfo, check) = GetAccInfo(accNo);
                }
                catch
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Account Number!");
                    Console.WriteLine();
                }
                if (check)
                {
                    Console.WriteLine("Please enter in the fields you wish to update (leave blank otherwise): ");
                    Console.WriteLine();
                    try
                    {
                        bool IsLetter = true;
                        Console.Write("Login: ");
                        string username = Console.ReadLine();
                        string user, pin;
                        if (string.IsNullOrEmpty(username))
                        {
                            user = "";
                        }
                        else
                            user = username;
                        Console.Write("Pin Code: ");
                        string pinCode = Console.ReadLine();
                        if (string.IsNullOrEmpty(pinCode))
                        {
                            pin = "";
                        }
                        else
                            pin = pinCode;
                        Console.WriteLine();
                        Console.WriteLine("Tip: Name should contain atleast three letters without spaces!");
                        Console.WriteLine("Tip: Spaces are allowed but not within first three letters!");
                        Console.WriteLine();
                        Console.Write("Holder name: ");
                        string holderName = Console.ReadLine();
                        if (string.IsNullOrEmpty(holderName))
                            updatedInfo.holderName = oldInfo.holderName;
                        else
                        {
                            while (true)
                            {
                                if (holderName[0] == ' ' || holderName[1] == ' ' || holderName[2] == ' ')
                                {
                                    IsLetter = false;
                                }
                                foreach (char c in holderName)
                                {
                                    if (!char.IsLetter(c) && c != ' ')
                                    {
                                        IsLetter = false;
                                        break;
                                    }
                                }
                                if (IsLetter)
                                {
                                    updatedInfo.holderName = holderName;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Invalid name!");
                                    Console.WriteLine("Name can only contain letters!");
                                    Console.WriteLine();
                                    Console.Write("Enter again: ");
                                    holderName = Console.ReadLine();
                                    IsLetter = true;
                                }
                            }
                        }
                        Console.Write("Status (active or disabled): ");
                        string status = Console.ReadLine();
                        if (string.IsNullOrEmpty(status))
                            updatedInfo.status = oldInfo.status;
                        else
                        {
                            while (true)
                            {
                                if (status.ToLower() == "active" || status.ToLower() == "disabled")
                                {
                                    updatedInfo.status = status;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Status is Invalid, must be active or disabled!");
                                    Console.WriteLine();
                                    Console.Write("Enter again: ");
                                    status = Console.ReadLine();
                                }
                            }
                        }
                        updatedInfo.balance = oldInfo.balance;
                        updatedInfo.accType = oldInfo.accType;
                        updatedInfo.accountNo = accNo;

                        //perform encrypt/decrypt on updating
                        (user, pin) = LoginBLL.LoginEncryptDecrypt(user, pin);
                        if (user == "")
                        {
                            updatedInfo.username = oldInfo.username;
                        }
                        else
                        {
                            updatedInfo.username = user;
                        }
                        if (pin == "")
                        {
                            updatedInfo.pinCode = oldInfo.pinCode;
                        }
                        else
                        {
                            updatedInfo.pinCode = pin;
                        }
                        try
                        {
                            AdminBLL.UpdateAccInfoBLL(updatedInfo);
                            Console.WriteLine("Account has been successfully updated!");
                        }
                        catch
                        {
                            Console.WriteLine();
                            Console.WriteLine("Unable to update the account!");
                            Console.WriteLine("Account with this login already Exist!");
                            Console.WriteLine();
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                }
            }

            //Search by ANDing the input fields
            void SearchForAcc()
            {
                CustomerBO c = new CustomerBO();
                Console.WriteLine();
                Console.WriteLine("===== Search Menu =====");
                Console.WriteLine();
                Console.WriteLine();
                try
                {
                    //here types of all variables are strings
                    //because it is easy to get Enter key (on ignoring input) in string type
                    //variable
                    //however the end result will remain true

                    Console.WriteLine("Tip: Press Enter to ignore the values");
                    Console.WriteLine("The ignored values will not be included in search result");
                    Console.WriteLine();
                    Console.WriteLine();

                    string accNo, id, name, accType, status, balance;
                    Console.Write("Account ID: ");
                    accNo = Console.ReadLine();
                    //if user ignores (after pressing enter key) stores null
                    if (string.IsNullOrEmpty(accNo))
                    {
                        accNo = null;
                    }

                    Console.Write("User ID: ");
                    id = Console.ReadLine();
                    if (string.IsNullOrEmpty(id))
                    {
                        id = null;
                    }
                    Console.Write("Holder Name: ");
                    name = Console.ReadLine();
                    if (string.IsNullOrEmpty(name))
                        name = null;
                    Console.Write("Type (Savings, Current): ");
                    accType = Console.ReadLine();
                    if (!string.IsNullOrEmpty(accType))
                    {
                        while (true)
                        {
                            if (accType.ToLower() == "savings" || accType.ToLower() == "current" || string.IsNullOrEmpty(accType))
                            {
                                if (string.IsNullOrEmpty(accType))
                                    accType = null;
                                break;
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Account Type is Invalid!");
                                Console.WriteLine();
                                Console.Write("Enter again: ");
                                accType = Console.ReadLine();
                            }
                        }
                    }
                    else
                    {
                        accType = null;
                    }
                    Console.Write("Balance: ");
                    balance = Console.ReadLine();
                    if (string.IsNullOrEmpty(balance))
                    {
                        balance = null;
                    }
                    Console.Write("Status (active, disabled): ");
                    status = Console.ReadLine();
                    if (!string.IsNullOrEmpty(status))
                    {
                        while (true)
                        {
                            if (status.ToLower() == "active" || status.ToLower() == "disabled" || string.IsNullOrEmpty(status))
                            {
                                if (string.IsNullOrEmpty(status))
                                    status = null;
                                break;
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Status is Invalid!");
                                Console.WriteLine();
                                Console.Write("Enter again: ");
                                status = Console.ReadLine();
                            }
                        }
                    }
                    else
                    {
                        status = null;
                    }

                    //Creating a sort of dynamic query
                    string query = @"Select accNo, id, name, balance, accType, status from Customer where 1=1";

                    if (accNo != null)
                    {
                        query += " and accNo = " + accNo;
                    }
                    if(id!= null)
                    {
                        query += " and id = " + id;
                    }
                    if (name!=null)
                    {
                        query += " and name = '" + name + "'";
                    }
                    if (accType != null)
                    {
                        query += " and accType = '" + accType + "'";
                    }
                    if (balance != null)
                    {
                        query += " and balance = " + balance;
                    }
                    if (status != null)
                    {
                        query += " and status = '" + status + "'";
                    }

                    List<CustomerBO> custList = new List<CustomerBO>();
                    custList = AdminBLL.SearchForAccBLL(query);
                    PrintSearchResult(custList);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            void PrintSearchResult(List<CustomerBO> custList)
            {
                Console.WriteLine();
                Console.WriteLine("===== Search Result =====");
                Console.WriteLine();

                Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
                Console.WriteLine(" Account ID | User ID    | Holder Name               | Balance               | Account Type  | Status      ");
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
                foreach (CustomerBO c in custList)
                {
                    Console.WriteLine(String.Format(" {0,-10} | {1,-10} | {2,-25} | {3,-21} | {4,-13} | {5,-17} ", c.accNo, c.id, c.name, c.balance, c.accType, c.status));
                }
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
                Console.WriteLine();
            }

            void ViewReports()
            {
                Console.WriteLine();
                Console.WriteLine("1----Accounts By Amount");
                Console.WriteLine("2----Accounts By Date");
                Console.WriteLine();
                Console.Write("Select one of the above two options: ");
                int choice = 0;
                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch(Exception ex)
                { Console.WriteLine(ex.Message); }
                if (choice == 1)
                {
                    double minAmnt = 0, maxAmnt = 0;
                    try
                    {
                        Console.Write("Enter the minimum amount: ");
                        minAmnt = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Enter the maximum amount: ");
                        maxAmnt = Convert.ToDouble(Console.ReadLine());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    List<CustomerBO> customerList = new List<CustomerBO>();
                    customerList = AdminBLL.GetAccByAmountBLL(minAmnt, maxAmnt);
                    Console.WriteLine();
                    Console.WriteLine("===== Search Result =====");
                    Console.WriteLine();

                    Console.WriteLine("--------------------------------------------------------------------------------------------------");
                    Console.WriteLine(" Account ID | User ID    | Holder Name               | Type        | Balance         | Status      ");
                    Console.WriteLine("--------------------------------------------------------------------------------------------------");
                    foreach (CustomerBO cust in customerList)
                    {
                        Console.WriteLine(String.Format(" {0,-10} | {1,-10} | {2,-25} | {3,-11} | {4,-15} | {5,-10} ", cust.accNo, cust.id, cust.name, cust.accType, cust.balance, cust.status));
                    }
                    Console.WriteLine("--------------------------------------------------------------------------------------------------");
                    Console.WriteLine();
                }
                else if (choice == 2)
                {
                    try
                    {
                        Console.Write("Enter Account Number: ");
                        int accNo = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter the starting date (dd/mm/yyyy): ");
                        DateTime startDate = Convert.ToDateTime(Console.ReadLine());
                        Console.Write("Enter the ending date (dd/mm/yyyy): ");
                        DateTime endingDate = Convert.ToDateTime(Console.ReadLine());

                        List<TransacRecordBO> transactionList = new List<TransacRecordBO>();
                        transactionList = AdminBLL.SearchByDateBLL(startDate, endingDate, accNo);
                        Console.WriteLine();
                        Console.WriteLine("===== Search Result =====");
                        Console.WriteLine();

                        Console.WriteLine("---------------------------------------------------------------------------------------------");
                        Console.WriteLine(" User ID    | Holder Name               | Amount          | Transaction Type     | Date      ");
                        Console.WriteLine("---------------------------------------------------------------------------------------------");
                        foreach (TransacRecordBO trans in transactionList)
                        {
                            Console.WriteLine(String.Format(" {0,-10} | {1,-25} | {2,-15} | {3,-20} | {4,-17} ", trans.userID, trans.name, trans.amount, trans.transacType, trans.date.ToString("dd/MM/yyyy")));
                        }
                        Console.WriteLine("---------------------------------------------------------------------------------------------");
                        Console.WriteLine();
                    }
                    catch
                    {
                        Console.WriteLine();
                        Console.WriteLine("Invalid Date Format!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input!");
                }
            }
            void Exit()
            {
                Console.WriteLine();
                Console.WriteLine("Exiting...");
                Console.WriteLine();
            }

            (AdminMenuBO,bool) GetAccInfo(int accNo)
            {
                AdminMenuBO adm = new AdminMenuBO();
                bool check;
                (adm, check) = AdminBLL.GetAccInfoBLL(accNo);
                if (!check)
                {
                    Console.WriteLine();
                    Console.WriteLine("Account does not exist!");
                    Console.WriteLine();
                    return (adm, false);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Account # " + accNo);
                    Console.WriteLine("Holder Name: " + adm.holderName);
                    Console.WriteLine("Account Type: " + adm.accType);
                    Console.WriteLine("Balance: " + adm.balance);
                    Console.WriteLine("Status: " + adm.status);
                    Console.WriteLine();
                    return (adm, true);
                }
            }


        }
    }
}
