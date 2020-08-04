using NBPChess.Web;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess.Lobby.UI
{
    public class GameTable : MonoBehaviour
    {
        public GameRow sampleRow;
        public GameObject rowParent;

        void Start()
        {

        }

        public void PopulateRows(List<GameInfo> games)
        {
            for (int i = 0; i < games.Count; ++i)
            {
                GameRow current = GameObject.Instantiate(sampleRow, rowParent.transform);
                current.Setup(i % 2 == 0, games[i]);
            }
        }
        void Update()
        {

        }
    }
}

