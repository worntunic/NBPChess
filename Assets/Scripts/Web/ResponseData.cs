using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess.Web
{
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
    public struct FullPlayerResponse
    {
        public FullPlayerData player;
    }
    public struct FullPlayerData
    {
        public string username;
        public int id;
        public int rank;
        public List<GameInfo> activeGames;
        public List<GameInfo> finishedGames;
    }
    public struct GameInfo
    {
        public int gameID;
        public PlayerData opponent;
        public GameState gameState;
        public int currentTimeLeft;
    }
}