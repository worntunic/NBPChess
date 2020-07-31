using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess.Web {
    public struct Response<T>
    {
        public int code;
        public string message;
        public T data;

        public static Response<T> FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Response<T>>(json);
        }
    }

    public struct PlayerData
    {
        public string username;
        public int id;
        public int rank;
    }
    public struct LoginResponseData
    {
        [JsonProperty(PropertyName = "player")]
        public PlayerData playerData;
        public string token;
    }
    public class PlayerRequest
    {
        private struct LoginRequestData
        {           
            public string username;
            public string password;
            public LoginRequestData(string username, string password)
            {
                this.username = username;
                this.password = password;
            }
            public string GetAsJson()
            {
                string json = JsonConvert.SerializeObject(this);
                return json;
            }
        }
        //URL suffices
        private const string rootUrl = "https://localhost:44330";
        private const string registerSuffix = "/api/Player/Register";
        private const string loginSuffix = "/api/Player/Login";
        //
        private WebRequest webRequest;
        //Events
        public event Action<LoginResponseData> onLoginFinished, onRegisterFinished;
        public event Action<string, string> onError;

        public PlayerRequest()
        {
            webRequest = new WebRequest(rootUrl);
        }

        public void RegisterPlayer(string username, string password, MonoBehaviour caller)
        {
            LoginRequestData lData = new LoginRequestData(username, password);
            caller.StartCoroutine(webRequest.SendPost(registerSuffix, lData.GetAsJson(), RegisterFinished, RequestError));
        }
        public void LoginPlayer(string username, string password, MonoBehaviour caller)
        {
            LoginRequestData lData = new LoginRequestData(username, password);
            caller.StartCoroutine(webRequest.SendPost(registerSuffix, lData.GetAsJson(), RegisterFinished, RequestError));
        }

        //Callbacks
        private void RegisterFinished(Dictionary<string, string> headers, string data)
        {
            Response<LoginResponseData> res = Response<LoginResponseData>.FromJson(data); 
            onRegisterFinished?.Invoke(res.data);
        } 
        private void LoginFinished(Dictionary<string, string> headers, string data)
        {
            Response<LoginResponseData> res = Response<LoginResponseData>.FromJson(data);
            onLoginFinished?.Invoke(res.data);
        }
        private void RequestError(string errorMessage, string additionalData)
        {
            onError?.Invoke(errorMessage, additionalData);
        }
    }
}