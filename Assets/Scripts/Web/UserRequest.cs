using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess.Web {
    
    public class PlayerRequest
    {
        private struct GameInfoRequestData
        {
            public int gameid;

            public GameInfoRequestData(int GameID)
            {
                this.gameid = GameID;
            }

            public string GetAsJson()
            {
                string json = JsonConvert.SerializeObject(this);
                return json;
            }
        }
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
        private const string activeGamesSuffix = "/api/Player/ActiveGames";
        private const string findGameSuffix = "/api/Game/Find";
        private const string allGameInfoSuffix = "/api/Game/AllInfo";

        private WebRequest webRequest;
        private LoginResponseData currentLoginData;
        //Events
        public event Action<LoginResponseData> onLoginFinished, onRegisterFinished;
        public event Action<FullPlayerData> onActiveGamesGot;
        public event Action<GameResponse> onFindGame;
        public event Action<GameWithMovesResponse> onGetAllGameInfo;
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
            caller.StartCoroutine(webRequest.SendPost(loginSuffix, lData.GetAsJson(), RegisterFinished, RequestError));
        }
        public void GetActiveGames(MonoBehaviour caller)
        {
            caller.StartCoroutine(webRequest.SendPost(activeGamesSuffix, "", GetActiveGamesFinished, RequestError, currentLoginData.token));
        }
        public void FindGame(MonoBehaviour caller)
        {
            caller.StartCoroutine(webRequest.SendPost(findGameSuffix, "", FindGameFinished, RequestError, currentLoginData.token));
        }
        public void GetAllGameInfo(int gameID, MonoBehaviour caller)
        {
            GameInfoRequestData girData = new GameInfoRequestData(gameID);
            caller.StartCoroutine(webRequest.SendPost(allGameInfoSuffix, girData.GetAsJson(), AllGameInfoFinished, RequestError, currentLoginData.token));
        }

        //Callbacks
        private void RegisterFinished(Dictionary<string, string> headers, string data)
        {
            Response<LoginResponseData> res = Response<LoginResponseData>.FromJson(data);
            currentLoginData = res.data;
            onRegisterFinished?.Invoke(res.data);
        } 
        private void LoginFinished(Dictionary<string, string> headers, string data)
        {
            Response<LoginResponseData> res = Response<LoginResponseData>.FromJson(data);
            currentLoginData = res.data;
            onLoginFinished?.Invoke(res.data);
        }
        private void GetActiveGamesFinished(Dictionary<string, string> headers, string data)
        {
            Response<FullPlayerResponse> res = Response<FullPlayerResponse>.FromJson(data);
            onActiveGamesGot?.Invoke(res.data.player);
        }
        private void FindGameFinished(Dictionary<string, string> headers, string data)
        {
            Response<GameResponse> res = Response<GameResponse>.FromJson(data);
            onFindGame?.Invoke(res.data);
        }
        private void AllGameInfoFinished(Dictionary<string, string> headers, string data)
        {
            Response<GameWithMovesResponse> res = Response<GameWithMovesResponse>.FromJson(data);
            onGetAllGameInfo?.Invoke(res.data);
        }
        private void RequestError(string errorMessage, string additionalData)
        {
            onError?.Invoke(errorMessage, additionalData);
        }
    }
}