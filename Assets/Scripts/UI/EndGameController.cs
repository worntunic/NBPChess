using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NBPChess.UI
{
    public class EndGameController : MonoBehaviour
    {
        public Image winPrefab;
        public int fxCount = 10;
        public float minSize, maxSize;
        public float minColor, maxColor;
        public Transform FXRoot;
        public GameObject victoryPanel;
        public Text victoryText;

        public void OpenTieScreen()
        {
            victoryText.text = "tie!";
            victoryPanel.SetActive(true);
        }

        public void OpenVictoryScreen(PieceColor victoryColor)
        {

            string victoryString = (victoryColor + " won!");
            if (victoryColor == PieceColor.White)
            {
                victoryString = victoryString.ToLower();

            } else
            {
                victoryString = victoryString.ToUpper();
            }
            victoryText.text = victoryString;
            CreateVictoryEffects();
            victoryPanel.SetActive(true);
        }

        private void CreateVictoryEffects()
        {
            for (int i = 0; i < fxCount; i++)
            {
                Image current = Instantiate(winPrefab, FXRoot);
                float xOffset = Random.Range(0f, 1f);
                float yOffset = Random.Range(0f, 1f);
                float size = Random.Range(minSize, maxSize);

                current.rectTransform.anchorMin = new Vector2(xOffset, yOffset);
                current.rectTransform.anchorMax = new Vector2(xOffset + size, yOffset + size);

                current.color = GetRandomColor(minColor, maxColor);
                current.gameObject.SetActive(true);
            }
        }

        private Color GetRandomColor(float min, float max)
        {
            float r = Random.Range(min, max);
            float g = Random.Range(min, max);
            float b = Random.Range(min, max);

            return new Color(r, g, b);
        }

    }
}

