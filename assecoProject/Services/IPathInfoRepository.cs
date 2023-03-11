using System.Collections;

namespace assecoProject.Services
{
    public interface IPathInfoRepository
    {
        Task<string> GetAccessToken(string clientId, string clientSecret,string authToken);
        HttpClient HeadersForAccessTokenGenerate(string clientId, string clientSecret);

        public Task<string> GetXmlFile(string accessToken,string wadlEndpoint);

        public List<string>  ParseXmlToList(string xmlFile);

    }
}
