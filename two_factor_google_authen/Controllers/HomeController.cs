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

namespace two_factor_google_authen.Controllers
{
    public class HomeController : Controller
    {

        private const string key = "qaz123!@@)(*"; // any 10-12 char string for use as private key in google authenticator

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            try
            {
                var bll = new UserDataBusiness();
                string message = "";
                bool status = false;
                bool isValid = false;

                isValid = userDataProvider.ValidateUser(login);

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

    }
}