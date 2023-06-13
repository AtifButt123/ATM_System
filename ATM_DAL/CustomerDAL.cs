//bsef19m001
//Atif Iqbal Butt
//EAD Assignment # 01

//Admin login: admin
//Admin pincode: 123

/************************** CustomerDAL file (Date Access Layer) **************************/


using Microsoft.Data.SqlClient;
using ATM_BO;
namespace ATM_DAL
{
    public static class CustomerDAL
    {
        //initiating database connection
        static readonly string connString;
        static readonly SqlConnection con;
        static CustomerDAL()
        {
            connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM_System_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con = new SqlConnection(connString);
        }

        public static CustomerBO ValidateMoneyDB(LoginBO userLogin)
        {
            con.Close();
            con.Open();
            CustomerBO c = new CustomerBO();
            string query = @"select balance, id from Customer where loginID = @id ";
            SqlParameter p = new SqlParameter("id", userLogin.id);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                c.balance = Convert.ToDouble(dr[0]);
                c.id = Convert.ToInt32(dr[1]);
            }
            con.Close();
            return c;
        }
        public static void WithdrawDB(CustomerBO c, double amount)
        {
            con.Open();
            string query = @"update Customer set balance = @b where id = @id";
            SqlParameter p1 = new SqlParameter("id", c.id);
            SqlParameter p2 = new SqlParameter("b", c.balance);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            int rows = cmd.ExecuteNonQuery();
            con.Close();
            con.Open();

            //put the transaction record in the TransacRecord table
            query = @"insert into TransacRecord(userID, transacType, amount, date) values(@id, 'Cash Withdrawal', @amt, @date)";
            SqlParameter p3 = new SqlParameter("id", c.id);
            SqlParameter p4 = new SqlParameter("amt", amount);
            SqlParameter p5 = new SqlParameter("date", DateTime.Now);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            rows = cmd.ExecuteNonQuery();
            con.Close();
        }

        public static double CheckWithdrawLimitDAL(TransacRecordBO transObj)
        {
            con.Close();
            con.Open();
            double totalAmount = 0;
            string query = @"Select id from Customer where loginID = @id";
            SqlParameter p = new SqlParameter("id", transObj.userID);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                transObj.userID = Convert.ToInt32(dr[0]);
            }
            con.Close();

            //check if record exist or not
            con.Open();
            query = @"Select count(userID) from TransacRecord where userID = @id 
                      and transacType = 'Cash Withdrawal' and date = @d";
            SqlParameter p1 = new SqlParameter("id", transObj.userID);
            SqlParameter p2 = new SqlParameter("d", transObj.date.Date);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            dr = cmd.ExecuteReader();
            int count = 0;
            if (dr.Read())
            {
                count = Convert.ToInt32(dr[0]);
            }
            con.Close();

            //if record exist then return the total amount of withdrawal on withdrawal date
            if (count != 0)
            {
                con.Open();
                query = @"Select sum(amount) from TransacRecord where userID = @id 
                      and transacType = 'Cash Withdrawal' and date = @d";
                SqlParameter p3 = new SqlParameter("id", transObj.userID);
                SqlParameter p4 = new SqlParameter("d", transObj.date);
                cmd = new SqlCommand(query, con);
                cmd.Parameters.Add(p3);
                cmd.Parameters.Add(p4);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    totalAmount = Convert.ToDouble(dr[0]);
                    //Console.WriteLine(totalAmount);
                }
                con.Close();
                return totalAmount;
            }
            else
            {
                return totalAmount;
            }
        }
        public static CustomerBO GetReceiptDB(LoginBO userLogin)
        {
            con.Open();
            CustomerBO c = new CustomerBO();
            string query = @"select accNo, balance from Customer where loginID = @id";
            SqlParameter p = new SqlParameter("id", userLogin.id);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                c.accNo = Convert.ToInt32(dr[0]);
                c.balance = Convert.ToDouble(dr[1]);
            }
            con.Close();
            return c;
        }

        public static CustomerBO GetAccConfirmationDB(CustomerBO c)
        {
            con.Open();
            string query = @"select name from Customer where accNo = @accNO";
            SqlParameter p = new SqlParameter("accNo", c.accNo);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                c.name = Convert.ToString(dr[0]);
            }
            con.Close();
            return c;
        }

        public static void CashTransferDB(CustomerBO c, int loginId)
        {
            //it adds the cash to the recipent acc
            con.Open();
            LoginBO userLogin = new LoginBO();
            userLogin.id = loginId;
            string query = @"update Customer set balance = balance + @b where accNo = @a";
            SqlParameter p1 = new SqlParameter("b", c.balance);
            SqlParameter p2 = new SqlParameter("a", c.accNo);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            int rows = cmd.ExecuteNonQuery();
            
            //it deducts the cash from the sender acc
            query = @"update Customer set balance = balance - @b where loginId = @id";
            SqlParameter p3 = new SqlParameter("id", loginId);
            SqlParameter p4 = new SqlParameter("b", c.balance);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            rows = cmd.ExecuteNonQuery();
            con.Close();

            //record the transaction
            query = @"insert into TransacRecord(userID, transacType, amount, RecipientAccNo, date) values(@uid, 'Cash Transfer', @a, @an, @d)";
            CustomerBO c1 = ValidateMoneyDB(userLogin);
            con.Open();
            SqlParameter p5 = new SqlParameter("uid", c1.id);
            SqlParameter p6 = new SqlParameter("a", c.balance);
            SqlParameter p7 = new SqlParameter("an", c.accNo);
            SqlParameter p8 = new SqlParameter("d", DateTime.Now);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p5);
            cmd.Parameters.Add(p6);
            cmd.Parameters.Add(p7);
            cmd.Parameters.Add(p8);
            rows = cmd.ExecuteNonQuery();
            con.Close();

        }
        public static void DepositCashDB(CustomerBO c)
        {
            con.Close();
            con.Open();
            LoginBO userLogin = new LoginBO();
            userLogin.id = c.loginID;

            //deposit the cash
            string query = @"update Customer set balance = balance + @b where loginID = @id";
            SqlParameter p1 = new SqlParameter("b", c.balance);
            SqlParameter p2 = new SqlParameter("id", c.loginID);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            int rows = cmd.ExecuteNonQuery();
            CustomerBO c1 = ValidateMoneyDB(userLogin);
            con.Close();
            con.Open();

            //record the transaction
            query = @"insert into TransacRecord(userID, transacType, amount, date) values(@uid, 'Cash Deposit', @a, @d)";
            SqlParameter p3 = new SqlParameter("uid", c1.id);
            SqlParameter p4 = new SqlParameter("a", c.balance);
            SqlParameter p5 = new SqlParameter("d", DateTime.Now);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            rows = cmd.ExecuteNonQuery();
            con.Close();
        }

        public static string CheckAccountStatusDAL(int loginID)
        {
            con.Open();
            string query = @"select status from Customer where loginID = @id";
            SqlParameter p = new SqlParameter("id", loginID);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string status = dr[0].ToString();
                con.Close();
                return status;
            }
            else
            {
                con.Close();
                return "false";
            }
        }
        public static string IsAccountActiveDAL(int accNo)
        {
            con.Open();
            string query = @"select status from Customer where accNo = @an";
            SqlParameter p = new SqlParameter("an", accNo);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string status = dr[0].ToString();
                con.Close();
                return status;
            }
            else
            {
                con.Close();
                return "false";
            }
        }
    }
}
