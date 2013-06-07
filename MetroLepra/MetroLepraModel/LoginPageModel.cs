using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLepra.Model
{
    public class LoginPageModel
    {
        public String CaptchaImageUrl { get; set; }
        public String LoginCode { get; set; }
        public Stream CaptchaImageStream { get; set; }
    }
}
