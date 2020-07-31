using NBPChess.Web;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NBLD.Lobby.UI
{
    public class WebUIController : MonoBehaviour
    {
        [Header("UI Elements")]
        public Button loginButton;
        public Button registerButton;
        public GameObject requestActiveOverlay;
        public SingleNotification notification;
        public InputField usernameInputField, passwordInputField;
        [Header("Web")]
        private PlayerRequest playerRequest;
        private bool playerRequestInProgress = false;

        public void Awake()
        {
            playerRequest = new PlayerRequest();
        }
        private void OnEnable()
        {
            //Sub UI
            loginButton.onClick.AddListener(OnLoginClicked);
            registerButton.onClick.AddListener(OnRegisterClicked);
            //Sub Web
            playerRequest.onLoginFinished += OnLoginFinished;
            playerRequest.onRegisterFinished += OnRegisterFinished;
            playerRequest.onError += OnError;
        }
        private void OnDisable()
        {
            //Sub UI
            loginButton.onClick.RemoveListener(OnLoginClicked);
            registerButton.onClick.RemoveListener(OnRegisterClicked);
            //Sub Web
            playerRequest.onLoginFinished -= OnLoginFinished;
            playerRequest.onRegisterFinished -= OnRegisterFinished;
            playerRequest.onError -= OnError;

        }

        public void OnLoginClicked()
        {
            SetRequestInProgress(true);
            playerRequest.LoginPlayer(usernameInputField.text, passwordInputField.text, this);
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
            SetRequestInProgress(false);
        }
        private void OnRegisterFinished(LoginResponseData lrData)
        {
            Debug.Log("Success!");
            SetRequestInProgress(false);
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
    }
}

