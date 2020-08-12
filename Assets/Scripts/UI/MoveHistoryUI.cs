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
        private bool initialized = false;
        public void Initialize(MoveManager moveManager)
        {
            if (initialized)
            {
                foreach (MoveHistoryEntryUI entry in entries)
                {
                    Destroy(entry.gameObject);
                }
            }
            entries.Clear();
            this.moveManager = moveManager;
            moveManager.beforeMovePlayed += OnMovePlayed;
            initialized = true;
        }

        private void OnMovePlayed(ChessMove move, string moveString, bool newMove)
        {
            int numberOfMoves = moveManager.GetNumberOfMoves();
            /*for(int i = 0; i < entries.Count; i++)
            {
                entries[i].MoveByWidth(entries.Count - i);
            }*/
            MoveHistoryEntryUI entry = Instantiate(sampleEntry, contentArea);
            //entry.transform.SetAsFirstSibling();
            entry.Initialize(numberOfMoves, moveString, move.activePiece.GetColor());
            RectTransform contentParent = (RectTransform)contentArea.transform;
            float contentAreaHeight = entry.height * numberOfMoves;
            contentAreaHeight = Mathf.Max(contentAreaHeight, contentParent.rect.height);
            contentArea.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, contentAreaHeight);
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
