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
        public List<PlayerGameInfo> activeGames;
        public List<PlayerGameInfo> finishedGames;
    }
    public struct PlayerGameInfo
    {
        public int gameID;
        public PlayerData opponent;
        public GameState gameState;
        public int currentTimeLeft;
    }
    //Game Finding
    public struct FullGameResponse
    {
        public GameInfo game;
        public bool gamefound;
    }
    public struct GameInfo
    {
        public int id;
        public FullGameData gamedata;
    }
    public struct FullGameData
    {
        public int wplayer;
        public int bplayer;
        public int wtimeleft;
        public int btimeleft;
        public GameState gamestate;
    }
}