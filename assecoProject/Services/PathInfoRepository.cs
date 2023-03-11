using Auth0.AuthenticationApi.Models;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Collections;

namespace assecoProject.Services
{
    public class PathInfoRepository : IPathInfoRepository
    {
        public async Task<string> GetAccessToken(string ClientId, string ClientSecret,string authToken)
        {
            AccessTokenResponse token = null;

            try
            {
                HttpClient client = HeadersForAccessTokenGenerate(ClientId,ClientSecret);
                string body = "grant_type=client_credentials&scope=USERAPI";
                client.BaseAddress = new Uri("https://oauth2.assecobs.pl/api/oauth2/token");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                request.Content = new StringContent(body,
                                                    Encoding.UTF8,
                                                    "application/x-www-form-urlencoded");//CONTENT-TYPE header

                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();

                postData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                postData.Add(new KeyValuePair<string, string>("scope", "USERAPI"));

                request.Content = new FormUrlEncodedContent(postData);
                HttpResponseMessage tokenResponse = client.PostAsync(authToken, new FormUrlEncodedContent(postData)).Result;

                //var token = tokenResponse.Content.ReadAsStringAsync().Result;
                token = await tokenResponse.Content.ReadAsAsync<AccessTokenResponse>(new[] { new JsonMediaTypeFormatter() });
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            return token != null ? token.AccessToken : null;
        }

        public async Task<string> GetXmlFile(string accessToken,string wadlEnpoint)
        {
            string getCountryNamesResponse = null;
            try
            {
               

              

                HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = false };
                HttpClient client = new HttpClient(handler);


                try
                {
                    client.BaseAddress = new Uri(wadlEnpoint);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //client.BaseAddress = new Uri(searchUserEndPoint);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Uri.EscapeUriString(client.BaseAddress.ToString()));
                //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage tokenResponse = await client.GetAsync(Uri.EscapeUriString(client.BaseAddress.ToString()));
                if (tokenResponse.IsSuccessStatusCode)
                {
                    getCountryNamesResponse = tokenResponse.Content.ReadAsStringAsync().Result;
                }
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }



            return getCountryNamesResponse; //GetCountryNames
        }

        public HttpClient HeadersForAccessTokenGenerate(string clientId, string clientSecret)
        {
     
            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = false };
            HttpClient client = new HttpClient(handler);
            try
            {
                client.BaseAddress = new Uri("https://oauth2.assecobs.pl/api/oauth2/token");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(
                         System.Text.ASCIIEncoding.ASCII.GetBytes(
                            $"{clientId}:{clientSecret}")));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return client;
        }

       

        public List<string>  ParseXmlToList(string xmlFile)
        {
            XDocument xml;
            xml = XDocument.Parse(xmlFile);


            List<string> lista = xml.Descendants().Where(x => x.Attribute("path") != null && x.Attribute("path").Value != "/").Select(x => x.Attribute("path").Value)
                .ToList();


            return lista;
        }

        
    }
}
