using System.Text.RegularExpressions;

namespace ValidationTest
{
    public class Functions
    {
        // Fields

        public string item;
        public string quantity;
        public string category;
        public string price;
        public string productID;
        public string orderID;

        // Functions

        public bool IsInputEmpty(string msg)
        {
            return msg == "";
        }

        public bool CheckForValidStockInput()
        {
            if (IsInputEmpty(item))
            {
                return false;
            }
            else if (IsInputEmpty(quantity))
            {
                return false;
            }
            else if (!Regex.Match(quantity, "^[1-9][0-9]*$").Success)
            {
                return false;
            }
            else if (IsInputEmpty(category))
            {
                return false;
            }
            else if (IsInputEmpty(price))
            {
                return false;
            }
            // TODO: Make regex accept decimals
            else if (!Regex.Match(price, "^[1-9][0-9]*$").Success)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckForValidSaleInput()
        {
            if (IsInputEmpty(productID))
            {
                return false;
            }
            else if (!Regex.Match(productID, "^[1-9][0-9]*$").Success)
            {
                return false;
            }
            else if (IsInputEmpty(orderID))
            {
                return false;
            }
            else if (!Regex.Match(orderID, "^[1-9][0-9]*$").Success)
            {
                return false;
            }
            else if (IsInputEmpty(quantity))
            {
                return false;
            }
            else if (!Regex.Match(quantity, "^[1-9][0-9]*$").Success)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
