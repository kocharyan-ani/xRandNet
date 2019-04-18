using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    public class Research
    {
        public string name { get; set; }
        public string research { get; set; }
        public string model { get; set; }
        public string generation { get; set; }
        public bool connected { get; set; }
        public int count { get; set; }
    }
}