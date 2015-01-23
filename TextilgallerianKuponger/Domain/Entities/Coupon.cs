using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities {
    public class Coupon {
        public String Code { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<Customer> CustomersValidFor { get; set; }
        public List<Customer> CustomersUsedBy { get; set; }
        public List<Product> Products { get; set; }
//        public List<String> Emails { get; set; }
//        public List<String> SocialSecurityNumber { get; set; }
        public bool CanBeCombined { get; set; }
    }
}
