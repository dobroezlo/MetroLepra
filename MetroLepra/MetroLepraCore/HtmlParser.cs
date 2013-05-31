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

            htmlData = Regex.Replace(htmlData, "\n+", "");
            htmlData = Regex.Replace(htmlData, "\r+", "");
            htmlData = Regex.Replace(htmlData, "\t+", "");

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
    }
}
