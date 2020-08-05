using NBPChess.Web;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NBPChess.Lobby.UI
{
    public class WebUIController : MonoBehaviour
    {
        [Header("Containers")]
        public GameObject LoginScreen;
        public GameObject LobbyScreen;
        [Header("Login UI Elements")]
        public Button loginButton;
        public Button registerButton;
        public GameObject requestActiveOverlay;
        public SingleNotification notification;
        public InputField usernameInputField, passwordInputField;
        [Header("Lobby UI Elements")]
        public Text playerNameText;
        public Text playerRankText;
        public Button logoutButton;
        public Button findGameButton;
        public Button joinGameButton;
        public Text joinGameButtonText;
        public DeselectZone deselectZone;
        [Header("Lobby Controllers")]
        public GameTable gameTable;
        [Header("Web")]
        private PlayerRequest playerRequest;
        private bool playerRequestInProgress = false;
        private PlayerData currentPlayerData;
        [Header("General Visual Options")]
        public Color normalTextColor, disabledTextColor;
        //State
        private PlayerGameInfo curSelectedGame;
        private GameRow curSelectedRow;
        private bool isCurSelected;

        public void Awake()
        {
            playerRequest = new PlayerRequest();
            ActivateLoginScreen();
            OnDeselect();
        }
        private void OnEnable()
        {
            //Login Sub UI
            loginButton.onClick.AddListener(OnLoginClicked);
            registerButton.onClick.AddListener(OnRegisterClicked);
            //Login Sub Web
            playerRequest.onLoginFinished += OnLoginFinished;
            playerRequest.onRegisterFinished += OnRegisterFinished;
            playerRequest.onError += OnError;
            //Lobby Sub UI
            logoutButton.onClick.AddListener(OnLogoutClicked);
            findGameButton.onClick.AddListener(OnFindGameClicked);
            gameTable.onGameSelected += OnGameSelected;
            joinGameButton.onClick.AddListener(OnJoinGameClicked);
            deselectZone.onDeselectClicked += OnDeselect;
            //Lobby Sub Web
            playerRequest.onActiveGamesGot += OnActiveGamesGot;

        }
        private void OnDisable()
        {
            //Login Sub UI
            loginButton.onClick.RemoveListener(OnLoginClicked);
            registerButton.onClick.RemoveListener(OnRegisterClicked);
            //Sub Web
            playerRequest.onLoginFinished -= OnLoginFinished;
            playerRequest.onRegisterFinished -= OnRegisterFinished;
            playerRequest.onError -= OnError;
            //Lobby Sub UI
            logoutButton.onClick.RemoveListener(OnLogoutClicked);
            findGameButton.onClick.RemoveListener(OnFindGameClicked);
            gameTable.onGameSelected -= OnGameSelected;
            joinGameButton.onClick.RemoveListener(OnJoinGameClicked);
            deselectZone.onDeselectClicked -= OnDeselect;


            //Lobby Sub Web
            playerRequest.onActiveGamesGot -= OnActiveGamesGot;
        }
        //UI Events
        public void OnLoginClicked()
        {
            SetLoginRequestInProgress(true);
            playerRequest.LoginPlayer(usernameInputField.text, passwordInputField.text, this);
        }
        public void OnRegisterClicked()
        {
            SetLoginRequestInProgress(true);
            playerRequest.RegisterPlayer(usernameInputField.text, passwordInputField.text, this);
        }
        public void OnLogoutClicked()
        {
            ActivateLoginScreen();
        }
        public void OnFindGameClicked()
        {
            playerRequest.FindGame(this);
            SetLobbyRequestInProgress(true, "Finding game...");
        }
        public void OnGameSelected(PlayerGameInfo gameInfo, GameRow gameRow)
        {
            if(isCurSelected)
            {
                curSelectedRow.DeselectRow();
            }
            curSelectedGame = gameInfo;
            curSelectedRow = gameRow;
            curSelectedRow.SelectRow();
            isCurSelected = true;
            joinGameButton.interactable = true;
            joinGameButtonText.color = normalTextColor;
        }
        public void OnDeselect()
        {
            if (isCurSelected)
            {
                curSelectedRow.DeselectRow();
            }
            isCurSelected = false;
            joinGameButton.interactable = false;
            joinGameButtonText.color = disabledTextColor;
        }
        public void OnJoinGameClicked()
        {
            Debug.Log("Join game!");
        }

        //Event callbacks
        private void OnLoginFinished(LoginResponseData lrData)
        {
            Debug.Log("Success!");
            LoginSuccess(lrData);
        }
        private void OnRegisterFinished(LoginResponseData lrData)
        {
            Debug.Log("Success!");
            LoginSuccess(lrData);
        }
        private void LoginSuccess(LoginResponseData lrData)
        {
            SetLoginRequestInProgress(false);
            currentPlayerData = lrData.playerData;
            ActivateLobbyScreen();
        }
        private void OnActiveGamesGot(FullPlayerData data)
        {
            gameTable.PopulateRows(data.activeGames);
        }
        private void SetLoginRequestInProgress(bool requestInProgress)
        {
            playerRequestInProgress = requestInProgress;
            requestActiveOverlay.SetActive(requestInProgress);
            loginButton.interactable =
            registerButton.interactable =
            usernameInputField.interactable =
            passwordInputField.interactable =
                !requestInProgress;
        }
        private void SetLobbyRequestInProgress(bool requestInProgress, string displayText = "")
        {

        }

        private void OnError(string errorMessage, string additionalData)
        {
            SetLoginRequestInProgress(false);
            SetLobbyRequestInProgress(false);
            string notificationString = errorMessage;
            if (!string.IsNullOrEmpty(additionalData))
            {
                Response<object> res = Response<object>.FromJson(additionalData);
                notificationString += ":\n" + res.message;
            }
            notification.CreateNotification(notificationString);
        }

        private void ActivateLobbyScreen()
        {
            LobbyScreen.SetActive(true);
            LoginScreen.SetActive(false);

            playerNameText.text = currentPlayerData.username;
            playerRankText.text = $"Rank: {currentPlayerData.rank.ToString()}";

            playerRequest.GetActiveGames(this);
        }
        private void ActivateLoginScreen()
        {
            LobbyScreen.SetActive(false);
            LoginScreen.SetActive(true);

            usernameInputField.text = "";
            passwordInputField.text = "";
        }


    }
}

