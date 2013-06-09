using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
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

                var postBody = HttpUtility.HtmlDecode(jToken["body"].Value<String>());

                var imageRegex = "img src=\"(.+?)\"";
                var imageMatches = Regex.Matches(postBody, imageRegex);
                var headerImage = "";

                foreach (Match match in imageMatches)
                {
                    if (String.IsNullOrEmpty(headerImage))
                        headerImage = "http://src.sencha.io/80/80/" + match.Groups[1].Value;

                    //TODO: Optimize for screen below 720p
                    //postBody = postBody.Replace(match.Groups[1].Value, "http://src.sencha.io/" + 1280 + "/" + match.Groups[1].Value);
                }

                var doc = new HtmlDocument();
                doc.LoadHtml(postBody);
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
                post.HeaderImageUrl = headerImage;
                post.HeaderText = text;
                post.TotalCommentsCount = jToken["comm_count"].Value<String>();
                post.UnreadCommentsCount = jToken["unread"].Value<String>();

                post.Author = new UserModel
                                {
                                    Username = jToken["login"].Value<String>(), 
                                    CustomRank = jToken["user_rank"].Value<String>(), 
                                    Gender = (UserGender) jToken["gender"].Value<int>()
                                };
                post.Date = jToken["textdate"].Value<String>();
                post.Time = jToken["posttime"].Value<String>();
                post.Vote = jToken["user_vote"].Value<int>();

                posts.Add(post);
            }

            return posts;
        }
    }
}
