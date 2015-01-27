using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    ///  Class to handle % discount on total sum for regulars.
    /// </summary>
    class Regular : Customer
    {
        public decimal Percentage { get; set; }
    }
}
