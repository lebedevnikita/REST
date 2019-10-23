using Microsoft.SqlServer.Server;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Net;
using System.Text;

namespace REST
{
    public class Soap
    {
        [SqlFunction(FillRowMethodName = "GetData", SystemDataAccess = SystemDataAccessKind.Read, TableDefinition = "Status nvarchar(max), Content nvarchar(max)")]
        public static IEnumerator GetResponseSoap(
          string ResourceEncoding,
          string url,
          string ContentType,
          string method,
          string soapEnvelope)
        {
            url = url.Trim('/').Trim('\\');
            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(url);
            _request.Method = "POST";
            _request.ContentType = ContentType;
            _request.Headers.Add("SOAPAction", method);
            StreamWriter _streamWriter = new StreamWriter(_request.GetRequestStream());
            _streamWriter.Write(soapEnvelope);
            _streamWriter.Close();
            HttpWebResponse _response = (HttpWebResponse)_request.GetResponse();
            StreamReader _streamReader = new StreamReader(_response.GetResponseStream());
            Encoding encoding = Encoding.GetEncoding(ResourceEncoding);
            byte[] _result = encoding.GetBytes(_streamReader.ReadToEnd());
            string http_string = encoding.GetString(_result);
            string http_status = _response.StatusDescription;
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
