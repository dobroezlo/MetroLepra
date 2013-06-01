using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLepra.Model
{
    public class PostModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public string Rating { get; set; }

        public string Url { get; set; }

        public string Image { get; set; }

        public string Text { get; set; }

        public string User { get; set; }

        public string Comments { get; set; }

        public string Wrote { get; set; }

        public string When { get; set; }

        public int Vote { get; set; }
    }
}
