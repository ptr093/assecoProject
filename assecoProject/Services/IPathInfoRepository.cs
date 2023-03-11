using System.Collections;

namespace assecoProject.Services
{
    public interface IPathInfoRepository
    {
        Task<string> GetAccessToken();
        HttpClient HeadersForAccessTokenGenerate();

        public Task<string> GetXmlFile(string accessToken);

        public List<string>  ParseXmlToList(string xmlFile);

    }
}
