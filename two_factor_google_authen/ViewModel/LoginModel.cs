using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace two_factor_google_authen.ViewModel
{
    //public class LoginModel
    //{
    //    public string Username { get; set; }
    //    public string Password  { get; set; }
    //}

    public class LoginModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public enum AccountStatus
    {
        Submitted,
        Approved,
        Rejected
    }

}