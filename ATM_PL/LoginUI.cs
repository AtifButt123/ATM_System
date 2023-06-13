//bsef19m001
//Atif Iqbal Butt
//EAD Assignment # 01

//Admin login: admin
//Admin pincode: 123

/************************** LoginUI file (Presentation Layer) **************************/


using ATM_BO;
using ATM_BLL;
namespace ATM_PL
{
    public class LoginUI 
    {

        public static void Login()
        {
            Console.WriteLine("----------- Welcome to our ATM service -----------");
            Console.WriteLine();
            Console.WriteLine();
            LoginBO userLogin = new LoginBO();
            Console.Write("Enter login: ");
            string username = Console.ReadLine();
            userLogin.username = username;
            userLogin.pincode = "";

            //check first if login exist in db or not
            //if login exist then ask for pin code
            if (!LoginBLL.IsLoginExist(userLogin))
            {
                Console.WriteLine("Incorrect login!");
            }
            else
            {
                Console.Write("Enter Pin code: ");
                userLogin.pincode = Console.ReadLine();
                userLogin.username = username;

                //now check if the given pincode is correct against that login or not
                userLogin = LoginBLL.CheckLogin(userLogin);

                //here userType means two types of user admin and customer
                // 'c' means customer
                // 'a' means admin

                if (userLogin.userType == 'c')
                {
                    Console.Clear();
                    CustomerUI.CustomerMenu(userLogin);
                }
                else if (userLogin.userType == 'a')
                {
                    Console.Clear();
                    AdminUI.AdminMenu(userLogin);
                }
                else
                {

                    //if user fails 3 attemps (including first attempt) then his/her account
                    //will be disabled
                    Console.WriteLine();
                    Console.WriteLine("You failed 3 attempts!");
                    Console.WriteLine("Your account has been disabled!");
                    Console.WriteLine("Please contact the bank!");
                    Console.WriteLine();
                }
            }
        }
        
    }
        
}