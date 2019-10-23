using Microsoft.SqlServer.Server;
using RestSharp;
using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Text;

namespace REST
{
    public class Http
    {
        [SqlFunction(FillRowMethodName = "GetData", SystemDataAccess = SystemDataAccessKind.Read, TableDefinition = "Status nvarchar(max), Content nvarchar(max)")]
        public static IEnumerator GetHtml(
          string ResourceEncoding,
          string BaseUrl,
          string Resource)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri(BaseUrl);
            RestRequest request = new RestRequest(Resource);
            IRestResponse response = client.Get((IRestRequest)request);
            Encoding encoding = Encoding.GetEncoding(ResourceEncoding);
            string http_string = encoding.GetString(response.RawBytes);
            string http_status = response.ResponseStatus.ToString();
            yield return (object)new ArrayList()
      {
        (object) http_status,
        (object) http_string
      };
        }

        public static void GetData(object Values, out SqlString Status, out SqlString Content)
        {
            ArrayList arrayList = (ArrayList)Values;
            Status = (SqlString)arrayList[0].ToString();
            Content = (SqlString)arrayList[1].ToString();
        }
    }
}
