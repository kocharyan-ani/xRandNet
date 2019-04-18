using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ResearchBody
    {
        public string name { get; set; }
        public string research { get; set; }
        public string model { get; set; }
        public string generation { get; set; }
        public bool connected { get; set; }
        public int count { get; set; }
        public List<OptionBody> analyzeOptions { get; set; }
        public List<OptionBody> parameters { get; set; }
    }
}