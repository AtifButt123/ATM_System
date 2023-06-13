//bsef19m001
//Atif Iqbal Butt
//EAD Assignment # 01

//Admin login: admin
//Admin pincode: 123

/************************** LoginDAL file (Presentation Layer) **************************/


using ATM_BO;
using Microsoft.Data.SqlClient;
namespace ATM_DAL
{
    public static class LoginDAL
    {

        //initiating database connection
        static readonly string connString;
        static readonly SqlConnection con;
        static LoginDAL()
        {
            connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM_System_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con = new SqlConnection(connString);
        }
        public static LoginBO CheckLoginDB(LoginBO userLogin)
        {
            con.Open();
            string query = @"Select Id, type from Login where username = @u and pincode = @p";
            SqlParameter p1 = new SqlParameter("u", userLogin.username);
            SqlParameter p2 = new SqlParameter("p", userLogin.pincode);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            SqlDataReader dr = cmd.ExecuteReader();
            userLogin.userType = 'n';
            if (dr.Read())
            {
                userLogin.id = Convert.ToInt32(dr[0]);
                userLogin.userType = Convert.ToChar(dr[1]);
            }
            con.Close();
            return userLogin;      
        }

        public static void DisableLoginDB(LoginBO user)
        {
            con.Open();
            string query = @"update Customer set status = 'disabled' 
                             where loginID = (select Id from Login 
                             where username = @u)";
            SqlParameter p = new SqlParameter("u", user.username);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p);
            int rows = cmd.ExecuteNonQuery();
            con.Close();
        }

        public static bool IsLoginExistDB(LoginBO user)
        {
            con.Open();
            string query = @"Select Id from Login where username = @u";
            SqlParameter p = new SqlParameter("u", user.username);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }
        }

    }
}