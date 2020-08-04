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
        [Header("Lobby Controllers")]
        public GameTable gameTable;
        [Header("Web")]
        private PlayerRequest playerRequest;
        private bool playerRequestInProgress = false;
        private PlayerData currentPlayerData;


        public void Awake()
        {
            playerRequest = new PlayerRequest();
            ActivateLoginScreen();
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
            //Lobby Sub Web
            playerRequest.onActiveGamesGot -= OnActiveGamesGot;
        }

        public void OnLoginClicked()
        {
            SetRequestInProgress(true);
            playerRequest.LoginPlayer(usernameInputField.text, passwordInputField.text, this);
        }
        public void OnLogoutClicked()
        {
            ActivateLoginScreen();
        }

        public void OnRegisterClicked()
        {
            SetRequestInProgress(true);
            playerRequest.RegisterPlayer(usernameInputField.text, passwordInputField.text, this);
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
            SetRequestInProgress(false);
            currentPlayerData = lrData.playerData;
            ActivateLobbyScreen();
        }
        private void OnActiveGamesGot(FullPlayerData data)
        {
            gameTable.PopulateRows(data.activeGames);
        }
        private void SetRequestInProgress(bool requestInProgress)
        {
            playerRequestInProgress = requestInProgress;
            requestActiveOverlay.SetActive(requestInProgress);
            loginButton.interactable =
            registerButton.interactable =
            usernameInputField.interactable =
            passwordInputField.interactable =
                !requestInProgress;
        }

        private void OnError(string errorMessage, string additionalData)
        {
            SetRequestInProgress(false);
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

