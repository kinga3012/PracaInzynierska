using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization;

namespace PracaInzynierska.Services
{
    [DataContract]
    public class UserRegisterHelper
    {
        [DataMember]
        public bool RegisterSuccess { get; set; }

        [DataMember]
        public List<string> errors { get; set; }

        public UserRegisterHelper(List<IdentityError>? errors, bool registerSuccess)
        {
            RegisterSuccess = registerSuccess;
            this.errors = new();
            if (errors != null)
            {
                this.errors = (errors.Select(error => error.Description)).ToList();
            }

        }
        public UserRegisterHelper()
        {
            RegisterSuccess = true;
            errors = new List<string>();
        }
    }
}
