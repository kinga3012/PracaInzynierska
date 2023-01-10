using System.Runtime.Serialization;

namespace PracaInzynierska.Services
{
    [DataContract]
    public class UserHelper
    {
        [DataMember]
        public bool LoginSuccess { get; set; }

        [DataMember]
        public string UserName { get; set; }

        public UserHelper(string userName, bool loginSuccess)
        {
            UserName = userName;
            LoginSuccess = loginSuccess;
        }
        public UserHelper()
        {
            UserName = "";
            LoginSuccess = false;
        }
    }

}
