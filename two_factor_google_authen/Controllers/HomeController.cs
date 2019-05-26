using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
//using two_factor_google_authen.DataProvider;
using two_factor_google_authen.ViewModel;
using two_factor_google_authen.UserDataBusiness;
using System.Data.SqlClient; //this namespace is for sqlclient server  
using System.Configuration; // this namespace is add I am adding connection name in web config file config connection name 
using System.Data;

namespace two_factor_google_authen.Controllers
{
    public class HomeController : Controller
    {

        private const string key = "qaz123!@@)(*"; // any 10-12 char string for use as private key in google authenticator
        private string connectionString = ConfigurationManager.ConnectionStrings["local"].ConnectionString;

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            try
            {
                //var bll = new UserDataBusiness();
                string message = "";
                bool status = false;
                bool isValid = false;

                isValid = validateUser(login);

                //check username and password form our database here
                //for demo I am going to use Admin as Username and Password1 as Password static value
                //if (login.Username == "Admin" && login.Password == "Password1")
                if (isValid)
                {
                    status = true; // show 2FA form
                    message = "2FA Verification";
                    Session["Username"] = login.Username;

                    //2FA Setup
                    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                    string UserUniqueKey = (login.Username + key); //as Its a demo, I have done this way. But you should use any encrypted value here which will be unique value per user.
                    Session["UserUniqueKey"] = UserUniqueKey;
                    var setupInfo = tfa.GenerateSetupCode("Dotnet Awesome", login.Username, UserUniqueKey, 300, 300);
                    ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                    ViewBag.SetupCode = setupInfo.ManualEntryKey;
                }
                else
                {
                    message = "Invalid credential";
                }
                ViewBag.Message = message;
                ViewBag.Status = status;
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult MyProfile()
        {
            if (Session["Username"] == null || Session["IsValid2FA"] == null || !(bool)Session["IsValid2FA"])
            {
                return RedirectToAction("Login");
            }

            ViewBag.Message = "Welcome " + Session["Username"].ToString();
            return View();
        }

        public ActionResult Verify2FA()
        {
            var token = Request["passcode"];
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string UserUniqueKey = Session["UserUniqueKey"].ToString();
            bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);
            if (isValid)
            {
                Session["IsValid2FA"] = true;
                return RedirectToAction("MyProfile", "Home");
            }
            return RedirectToAction("Login", "Home");
        }

        protected Boolean validateUser(LoginModel login)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                bool result = false;

                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("spValidateUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                //cmd.Parameters param1 = new SqlParameter();
                //SqlParameter param2 = new SqlParameter();

                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = login.Username;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = login.Password;

                conn.Open();
                cmd.ExecuteNonQuery();

                result = Convert.ToBoolean(cmd.ExecuteScalar());

                //int temp = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                //if (temp == 1)
                //{
                //    result = true;
                //}
                //else
                //{
                //    result = false;
                //}

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