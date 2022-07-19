using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MechanicWorkShop
{
    class YearRule : ValidationRule
    {

        int min;
        int max;

        public int Min { get => min; set => min = value; }
        public int Max { get => max; set => max = value; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            TextBox tb = value as TextBox;
            String textBoxValue = tb.Text.Trim();

            if (tb.Name == "txtFname")
            {
                if (textBoxValue.Length == 0)
                {
                    return new ValidationResult(false, "Please enter First name.");
                }
            }

            if (tb.Name == "txtLname")
            {
                if (textBoxValue.Length == 0)
                {
                    return new ValidationResult(false, "Please enter Last name.");
                }
            }

            if (tb.Name == "txtCreditCard")
            {
                if (textBoxValue.Length == 0)
                {
                    return new ValidationResult(false, "Please enter Credit Card No. name.");
                }

                if (!ValidateCardNumber(textBoxValue))
                {
                    return new ValidationResult(false, "Please enter only 16 digits numeric Credit Card No.");
                }
            }

            if (tb.Name == "txtStone")
            {
                if (textBoxValue.Length == 0)
                {
                    return new ValidationResult(false, "Please enter no of stones.");
                }
            }

            if (tb.Name == "txtNeighbour")
            {
                if (textBoxValue.Length == 0)
                {
                    return new ValidationResult(false, "Please enter no of neighbours you want to take concent from.");
                }
            }

            if (tb.Name == "txtObstacles")
            {
                if (textBoxValue.Length == 0)
                {
                    return new ValidationResult(false, "Please enter no of obstacles");
                }
            }

            if (tb.Name == "txtLotSize")
            {
                if (textBoxValue.Length == 0)
                {
                    return new ValidationResult(false, "Please enter lotsize.");
                }
            }

            if (tb.Name == "txtWorkingArea")
            {
                if (textBoxValue.Length == 0)
                {
                    return new ValidationResult(false, "Please enter working area.");
                }
            }

            return ValidationResult.ValidResult;
        }

        private bool ValidateCardNumber(String cardNumber)
        {
            bool isValidCard = false;
            try
            {
                if (cardNumber.Length == 16)
                {
                    Char[] cardNo = cardNumber.ToCharArray();
                    for (int i = 0; i < cardNo.Length; i++)
                    {
                        if (Char.IsDigit(cardNo[i]))
                        {
                            isValidCard = true;
                        }
                        else
                        {
                            isValidCard = false;
                            break;
                        }
                    }
                }
                else
                {
                    isValidCard = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return isValidCard;
        }
    }
}
