//bsef19m001
//Atif Iqbal Butt
//EAD Assignment # 01

//Admin login: admin
//Admin pincode: 123

/************************** AdminDAL file (Presentation Layer) **************************/


using Microsoft.Data.SqlClient;
using ATM_BO;
namespace ATM_DAL
{
    public static class AdminDAL
    {

        //initiating database connection
        static readonly string connString;
        static readonly SqlConnection con;
        static AdminDAL()
        {
            connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM_System_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con = new SqlConnection(connString);
        }

        //Create account
        public static int CreateAccDAL(AdminMenuBO adm)
        {
            con.Close();
            con.Open();

            //Insert the login crediantials of new user in  Login table
            string query = @"insert into Login(username, pincode, type) values(@u, @p, 'c')";
            SqlParameter p1 = new SqlParameter("u", adm.username);
            SqlParameter p2 = new SqlParameter("p", adm.pinCode);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            int rows = cmd.ExecuteNonQuery();
            con.Close();
            con.Open();
            query = @"Select Id from Login where username = @user";
            SqlParameter p3 = new SqlParameter("user", adm.username);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p3);
            SqlDataReader dr = cmd.ExecuteReader();
            int loginID = 0;
            if (dr.Read())
            {
                loginID = Convert.ToInt32(dr[0]);
            }
            con.Close();
            con.Open();

            //Getting the last account number in order to provide next account number
            query = @"Select count(accNo) from Customer";
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            int accNo = 0;
            int count = 0;
            if (dr.Read())
            {
                count = Convert.ToInt32(dr[0]);
            }
            con.Close();
            con.Open();
            if (count != 0)
            {
                query = @"select max(accNo) from Customer";
                cmd = new SqlCommand(query, con);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    accNo = Convert.ToInt32(dr[0]);
                }
            }
            con.Close();
            con.Open();

            //Enter new record in Customer table
            query = @"insert into Customer(accNo, name, accType, balance, status, loginID) values(@acNo + 1, @name, @acType, @bal, @stat, @id)";
            SqlParameter p4 = new SqlParameter("acNo", accNo);
            SqlParameter p5 = new SqlParameter("name", adm.holderName);
            SqlParameter p6 = new SqlParameter("acType", adm.accType);
            SqlParameter p7 = new SqlParameter("bal", adm.balance);
            SqlParameter p8 = new SqlParameter("stat", adm.status);
            SqlParameter p9 = new SqlParameter("id", loginID);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            cmd.Parameters.Add(p6);
            cmd.Parameters.Add(p7);
            cmd.Parameters.Add(p8);
            cmd.Parameters.Add(p9);
            rows = cmd.ExecuteNonQuery();
            con.Close();
            return accNo;
        }

        public static bool DeleteExistAccDAL(int accNo)
        {
            con.Open();
            string query = @"Delete from Login where id = (Select loginID from Customer where accNo = @an)";
            SqlParameter p = new SqlParameter("an", accNo);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p);
            int rows = cmd.ExecuteNonQuery();
            con.Close();
            if (rows > 0)
                return true;
            else
                return false;
        }

        public static void UpdateAccInfoDAL(AdminMenuBO adm)
        {
            con.Close();
            con.Open();

            //Update in Customer Table
            string query = @"update Customer set name = @n, status = @s where accNo = @an";
            SqlParameter p1 = new SqlParameter("n", adm.holderName);
            SqlParameter p2 = new SqlParameter("s", adm.status);
            SqlParameter p3 = new SqlParameter("an", adm.accountNo);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            int rows = cmd.ExecuteNonQuery();
            con.Close();
            con.Open();

            //Update in Login Table
            query = @"update Login set username = @u, pincode = @p 
                      where Id = (select loginID from Customer where accNo = @an)";
            SqlParameter p4 = new SqlParameter("u", adm.username);
            SqlParameter p5 = new SqlParameter("p", adm.pinCode);
            SqlParameter p6 = new SqlParameter("an", adm.accountNo);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            cmd.Parameters.Add(p6);
            rows = cmd.ExecuteNonQuery();
            con.Close();
        }
        public static (AdminMenuBO, bool) GetAccInfoDAL(int accNo)
        {
            con.Open();
            AdminMenuBO adm = new AdminMenuBO();
            string query = @"Select name, accType, balance, status, username, 
                             pincode from Customer inner join Login on 
                             Customer.loginID = Login.Id where accNo = @an";
            SqlParameter p = new SqlParameter("an", accNo);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                adm.holderName = dr[0].ToString();
                adm.accType = dr[1].ToString();
                adm.balance = Convert.ToDouble(dr[2]);
                adm.status = dr[3].ToString();
                adm.username = dr[4].ToString();
                adm.pinCode = dr[5].ToString();
                con.Close();
                return (adm, true);
            }
            else
            {
                con.Close();
                return (adm, false);
            }
        }

        //Get records with amount between two values
        public static List<CustomerBO> GetAccByAmountDAL(double min, double max)
        {
            List<CustomerBO> customerList = new List<CustomerBO>();
            con.Open();
            string query = @"Select id, accNo, name, accType, balance, status 
                             from Customer where balance >= @min AND balance <= @max";

            SqlParameter p1 = new SqlParameter("@min", min);
            SqlParameter p2 = new SqlParameter("@max", max);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CustomerBO c = new CustomerBO();
                c.id = Convert.ToInt32(dr[0]);
                c.accNo = Convert.ToInt32(dr[1]);
                c.name = Convert.ToString(dr[2]);
                c.accType = Convert.ToString(dr[3]);
                c.balance = Convert.ToDouble(dr[4]);
                c.status = Convert.ToString(dr[5]);
                customerList.Add(c);
            }
            con.Close();
            return customerList;
        }

        //search record within two dates
        public static List<TransacRecordBO> SearchByDateDAL(DateTime startDate, DateTime endDate, int accNo)
        {
            con.Close();
            con.Open();
            List<TransacRecordBO> TransRecordList = new List<TransacRecordBO>();
            string query = @"Select userID, name, amount, transacType, date 
                             from TransacRecord inner join Customer 
                             on TransacRecord.userID = Customer.id where date >= @minD AND date <= @maxD and accNo = @an";

            SqlParameter p1 = new SqlParameter("@minD", startDate);
            SqlParameter p2 = new SqlParameter("@maxD", endDate);
            SqlParameter p3 = new SqlParameter("@an", accNo);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                TransacRecordBO transRecord = new TransacRecordBO();
                transRecord.userID = Convert.ToInt32(dr[0]);
                transRecord.name = Convert.ToString(dr[1]);
                transRecord.amount = Convert.ToDouble(dr[2]);
                transRecord.transacType = Convert.ToString(dr[3]);
                transRecord.date = Convert.ToDateTime(dr[4]);
                TransRecordList.Add(transRecord);
            }
            con.Close();
            return TransRecordList;
        }

        public static List<CustomerBO> SearchForAccDAL(string query)
        {
            con.Close();
            con.Open();

            //add all records in the list then return the list
            List<CustomerBO> custList = new List<CustomerBO>();

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CustomerBO cust = new CustomerBO();
                cust.accNo = Convert.ToInt32(dr[0]);
                cust.id = Convert.ToInt32(dr[1]);
                cust.name = Convert.ToString(dr[2]);
                cust.balance = Convert.ToDouble(dr[3]);
                cust.accType = Convert.ToString(dr[4]);
                cust.status = Convert.ToString(dr[5]);
                custList.Add(cust);
            }
            con.Close();
            return custList;
        }
    }
}
