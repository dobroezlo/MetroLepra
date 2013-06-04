using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLepra.Model
{
    public class MainPageModel
    {
        public string VoteCode { get; set; }

        public string MyThingsHandlerCode { get; set; }

        public string ChatHandlerCode { get; set; }

        public string Username { get; set; }

        public string LogoutCode { get; set; }

        public List<SubLepraModel> MySubLepras { get; set; }
    }
}
