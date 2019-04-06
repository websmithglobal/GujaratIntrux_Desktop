using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

public static class ERRORREPORTING
{
    public static string Report(Exception ex, string url, Guid userid,string projectkey,string requestdata)
    {
        String URL = "http://errorreporting.appsmith.co.in/Error/Notify";
        WebRequest request = WebRequest.Create(URL);
        request.Method = "POST";
        request.ContentType = "application/json";

        var values = new Dictionary<string, string>
            {
               { "errorcode", DateTime.Now.Ticks.ToString() },
               { "errortext", ex.Message.ToString() },
               { "errorsource",ex.Source },
               { "errorstacktrace", ex.StackTrace },
               { "errorfileurl", url },
               { "requestdata", requestdata },
               { "CreatedBy", userid.ToString() },
               { "projectkey", projectkey.ToString() },
            };

        string jsonFormat = Newtonsoft.Json.JsonConvert.SerializeObject(values);

        Byte[] data = Encoding.UTF8.GetBytes(jsonFormat);

        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }
        WebResponse response = request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return reader.ReadLine();
    }
}