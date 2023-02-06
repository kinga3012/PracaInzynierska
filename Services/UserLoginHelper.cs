using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization;

namespace PracaInzynierska.Services
{
    [DataContract]
    public class UserLoginHelper
    {
        [DataMember]
        public bool LoginSuccess { get; set; }

        [DataMember]
        public string UserName { get; set; }

        public UserLoginHelper(string userName, bool loginSuccess)
        {
            UserName = userName;
            LoginSuccess = loginSuccess;
        }
        public UserLoginHelper()
        {
            UserName = "";
            LoginSuccess = false;
        }
    }

}
