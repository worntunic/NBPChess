using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess.Web {
    
    public class PlayerRequest
    {
        private struct PlayMoveRequestData
        {
            public int gameid;
            public string move;
            public GameState gamestate;

            public PlayMoveRequestData(int GameID, string move, GameState gamestate)
            {
                this.gameid = GameID;
                this.move = move;
                this.gamestate = gamestate;
            }

            public string GetAsJson()
            {
                string json = JsonConvert.SerializeObject(this);
                return json;
            }
        }
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
        private string rootUrl = "https://localhost";
        private string rootPort = ":44330";
        private const string registerSuffix = "/api/Player/Register";
        private const string loginSuffix = "/api/Player/Login";
        private const string activeGamesSuffix = "/api/Player/ActiveGames";
        private const string findGameSuffix = "/api/Game/Find";
        private const string allGameInfoSuffix = "/api/Game/AllInfo";
        private const string playMoveSuffix = "/api/Game/PlayMove";
        private const string stateChangedSuffix = "/api/Game/WaitForGameState";

        private WebRequest webRequest;
        private LoginResponseData currentLoginData;
        //Events
        public event Action<LoginResponseData> onLoginFinished, onRegisterFinished;
        public event Action<FullPlayerData> onActiveGamesGot;
        public event Action<GameResponse> onFindGame;
        public event Action<GameWithMovesResponse> onGetAllGameInfo;
        public event Action<GameWithMovesResponse> onPlayMoveFinished;
        public event Action<GameWithMovesResponse> onWaitForGameStateFinished;
        public event Action<string, string> onError;

        public PlayerRequest()
        {
            webRequest = new WebRequest(rootUrl);
        }
        public void SetRootURL(string rootURL)
        {
            this.rootUrl = rootURL;
            webRequest.SetURL(GetRootURL());
        }
        public string GetRootWithoutPort()
        {
            return rootUrl;
        }
        private string GetRootURL()
        {
            return rootUrl + rootPort;
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
        public void PlayMove(int gameID, string move, GameState gameState, MonoBehaviour caller)
        {
            PlayMoveRequestData pmrData = new PlayMoveRequestData(gameID, move, gameState);
            caller.StartCoroutine(webRequest.SendPost(playMoveSuffix, pmrData.GetAsJson(), PlayMoveFinished, RequestError, currentLoginData.token));
        }
        public void WaitForGameStateChange(int gameID, MonoBehaviour caller)
        {
            GameInfoRequestData girData = new GameInfoRequestData(gameID);
            caller.StartCoroutine(webRequest.SendPost(stateChangedSuffix, girData.GetAsJson(), WaitForGameStateFinished, RequestError, currentLoginData.token));
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
        //Chess
        private void PlayMoveFinished(Dictionary<string, string> headers, string data)
        {
            Response<GameWithMovesResponse> res = Response<GameWithMovesResponse>.FromJson(data);
            onPlayMoveFinished?.Invoke(res.data);
        }
        private void WaitForGameStateFinished(Dictionary<string, string> headers, string data)
        {
            Response<GameWithMovesResponse> res = Response<GameWithMovesResponse>.FromJson(data);
            onWaitForGameStateFinished?.Invoke(res.data);
        }
        private void RequestError(string errorMessage, string additionalData)
        {
            onError?.Invoke(errorMessage, additionalData);
        }
    }
}