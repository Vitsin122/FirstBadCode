using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP4_Project.Table1
{
    class differentiated_payment
    {
        public int Term { get; set; }
        public double Payment { get; set; }

        public differentiated_payment(int term, double payment)
        {
            Term = term;
            Payment = payment;
        }
    }
}
