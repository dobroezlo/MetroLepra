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

        public string HeaderImageUrl { get; set; }

        public string HeaderText { get; set; }

        public UserModel Author { get; set; }

        public string Comments { get; set; }

        public string Wrote { get; set; }

        public string When { get; set; }

        public int Vote { get; set; }

        public string TotalCommentsCount { get; set; }

        public string UnreadCommentsCount { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string Type { get; set; }

        public string AddCommentCode { get; set; }
    }
}
