using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicWorkShop
{
    public class Appointment : IComparable<Appointment>
    {

        private String time;
        private Customer customer;

        public string Time { get => time; set => time = value; }
        public Customer Customer { get => customer; set => customer = value; }

        //This method will compare List of appointment as per User's choice
        public int CompareTo(Appointment obj)
        {
            int result = this.Customer.Name.CompareTo(obj.Customer.Name);
            return result;
        }

    }
}
