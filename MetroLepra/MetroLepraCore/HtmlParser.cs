using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MetroLepra.Model;

namespace MetroLepra.Core
{
    public class HtmlParser
    {
        public static UserModel ParseUserProfile(string htmlData)
        {
            var user = new UserModel();

            htmlData = CleanupHtml(htmlData);

            var username = Regex.Match(htmlData, "<a href=\"/users/(.+?)\" usernum=\".+?\">.+?</a>").Groups[1].Value;

            var userPic = Regex.Match(htmlData, "<table class=\"userpic\"><tbody><tr><td><img src=\"(.+?)\".+?/>").Groups[1].Value;

            var regData = Regex.Match(htmlData, "<div class=\"userregisterdate\">(.+?)</div>").Groups[1].Value;
            regData = Regex.Replace(regData, "\\.", "");
            var regDataArray = regData.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            var number = regDataArray[0];
            var date = regDataArray[1];

            var name = Regex.Match(htmlData, "<div class=\"userbasicinfo\"><h3>(.+?)</h3>").Groups[1].Value;
            var location = Regex.Match(htmlData, "<div class=\"userego\">(.+?)</div>").Groups[1].Value;
            var karma = Regex.Match(htmlData, "<span class=\"rating\" id=\"js-user_karma\".+?><em>(.+?)</em>").Groups[1].Value;

            var statWroteMatch = Regex.Match(htmlData, "<div class=\"userstat userrating\">(.+?)</div>");
            var statRateMatch = Regex.Match(htmlData, "<div class=\"userstat uservoterate\">Вес голоса&nbsp;&#8212; (.+?)<br.*?>Голосов в день&nbsp;&#8212; (.+?)</div>");

            var userStat = Regex.Replace(statWroteMatch.Groups[1].Value, "(<([^>]+)>)", " ");
            var voteStat = "Вес голоса&nbsp;&#8212; " + statRateMatch.Groups[1].Value + ",<br>Голосов в день&nbsp;&#8212; " + statRateMatch.Groups[2].Value;

            var story = Regex.Match(htmlData, "<div class=\"userstory\">(.+?)</div>").Groups[1].Value;

            var contactsMatch = Regex.Match(htmlData, "<div class=\"usercontacts\">(.+?)</div>");
            var contacts = Regex.Split(contactsMatch.Groups[1].Value, "<br.*?>");

            user.Username = username;
            user.Userpic = userPic;
            user.Number = number;
            user.RegistrationDate = date;
            user.FullName = name;
            user.Location = location;
            user.Karma = karma;
            user.UserStat = userStat;
            user.VoteStat = voteStat;
            user.Contacts = contacts;
            user.Description = story;

            return user;
        }

        public static List<PostModel> ParsePosts(string htmlData)
        {
            var posts = new List<PostModel>();

            htmlData = CleanupHtml(htmlData);

            var postRegex = "<div class=\"post.+?id=\"(.+?)\".+?class=\"dt\">(.+?)</div>.+?<div class=\"p\">(Написал|Написала)(.+?)<a href=\".*?/users/.+?\".*?>(.+?)</a>,(.+?)в(.+?)<span>.+?<a href=\".*?/(comments|inbox)/.+?\">(.+?)</span>.+?.+?(<div class=\"vote\".+?><em>(.+?)</em></span>|</div>)(<a href=\"#\".+?class=\"plus(.*?)\">.+?<a href=\"#\".+?class=\"minus(.*?)\">|</div>)";
            var matches = Regex.Matches(htmlData, postRegex);
            foreach (Match match in matches)
            {
                var postBody = match.Groups[2].Value;

                var imageRegex = "img src=\"(.+?)\"";
                var imageMatches = Regex.Matches(postBody, imageRegex);
                var img = "";

                foreach (Match imageMatch in imageMatches)
                {
                    if (String.IsNullOrEmpty(img))
                        img = "http://src.sencha.io/80/80/" + imageMatch.Groups[1].Value;

                    //TODO: Optimize for screen below 720p
                    postBody = postBody.Replace(imageMatch.Groups[1].Value, "http://src.sencha.io/" + 1280 + "/" + imageMatch.Groups[1].Value);
                }

                var text = Regex.Replace(postBody, "(<([^>]+)>)", " ");
                if (text.Length > 140)
                {
                    text = text.Substring(0, 140);
                    text += "...";
                }

                var userSub = match.Groups[5].Value.Split(new[] { "</a> в " }, StringSplitOptions.RemoveEmptyEntries);
                var sub = userSub.Length > 1 ? Regex.Replace(userSub[1], "(<([^>]+)>)", "") : "";

                var user = userSub[0];

                var vote = 0;
                if (match.Groups[12].Success && match.Groups[12].Value.Length > 0)
                    vote = 1;
                else if (match.Groups[13].Success && match.Groups[13].Value.Length > 0)
                    vote = -1;

                var post = new PostModel();
                post.Id = match.Groups[1].Value.Replace("p", "");
                post.Body = postBody;
                post.Rating = match.Groups[11].Value;
                post.Author = new UserModel {Username = user, Gender = match.Groups[3].Value == "Написал" ? UserGender.Male : UserGender.Female, CustomRank = match.Groups[4].Value};
                post.Type = match.Groups[8].Value;
                post.Url = sub;
                post.Date = match.Groups[6].Value;
                post.Time = match.Groups[7].Value;

                var comments = Regex.Replace(match.Groups[9].Value, "(<([^>]+)>)", "");
                comments = Regex.Replace(comments, "коммента.+?(\\s|$)", "");
                comments = Regex.Replace(comments, " нов.+", "");

                var commentsSplit = comments.Split('/');
                post.TotalCommentsCount = commentsSplit[0].Trim();
                if (commentsSplit.Length == 2)
                    post.UnreadCommentsCount = commentsSplit[1].Trim();
                
                post.Image = img;
                post.Text = text;
                post.Vote = vote;

                posts.Add(post);
            }

            return posts;
        }

        public static MainPageModel ParseMainPage(String htmlData)
        {
            var mainPageModel = new MainPageModel();

            htmlData = CleanupHtml(htmlData);

            mainPageModel.PostVoteCode = Regex.Match(htmlData, "wtf_vote = '(.+?)'").Groups[1].Value;
            mainPageModel.MyThingsHandlerCode = Regex.Match(htmlData, "mythingsHandler.wtf = '(.+?)'").Groups[1].Value;
            mainPageModel.ChatHandlerCode = Regex.Match(htmlData, "chatHandler.wtf = '(.+?)'").Groups[1].Value;
            mainPageModel.Username = Regex.Match(htmlData, "<div id=\"greetings\" class=\"columns_wrapper\">.+?<a href=\".+?\">(.+?)</a>").Groups[1].Value;
            mainPageModel.LogoutCode = Regex.Match(htmlData, "name=\"wtf\" value=\"(.+?)\"").Groups[1].Value;

            //if username is not found then htmldata is not actualy a main page (probably login failed and this is a login page)
            if (String.IsNullOrEmpty(mainPageModel.Username))
                return null;

            // sub lepra regex
            var subReg =
                "<div class=\"sub\"><strong class=\"logo\"><a href=\"(.+?)\" title=\"(.*?)\"><img src=\"(.+?)\" alt=\".+?\" />.+?<div class=\"creator\">.+?<a href=\".*?/users/.+?\">(.+?)</a>";

            var subLepraMatches = Regex.Matches(htmlData, subReg);
            mainPageModel.MySubLepras = new List<SubLepraModel>();

            foreach (Match match in subLepraMatches)
            {
                var subLepra = new SubLepraModel();
                subLepra.Name = match.Groups[2].Value;
                subLepra.Creator = match.Groups[4].Value;
                subLepra.Link = match.Groups[1].Value;
                subLepra.Logo = match.Groups[3].Value;

                mainPageModel.MySubLepras.Add(subLepra);
            }

            return mainPageModel;
        }

        public static LoginPageModel ParseLoginPage(String htmlData)
        {
            htmlData = CleanupHtml(htmlData);

            var loginInfoRegex = "<img alt=\"captcha\" src=\"(.+?)\" width=\"250\" height=\"60\" />.+?<input type=\"hidden\" name=\"logincode\" value=\"(.+?)\" />";
            var loginInfoMatch = Regex.Match(htmlData, loginInfoRegex);

            var loginPageModel = new LoginPageModel {CaptchaImageUrl = loginInfoMatch.Groups[1].Value, LoginCode = loginInfoMatch.Groups[2].Value};

            return loginPageModel;
        }

        public static List<SubLepraModel> ParseUnderground(String htmlData)
        {
            htmlData = CleanupHtml(htmlData);

            var subRegex = "<strong class=\"jj_logo\"><a href=\"(.+?)\"><img src=\"(.+?)\" alt=\"(.*?)\" />.+?<a href=\".*?/users/.+?\">(.+?)</a>";

            var underground = new List<SubLepraModel>();
            var matches = Regex.Matches(htmlData, subRegex);
            foreach (Match match in matches)
            {
                var subLepra = new SubLepraModel();
                subLepra.Name = match.Groups[3].Success ? match.Groups[3].Value : match.Groups[1].Value;
                subLepra.Creator = match.Groups[4].Value;
                subLepra.Link = match.Groups[1].Value;
                subLepra.Logo = match.Groups[2].Value;

                underground.Add(subLepra);
            }

            return underground;
        }

        public static DemocracyPageModel ParseDemocracyPage(String htmlData)
        {
            htmlData = CleanupHtml(htmlData);

            var democracyMatch = Regex.Match(htmlData, "<div id=\"president\"><a href=\".+?\">(.+?)</a><p>.+?<br />(.+?)</p></div>");

            var model = new DemocracyPageModel();
            model.President = democracyMatch.Groups[1].Value;
            model.Term = democracyMatch.Groups[2].Value;

            return model;
        }

        private static string CleanupHtml(string htmlData)
        {
            htmlData = Regex.Replace(htmlData, "\n+", "");
            htmlData = Regex.Replace(htmlData, "\r+", "");
            htmlData = Regex.Replace(htmlData, "\t+", "");
            return htmlData;
        }

        public static string ExtractLoginError(String htmlData)
        {
            htmlData = CleanupHtml(htmlData);

            var errorRegex = "<div class=\"error\">(.+?)</div>";
            var errorMatch = Regex.Match(htmlData, errorRegex);
            return errorMatch.Groups[1].Value;
        }
    }
}
