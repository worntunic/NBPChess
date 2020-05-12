using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NBPChess.UI
{
    public class MoveHistoryUI : MonoBehaviour
    {
        MoveManager moveManager;
        public MoveHistoryEntryUI sampleEntry;
        public RectTransform contentArea;
        public Scrollbar scrollbar;
        public ScrollRect scrollRect;
        private List<MoveHistoryEntryUI> entries = new List<MoveHistoryEntryUI>();

        public void Initialize(MoveManager moveManager)
        {
            this.moveManager = moveManager;
            moveManager.beforeMovePlayed += OnMovePlayed;
        }

        private void OnMovePlayed(ChessMove move)
        {
            string moveString = AlgebraicNotation.ToAlgebraic(move, moveManager);
            int numberOfMoves = moveManager.GetNumberOfMoves();
            /*for(int i = 0; i < entries.Count; i++)
            {
                entries[i].MoveByWidth(entries.Count - i);
            }*/
            MoveHistoryEntryUI entry = Instantiate(sampleEntry, contentArea);
            //entry.transform.SetAsFirstSibling();
            entry.Initialize(numberOfMoves, moveString, move.activePiece.GetColor());
            float contentAreaHeight = entry.height * numberOfMoves;
            if (contentAreaHeight > contentArea.rect.height)
            {
                contentArea.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, entry.height * numberOfMoves);
            }
            scrollRect.verticalNormalizedPosition = 0;
            Canvas.ForceUpdateCanvases();

            scrollbar.value = 0;
            UpdateScroll();
        }

        private void UpdateScroll()
        {

        }
    }

}
