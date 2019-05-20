using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using two_factor_google_authen.ViewModel;
using two_factor_google_authen.DataProvider;

namespace two_factor_google_authen.UserDataBusiness
{
    public class UserDataBusiness
    {

        UserDataProvider userData = new UserDataProvider();

        public Boolean validatevalidateUserBuss(LoginModel login){

            return userData.ValidateUser(login);

        }
    }
}