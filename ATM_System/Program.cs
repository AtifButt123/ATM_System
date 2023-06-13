//bsef19m001
//Atif Iqbal Butt
//EAD Assignment # 01

//Admin login: admin
//Admin pincode: 123

/************************** Main Driver file **************************/


using ATM_PL;
namespace ATM_system
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //This function is defined in Presentation Layer Project (ATM_PL)
            //within file named LoginUI.cs
            
            //login two types of users (Customer, Admin)
            LoginUI.Login();
        }
    }
}