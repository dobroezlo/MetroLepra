using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MetroLepra.Model;
using Newtonsoft.Json.Linq;

namespace MetroLepra.Core
{
    public class JsonParser
    {
        public static LeproPanelModel ParseLeproPanel(string jsonData)
        {
            var jObject = JObject.Parse(jsonData);

            var leproPanelModel = new LeproPanelModel
            {
                UnreadInboxPostsCount = jObject["inboxunreadposts"].Value<int>(),
                UnreadInboxCommentsCount = jObject["inboxunreadcomms"].Value<int>(),
                Karma = jObject["karma"].Value<int>(),
                CommentsRatings = jObject["rating"].Value<int>(),
                VoteWeight = jObject["voteweight"].Value<int>(),
                UnreadCommentsCount = jObject["myunreadcomms"].Value<int>(),
                UnreadPostsCount = jObject["myunreadposts"].Value<int>()
            };

            return leproPanelModel;
        }

        public static List<PostModel> ParsePosts(string jsonData)
        {
            var posts = new List<PostModel>();

            var jObject = JObject.Parse(jsonData);

            var postsJson = jObject["posts"];

            foreach (var postJObject in postsJson)
            {
                var jToken = postJObject.First;

                var postBody = jToken["body"].Value<String>();

                var imageRegex = "img src=\"(.+?)\"";
                var imageMatches = Regex.Matches(postBody, imageRegex);
                var img = "";

                foreach (Match match in imageMatches)
                {
                    if (String.IsNullOrEmpty(img))
                        img = "http://src.sencha.io/80/80/" + match.Groups[1].Value;

                    //TODO: Optimize for screen below 720p
                    postBody = postBody.Replace(match.Groups[1].Value, "http://src.sencha.io/" + 1280 + "/" + match.Groups[1].Value);
                }

                var text = Regex.Replace(postBody, "(<([^>]+)>)", " ");
                if (text.Length > 140)
                {
                    text = text.Substring(0, 140);
                    text += "...";
                }

                var post = new PostModel();
                post.Id = jToken["id"].Value<String>();
                post.Body = postBody;
                post.Rating = jToken["rating"].Value<String>();
                post.Url = jToken["domain_url"].Value<String>();
                post.Image = img;
                post.Text = text;
                post.User = jToken["login"].Value<String>();
                post.Comments = jToken["comm_count"].Value<String>() + " / " + jToken["unread"].Value<String>();
                post.Wrote = String.Format("{0} {1}{2}", jToken["gender"].Value<int>() == 1 ? "Написал " : "Написала ",
                                           (String.IsNullOrEmpty(jToken["user_rank"].Value<String>()) ? "" : jToken["user_rank"].Value<String>() + " "),
                                           jToken["login"].Value<String>());

                post.When = jToken["textdate"] + " в " + jToken["posttime"];
                post.Vote = jToken["user_vote"].Value<int>();

                posts.Add(post);
            }

            return posts;
        }
    }
}
