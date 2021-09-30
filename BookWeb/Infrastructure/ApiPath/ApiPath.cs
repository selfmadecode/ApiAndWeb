using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookWeb.Infrastructure.ApiPath
{
    public static class ApiPath
    {
        public static string ApiBaseUrl = "https://localhost:44360/";
        public static string BookApiUrl = $"{ApiBaseUrl}api/v1/Book";
        public static string AuthorApiUrl = $"{ApiBaseUrl}api/v1/Author";
        public static string PublisherApiUrl = $"{ApiBaseUrl}api/v1/Publisher";
    }
}
