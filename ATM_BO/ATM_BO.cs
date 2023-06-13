namespace ATM_BO
{
    public class LoginBO
    {
        public int id { get; set; }
        public string username { get; set; }
        public string pincode { get; set; }
        public char userType { get; set; }
    }
    public class CustomerBO
    {
        public int id { get; set ;}
        public string? name { get; set; }
        public int accNo { get; set; }
        public string? accType { get; set; }
        public double balance { get; set; }
        public string? status { get; set; }
        public int loginID { get; set; }
        public CustomerBO()
        {
            name = null;
            accType = null;
            status = null;
        }
    }
    public class AdminBO
    {
        public int id { get; set ; }
        public string name { get; set; }
        public int loginID { get; set; }
    }
    public class AdminMenuBO
    {
        public int id { get; set; }
        public string username { get; set; }
        public int accountNo { get; set; }
        public string pinCode { get; set; }
        public string holderName { get; set; }

        public string accType { get; set; }
        public double balance { get; set; }
        public string status { get; set; }

    }

    public class TransacRecordBO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string transacType { get; set; }
        public double amount { get; set; }
        public DateTime date { get; set; }
        public int userID { get; set; }
    }
}