using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.SqlClient; //this namespace is for sqlclient server  
using System.Configuration; // this namespace is add I am adding connection name in web config file config connection name 
using two_factor_google_authen.ViewModel;
using Dapper;
using System.Data;
using System.Threading;

namespace two_factor_google_authen.DataProvider
{
    public class UserDataProvider : IUserDataProvider
    {

        //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["local"].ToString());
        string connectionString = ConfigurationManager.ConnectionStrings["local"].ConnectionString;

                public bool ValidateUser(LoginModel login)
        {
            throw new NotImplementedException();
        }

        protected Boolean validateUser( LoginModel login)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand();
                SqlParameter param1 = new SqlParameter();
                SqlParameter param2 = new SqlParameter();
                bool result = false;
            

                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = login.Username;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = login.Password;

                cmd = new SqlCommand("spValidateUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                cmd.ExecuteNonQuery();

                int temp = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                if (temp == 1)
                {
                    result =  true;
                }
                else
                {
                    result = false;
                }

                conn.Close();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}