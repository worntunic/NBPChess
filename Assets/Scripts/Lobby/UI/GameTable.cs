using NBPChess.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NBPChess.Lobby.UI
{
    public class GameTable : MonoBehaviour
    {
        public GameRow sampleRow;
        public GameObject rowParent;
        public float rowHeight = 50;
        public ScrollRect scrollRect;

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
                RectTransform rt = (RectTransform)current.transform;
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, i * rowHeight, rowHeight);
                current.Setup(i % 2 == 0, games[i], this);
            }
            ((RectTransform)rowParent.transform).SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, games.Count * rowHeight);
            scrollRect.verticalNormalizedPosition = 1;
            Canvas.ForceUpdateCanvases();
        }

        public void SelectRow(PlayerGameInfo gameInfo, GameRow gameRow)
        {
            onGameSelected?.Invoke(gameInfo, gameRow);
        }
    }
}

