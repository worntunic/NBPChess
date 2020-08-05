using NBPChess.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess.Lobby.UI
{
    public class GameTable : MonoBehaviour
    {
        public GameRow sampleRow;
        public GameObject rowParent;

        public event Action<PlayerGameInfo, GameRow> onGameSelected;

        public void PopulateRows(List<PlayerGameInfo> games)
        {
            foreach (Transform child in rowParent.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < games.Count; ++i)
            {
                GameRow current = GameObject.Instantiate(sampleRow, rowParent.transform);
                current.Setup(i % 2 == 0, games[i], this);
            }
        }

        public void SelectRow(PlayerGameInfo gameInfo, GameRow gameRow)
        {
            onGameSelected?.Invoke(gameInfo, gameRow);
        }
    }
}

