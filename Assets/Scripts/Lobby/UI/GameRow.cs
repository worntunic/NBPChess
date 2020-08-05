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
        private GameTable gameTable;
        private PlayerGameInfo gameInfo;
        [Header("Components")]
        public Image background;
        public Text username;
        public Text rank;
        public Text time;
        public Text status;
        [Header("Visual options")]
        public Color evenRowBkg; 
        public Color oddRowBkg, hoverRowBkg, selectRowBkg;
        private Color normalRowColor;
        private bool selected = false;


        public void Setup(bool evenRow, PlayerGameInfo gameInfo, GameTable gameTable)
        {
            this.gameTable = gameTable;
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

        public void SelectRow()
        {
            this.background.color = selectRowBkg;
            selected = true;
        }
        public void DeselectRow()
        {
            this.background.color = normalRowColor;
            selected = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            gameTable.SelectRow(gameInfo, this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!selected)
            {
                this.background.color = hoverRowBkg;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!selected)
            {
                this.background.color = normalRowColor;
            }
        }
    }
}

