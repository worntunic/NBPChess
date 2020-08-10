using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NBPChess.UI
{
    public class MoveHistoryEntryUI : MonoBehaviour
    {
        public Text numberText;
        public Text moveText;
        public Image bkgImg;
        public RectTransform rectTransform;
        public float height;
        public Color whiteBkgColor, blackBkgColor;
        public Color whiteTextColor, blackTextColor;
        private PieceColor pieceColor;

        public void Initialize(int number, string move, PieceColor color)
        {
            numberText.text = number.ToString() + '.';
            moveText.text = move.ToString();
            pieceColor = color;
            if (pieceColor == PieceColor.White)
            {
                bkgImg.color = whiteBkgColor;
                moveText.color = whiteTextColor;
                numberText.color = whiteTextColor;
            } else
            {
                bkgImg.color = blackBkgColor;
                moveText.color = blackTextColor;
                numberText.color = blackTextColor;
            }
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, (number - 1) * height, height);
            gameObject.SetActive(true);
        }
    }
}

