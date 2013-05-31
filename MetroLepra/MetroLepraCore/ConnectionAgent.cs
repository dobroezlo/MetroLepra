using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MetroLepra.Core
{
    public class ConnectionAgent
    {
        private const string AuthCookiesRegex = "lepro.sid=(.+?);.+?lepro.uid=(.+?);";
        private HttpClient _client;
        private static ConnectionAgent _instance;

        /// <summary>
        ///     Prevents a default instance of the <see cref="ConnectionAgent" /> class from being created.
        /// </summary>
        private ConnectionAgent()
        {
            _client = new HttpClient();
        }

        /// <summary>
        ///     Gets the current connection agent instance
        /// </summary>
        public static ConnectionAgent Current
        {
            get { return _instance ?? (_instance = new ConnectionAgent()); }
        }

        public async Task<String> GetMainPage()
        {
            var response = await _client.GetAsync("http://leprosorium.ru/");
            if (!response.IsSuccessStatusCode)
                return null;

            var loginPageHtmlData = await response.Content.ReadAsStringAsync();
            return loginPageHtmlData;
        }

        public async Task<String> GetMainPage(string sessionId, string userId)
        {
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(new Uri("http://leprosorium.ru/"), new Cookie("lepro.sid", sessionId));
            cookieContainer.Add(new Uri("http://leprosorium.ru/"), new Cookie("lepro.uid", userId));

            var handler = new HttpClientHandler {CookieContainer = cookieContainer};
            _client = new HttpClient(handler);

            return await GetMainPage();
        }

        public async Task<Stream> GetImageStream(String path)
        {
            var response = await _client.GetAsync(path);
            if (!response.IsSuccessStatusCode)
                return null;

            var imageStream = await response.Content.ReadAsStreamAsync();
            return imageStream;
        }

        /// <summary>
        /// Attempts to login user with specified credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="captcha"></param>
        /// <param name="loginCode"></param>
        /// <returns>Tuple structure with login information. Item1 = sessionId, Item2 = userId, Item3 = login response content (populated when login failed)</returns>
        public async Task<Tuple<String, String, String>> Login(String username, String password, String captcha, String loginCode)
        {
            var postString = String.Format("user={0}&pass={1}&captcha={2}&logincode={3}&save=1", username, password, captcha, loginCode);
            var byteArray = Encoding.UTF8.GetBytes(postString);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://leprosorium.ru/login/");
            httpWebRequest.AllowAutoRedirect = false;
            httpWebRequest.Method = HttpMethod.Post.Method;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = byteArray.Length;

            var requestStream = await httpWebRequest.GetRequestStreamAsync();
            requestStream.Write(byteArray, 0, byteArray.Length);

            var response = await httpWebRequest.GetResponseAsync();
            var setCookie = response.Headers["Set-Cookie"];

            var authCookies = ExtractAuthCookies(setCookie);
            if (authCookies == null)
            {
                using (var responseContentStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseContentStream);
                    var responseContent = await reader.ReadToEndAsync();
                    return new Tuple<string, string, string>(null, null, responseContent);
                }
            }

            return new Tuple<string, string, string>(authCookies.Item1, authCookies.Item2, null);
        }

        private Tuple<String, String> ExtractAuthCookies(string setCookie)
        {
            var authCookiesMatch = Regex.Match(setCookie, AuthCookiesRegex);
            if (!authCookiesMatch.Groups[1].Success || !authCookiesMatch.Groups[2].Success)
                return null;

            var sessionId = authCookiesMatch.Groups[1].Value;
            var userId = authCookiesMatch.Groups[2].Value;

            return new Tuple<string, string>(sessionId, userId);
        }
    }

    public static class HttpExtensions
    {
        public static Task<Stream> GetRequestStreamAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<Stream>();
            request.BeginGetRequestStream(ar =>
                                              {
                                                  var requestStream = request.EndGetRequestStream(ar);
                                                  taskComplete.TrySetResult(requestStream);
                                              }, request);
            return taskComplete.Task;
        }

        public static Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<HttpWebResponse>();
            request.BeginGetResponse(asyncResponse =>
                                         {
                                             try
                                             {
                                                 var responseRequest = (HttpWebRequest) asyncResponse.AsyncState;
                                                 var someResponse = (HttpWebResponse) responseRequest.EndGetResponse(asyncResponse);
                                                 taskComplete.TrySetResult(someResponse);
                                             }
                                             catch (WebException webExc)
                                             {
                                                 var failedResponse = (HttpWebResponse) webExc.Response;
                                                 taskComplete.TrySetResult(failedResponse);
                                             }
                                         }, request);
            return taskComplete.Task;
        }
    }
}