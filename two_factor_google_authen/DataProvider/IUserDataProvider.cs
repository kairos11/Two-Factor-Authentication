using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using two_factor_google_authen.ViewModel;

namespace two_factor_google_authen.DataProvider
{
    public interface IUserDataProvider
    {

      bool ValidateUser(LoginModel login);

    }
}
