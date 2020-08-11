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
        private bool victoryEffectsActive = false;
        public string localWinMessage = "You won!", localLossMessage = "You lost.";

        public void OpenTieScreen()
        {
            victoryText.text = "tie!";
            victoryPanel.SetActive(true);
        }
        public void CloseVictoryScreen()
        {
            victoryPanel.SetActive(false);
        }
        public void OpenVictoryScreen(bool localPlayerWon)
        {
            string victoryString = (localPlayerWon) ? localWinMessage : localLossMessage;
            victoryText.text = victoryString;
            if (localPlayerWon)
            {
                CreateVictoryEffects();
            } else
            {
                DestroyVictoryEffects();
            }

            victoryPanel.SetActive(true);
        }
        private void DestroyVictoryEffects()
        {
            if (victoryEffectsActive)
            {
                foreach (Transform child in FXRoot)
                {
                    Destroy(child.gameObject);
                }
                victoryEffectsActive = false;
            }
        }
        private void CreateVictoryEffects()
        {
            if (!victoryEffectsActive)
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
                victoryEffectsActive = true;
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

