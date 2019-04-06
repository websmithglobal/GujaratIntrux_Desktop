using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WEB.ConsoleApp.Entity
{
    #region FCM Class
    public class FCMResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<FCMResult> results { get; set; }
    }

    public class FCMResult
    {
        public string message_id { get; set; }
        public string error { get; set; }
    }

    public class Notification
    {
        public Notification()
        {
            this.title = "Gujarat Intrux";
            this.body = "Gujarat Intrux Notification";
            this.sound = "default";
        }

        public string title { get; set; }
        public string body { get; set; }
        public string sound { get; set; }
    }

    public class Data
    {
        public string Description { get; set; }
    }

    public class FCMRootObject
    {
        public string to { get; set; }
        public Notification notification { get; set; }
        public Data data { get; set; }
        public string priority { get; set; }
        public bool content_available { get; set; }


        public FCMResponse SendClientNotification(FCMRootObject dataObject)
        {
            FCMResponse response = new FCMResponse();
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                dataObject.priority = "high";
                dataObject.content_available = true;

                string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(dataObject);

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);

                tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAA5HUiHNg:APA91bFxsl58O6JwOos7lubeXi7-P7VhyFwOrqZEK3CA4qz2cklFjSQyhRwD-CeBPvL6YF23ii7dcCkLKy0F5-YaIhrYbPzLCpv0whwLIPT1L02jC7iioTTWotWVbjHXS7BVo87SRnuP"));
                tRequest.Headers.Add(string.Format("Sender: id={0}", "981217713368"));

                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String responseFromFirebaseServer = tReader.ReadToEnd();

                                response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return response;
        }
    }

    public class FCMSender
    {
        public FCMResponse SendClientNotification(FCMRootObject dataObject)
        {
            FCMResponse response = new FCMResponse();
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                dataObject.priority = "high";
                dataObject.content_available = true;

                string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(dataObject);

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);

                tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAzUlKJsQ:APA91bGszjREoPf3qU-lC1ZpJ6l2adVlFEsF2N26PFIVZ5Mq4lz4JaWrKph55tHBuOV17GBpkAvaBUKba5gpSjU1jvUt4vYj_VjVfuhdVq4edW-4hgwviVO9C0VBDofmU_qm0R2hcxbV"));
                tRequest.Headers.Add(string.Format("Sender: id={0}", "881697892036"));

                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String responseFromFirebaseServer = tReader.ReadToEnd();

                                response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }

    #endregion
}
