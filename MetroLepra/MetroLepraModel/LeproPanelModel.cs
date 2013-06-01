using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLepra.Model
{
    public class LeproPanelModel
    {
        public int UnreadInboxPostsCount { get; set; }

        public int UnreadInboxCommentsCount { get; set; }

        public int Karma { get; set; }

        public int CommentsRatings { get; set; }

        public int VoteWeight { get; set; }

        public int UnreadCommentsCount { get; set; }

        public int UnreadPostsCount { get; set; }
    }
}
