//bsef19m001
//Atif Iqbal Butt
//EAD Assignment # 01

//Admin login: admin
//Admin pincode: 123

/************************** AdminBLL file (Presentation Layer) **************************/


using ATM_DAL;
using ATM_BO;
namespace ATM_BLL
{
    public static class AdminBLL
    {
        public static int CreateAccBLL(AdminMenuBO adminBO)
        {
            (adminBO.username, adminBO.pinCode) = LoginBLL.LoginEncryptDecrypt(adminBO.username, adminBO.pinCode);
            int accNo = AdminDAL.CreateAccDAL(adminBO);
            accNo++;
            return accNo;
        }

        public static bool DeleteExistAccBLL(int accNo)
        {
            bool check = AdminDAL.DeleteExistAccDAL(accNo);
            return check;
        }
        public static void UpdateAccInfoBLL(AdminMenuBO adm)
        {
            AdminDAL.UpdateAccInfoDAL(adm);
        }

        public static (AdminMenuBO, bool) GetAccInfoBLL(int accNo)
        {
            bool check;
            AdminMenuBO adm = new AdminMenuBO();
            (adm, check) = AdminDAL.GetAccInfoDAL(accNo);
            return (adm, check);
        }

        public static List<CustomerBO> GetAccByAmountBLL(double min, double max)
        {
            double temp = 0;
            List<CustomerBO> customerList = new List<CustomerBO>();

            //even if the user, by mistake, enters max instead of min
            //then it swaps the two values to show the correct result
            if (min > max)
            {
                temp = min;
                min = max;
                max = temp;
            }
            customerList = AdminDAL.GetAccByAmountDAL(min, max);
            return customerList;
        }

        //Search records b/w two dates
        public static List<TransacRecordBO> SearchByDateBLL(DateTime startDate, DateTime endDate, int accNo)
        {
            List<TransacRecordBO> transactionList = new List<TransacRecordBO>();
            transactionList = AdminDAL.SearchByDateDAL(startDate, endDate, accNo);
            return transactionList;
        }

        public static List<CustomerBO> SearchForAccBLL(string query)
        {
            List<CustomerBO> custList = new List<CustomerBO>();
            custList = AdminDAL.SearchForAccDAL(query);
            return custList;
        }
    }
}
