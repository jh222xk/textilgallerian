using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities {
    /// <summary>
    /// Customer to check if valid for discount
    /// </summary>
    public class Customer {
        public String SocialSecurityNumber { get; set; }
        public String Email { get; set; }
    }
}
