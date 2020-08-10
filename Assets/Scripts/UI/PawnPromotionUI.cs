using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NBPChess.UI
{
    [System.Serializable]
    public struct PromotionButtonAndImage
    {
        public Button button;
        public Image image;
    }
    public class PawnPromotionUI : MonoBehaviour
    {
        public PromotionButtonAndImage knight, bishop, rook, queen;
        private MoveManager moveManager;
        private ChessArtSet artSet;
        private PieceColor currentColor = PieceColor.White;
        private bool initialized = false;

        public void Initialize(ChessArtSet artSet, MoveManager moveManager)
        {
            this.moveManager = moveManager;
            moveManager.onPawnPromotionStarted += SetupPawnPromotion;
            ChangeArtSet(artSet);
            if (!initialized)
            {
                RegisterButtonEvents();
            }
            initialized = true;
        }
        private void RegisterButtonEvents()
        {
            knight.button.onClick.AddListener(() => OnPieceSelected(PieceType.Knight));
            bishop.button.onClick.AddListener(() => OnPieceSelected(PieceType.Bishop));
            rook.button.onClick.AddListener(() => OnPieceSelected(PieceType.Rook));
            queen.button.onClick.AddListener(() => OnPieceSelected(PieceType.Queen));
        }
        public void ChangeArtSet(ChessArtSet artSet)
        {
            this.artSet = artSet;
            SetPromotionImages();
        }

        public void SetupPawnPromotion(PieceColor color)
        {
            currentColor = color;
            SetPromotionImages();
            gameObject.SetActive(true);
            Debug.Log("Got activated");
        }

        private void SetPromotionImages()
        {
            knight.image.sprite = artSet.GetSprite(PieceType.Knight, currentColor);
            bishop.image.sprite = artSet.GetSprite(PieceType.Bishop, currentColor);
            rook.image.sprite = artSet.GetSprite(PieceType.Rook, currentColor);
            queen.image.sprite = artSet.GetSprite(PieceType.Queen, currentColor);
        }

        private void OnPieceSelected(PieceType type)
        {
            if(type == PieceType.Pawn || type == PieceType.King)
            {
                throw new System.Exception("Cannot promote to " + type);
            }
            moveManager.PromotionPawnTypeSelected(type);
            gameObject.SetActive(false);
        }
    }
}

