//bsef19m001
//Atif Iqbal Butt
//EAD Assignment # 01

//Admin login: admin
//Admin pincode: 123

/************************** LoginBLL file (Presentation Layer) **************************/


using ATM_BO;
using ATM_DAL;
namespace ATM_BLL
{
    public static class LoginBLL
    {
        //check if userlogin and pincode are correct or not
        public static LoginBO CheckLogin(LoginBO userLogin)
        {
            int noOfAttemps = 2;
            string user = userLogin.username;
            (userLogin.username, userLogin.pincode) = LoginEncryptDecrypt(user, userLogin.pincode);
            userLogin = LoginDAL.CheckLoginDB(userLogin);

            while (noOfAttemps >= 1 && userLogin.userType == 'n')
            {
                Console.WriteLine("Invalid input, No of attempts remaining " + noOfAttemps);
                Console.Write("Enter Pin code again: ");
                userLogin.pincode = Console.ReadLine();
                (userLogin.username, userLogin.pincode) = LoginEncryptDecrypt(user, userLogin.pincode);
                userLogin = LoginDAL.CheckLoginDB(userLogin);
                noOfAttemps--;
            }

            //if noOfAttemps has crossed 3 and if the type of user if neither admin
            //nor customer then disable the login
            if (noOfAttemps == 0 && userLogin.userType == 'n')
            {
                LoginDAL.DisableLoginDB(userLogin);
            }
            return userLogin;
        }


        //check if acc exist in db against that login or not
        public static bool IsLoginExist(LoginBO user)
        {
            string username = user.username;
            string pincode = user.pincode;
            (username, pincode) = LoginEncryptDecrypt(username, pincode);
            user.username = username;
            user.pincode = pincode;
            bool check = LoginDAL.IsLoginExistDB(user);
            if (check)
                return true;
            return
                false;
        }

        //Perform simple encryption and decryption on login (username) and pincode
        public static (string, string) LoginEncryptDecrypt(string username, string pincode)
        {
            string user = username.ToUpper();
            string pin = pincode.ToUpper();
            string cipherLogin = "", cipherPinCode = "";

            //Seperating the letters and digit part from username
            foreach (char c in user)
            {
                if (char.IsLetter(c))
                {
                    char enValAlpha = Convert.ToChar(Convert.ToChar(90) - c + Convert.ToChar(65));
                    cipherLogin += enValAlpha;
                }
                else if (char.IsDigit(c))
                {
                    char enValNum = Convert.ToChar(Convert.ToChar(57) - c + Convert.ToChar(48));
                    cipherLogin += enValNum;
                }
            }

            //Seperating the letters and digit part from pincode
            foreach (char c in pin)
            {
                if (char.IsLetter(c))
                {
                    char enValAlpha = Convert.ToChar(Convert.ToChar(90) - c + Convert.ToChar(65));
                    cipherPinCode += enValAlpha;
                }
                if (char.IsDigit(c))
                {
                    char enValNum = Convert.ToChar(Convert.ToChar(57) - c + Convert.ToChar(48));
                    cipherPinCode += enValNum;
                }
            }
            user = cipherLogin;
            pin = cipherPinCode;
            return (user, pin);
        }

    }
}