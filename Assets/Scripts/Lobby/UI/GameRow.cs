using NBPChess.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NBPChess.Lobby.UI
{
    public class GameRow : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        private GameInfo gameInfo;
        [Header("Components")]
        public Image background;
        public Text username;
        public Text rank;
        public Text time;
        public Text status;
        [Header("Visual options")]
        public Color evenRowBkg, oddRowBkg, hoverRowBkg;
        private Color normalRowColor;

        public void Setup(bool evenRow, GameInfo gameInfo)
        {
            this.gameInfo = gameInfo;
            normalRowColor = (evenRow) ? evenRowBkg : oddRowBkg;
            this.username.text = gameInfo.opponent.username;
            this.rank.text = gameInfo.opponent.rank.ToString();

            int hours = gameInfo.currentTimeLeft / 3600;
            int minutes = gameInfo.currentTimeLeft / 60 - hours * 60;
            int seconds = gameInfo.currentTimeLeft % 60;




            this.time.text = $"{hours}:{minutes}:{seconds}";
            this.status.text = gameInfo.gameState.ToString();
            this.background.color = normalRowColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.background.color = hoverRowBkg;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.background.color = normalRowColor;
        }
    }
}

