using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Proc
    {
        public double Procc(double totalSale)
        {
            if (totalSale <= 10000)
            {
                return 0;
            }
            else if (totalSale > 10000 && totalSale <= 50000)
            {
                return 5;
            }
            else if (totalSale > 50000 && totalSale <= 300000)
            {
                return 10;
            }
            else
            {
                return 15;
            }
        }
    }
}
