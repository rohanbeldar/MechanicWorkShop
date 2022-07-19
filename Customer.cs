using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicWorkShop
{
    public class Customer
    {
        private String name;
        private String surname;
        private String creditCard;
        private Vehicle vehicle;

        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string CreditCard { get => creditCard; set => creditCard = value; }
        public Vehicle Vehicle { get => vehicle; set => vehicle = value; }
        public String FullName { get => name + ' ' + surname; }

    }
}
