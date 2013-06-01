using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MetroLepra.Model;

namespace MetroLepra.Core
{
    public class HtmlParser
    {
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
