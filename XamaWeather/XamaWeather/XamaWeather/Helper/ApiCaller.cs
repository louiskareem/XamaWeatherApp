using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace XamaWeather.Helper
{
    public class ApiCaller
    {
        public static async Task<ApiResponse> GetApiResponse(String url, String authID = null)
        {
            using(var client = new HttpClient())
            {
                // If auth id is not null, then assign authorization in header
                if(!String.IsNullOrWhiteSpace(authID))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", authID);
                }

                // If response is good, then get the content, else return an erro message and reason of the error
                var request = await client.GetAsync(url);

                if(request.IsSuccessStatusCode)
                {
                    return new ApiResponse
                    {
                        JsonResponse = await request.Content.ReadAsStringAsync()
                    };
                }
                else
                {
                    return new ApiResponse
                    {
                        ErrorMessage = request.ReasonPhrase
                    };
                }
            }
        }
    }
}
