using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funds.Dtos
{
    public class JzinfoDto
    {
        public DateTime Date { get; set; }
        public decimal Dwjz { get; set; }
        public decimal Ljjz { get; set; }
        public decimal RiseRate { get; set; }
    }
}
