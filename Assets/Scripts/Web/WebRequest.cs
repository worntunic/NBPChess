using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NBPChess.Web
{
    public class WebRequest
    {
        private string urlPrefix = "https://";
        private string url = "localhost";
        private string defaultContentType = "application/json";
        private const string AuthHeaderKey = "Authorization", BearerTokenPrefix = "Bearer ";
        public WebRequest(string url)
        {
            this.url = url;
        }
        public void SetURL(string url)
        {
            this.url = url;
        }
        public IEnumerator SendPost(string urlSuffix, string jsonData, Action<Dictionary<string, string>, string> callback, Action<string, string> errorCallback, string authToken = null)
        {
            using (UnityWebRequest request = UnityWebRequest.Post(urlPrefix + url + urlSuffix, jsonData))
            {
                //request.certificateHandler = new BypassCertificate();
                /*request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Accept", "text/json");*/
                if (!string.IsNullOrEmpty(jsonData))
                {
                    request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
                    request.uploadHandler.contentType = defaultContentType;
                }
                if (!string.IsNullOrEmpty(authToken))
                {
                    request.SetRequestHeader(AuthHeaderKey, BearerTokenPrefix + authToken);
                }
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    //Debug.LogError(request.error);
                    errorCallback?.Invoke(request.error, "");
                }
                if (request.isDone)
                {
                    Debug.Log("Form upload complete!");
                    string responseData = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    if (request.responseCode != 200)
                    {
                        errorCallback?.Invoke(request.error, responseData);
                    } else
                    {
                        callback?.Invoke(request.GetResponseHeaders(), responseData);
                    }
                }

            }     
        }
    }
    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            //Simply return true no matter what
            return true;
        }
    }
}
