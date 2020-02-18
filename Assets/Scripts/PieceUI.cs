using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NBPChess
{
    public class PieceUI : MonoBehaviour
    {
        public PieceType pieceType;
        private Board board;
        public RectTransform rectTransform;
        private Piece piece;
        public Image WhiteSpriteVersion, BlackSpriteVersion;

        private void ChangeColor()
        {
            bool isWhite = piece.GetColor() == PieceColor.White;
            BlackSpriteVersion.gameObject.SetActive(!isWhite);
            WhiteSpriteVersion.gameObject.SetActive(isWhite);
        }

        public void ChangeArt(PieceArtVariant artVariants)
        {
            BlackSpriteVersion.sprite = artVariants.blackVersion;
            WhiteSpriteVersion.sprite = artVariants.whiteVersion;
        }

        public void Initialize(PieceColor color, Tile tile, Board board, PieceArtVariant spriteVariants)
        {
            this.board = board;
            this.piece = PieceFactory.CreatePieceByType(pieceType, color, tile);
            ChangeArt(spriteVariants);
            ChangeColor();
            MovePiece(tile);
            this.piece.pieceMoved += OnPieceMoved;
        }

        private void OnPieceMoved(Tile oldTile, Tile newTile)
        {
            MovePiece(newTile);
        }

        private void MovePiece(Tile tile)
        {
            TileUI tileUI = board.GetTileUI(tile);
            RectTransform tileRect = tileUI.GetRectTransform();
            rectTransform.anchorMin = tileRect.anchorMin;
            rectTransform.anchorMax = tileRect.anchorMax;
        }

        public Piece GetPiece()
        {
            return piece;
        }
    }
}

