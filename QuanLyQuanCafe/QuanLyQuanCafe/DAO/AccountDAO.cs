using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountDAO() { }


        public bool Login(string username, string password)
        {
            string query = " select* from dbo.account where username = @username and password = @password";
            DataTable result = DataProvider.Instance.ExcuteQuery(query,new object[] { username, password});
            return result.Rows.Count > 0;
        }
        public bool UpdateAccount(string username, string displayname,string pass, string newpass  )
        {
            int result = DataProvider.Instance.ExcuteNonQuery("exec USP_UpdateAccount @userName , @displayName , @password , @newPassword", new object[] { username, displayname, pass, newpass });
            return result > 0;
        }
        public DataTable GetListAccount()
        {
            string query = "select UserName,DisplayName,Type from account";
            return DataProvider.Instance.ExcuteQuery(query);
        }
        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExcuteQuery("select * from Account where userName ='" + userName + "'");
            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }
        public bool InsertAccount(string username, string displayname, int type)
        {
            string query = string.Format("Insert into Account (UserName, DisplayName, Type, PassWord) values (N'{0}', N'{1}', {2}, N'1962026656160185351301320480154111117132155' )", username, displayname, type);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateAccount(string username, string displayname, int type)
        {
            string query = string.Format("update Account set  DisplayName = N'{1}', Type= {2} where UserName = N'{0}'", username, displayname, type);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteAccount(string username)
        {
            string query = string.Format("delete Account  where UserName = N'{0}'", username);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool ResetPass(string username)
        {
            string query = string.Format("update Account  set password = 0 where UserName = N'1962026656160185351301320480154111117132155'", username);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
    }
}
