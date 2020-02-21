using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NBPChess
{
    public class PieceUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public PieceType pieceType;
        private Board board;
        public RectTransform rectTransform;
        private Piece piece;
        public Image WhiteSpriteVersion, BlackSpriteVersion;
        private Image currentImage;
        private TileUI currentTileUI;
        private MoveManager moveManager;

        private void ChangeColor()
        {
            bool isWhite = piece.GetColor() == PieceColor.White;
            BlackSpriteVersion.gameObject.SetActive(!isWhite);
            WhiteSpriteVersion.gameObject.SetActive(isWhite);
            currentImage = isWhite ? WhiteSpriteVersion : BlackSpriteVersion;
        }

        public void ChangeArt(PieceArtVariant artVariants)
        {
            BlackSpriteVersion.sprite = artVariants.blackVersion;
            WhiteSpriteVersion.sprite = artVariants.whiteVersion;
        }

        public void Initialize(PieceColor color, Tile tile, Board board, PieceArtVariant spriteVariants, MoveManager moveManager)
        {
            this.moveManager = moveManager;
            this.board = board;
            board.RegisterPiece(this);
            this.piece = PieceFactory.CreatePieceByType(pieceType, color, tile, moveManager);
            ChangeArt(spriteVariants);
            ChangeColor();
            MovePiece(tile);
            this.piece.pieceMoved += OnPieceMoved;
            this.piece.pieceCapturedStateChanged += OnPieceCaptured;
        }

        private void OnPieceCaptured(bool captured)
        {
            this.gameObject.SetActive(!captured);
        }

        private void OnPieceMoved(Tile oldTile, Tile newTile)
        {
            currentTileUI = board.GetTileUI(newTile);
            MovePiece(newTile);
        }

        private void SetPiecePosition(TileUI tileUI)
        {
            currentTileUI = tileUI;
            RectTransform tileRect = currentTileUI.GetRectTransform();
            rectTransform.anchorMin = tileRect.anchorMin;
            rectTransform.anchorMax = tileRect.anchorMax;
            rectTransform.anchoredPosition = Vector2.zero;
        }
        private void MovePiece(Tile tile)
        {
            MovePiece(board.GetTileUI(tile));
        } 
        private void MovePiece(TileUI tileUI)
        {
            SetPiecePosition(tileUI);
        }

        private void MovePieceOutOfAnchor(Vector2 deltaMovement)
        {
            rectTransform.anchoredPosition = rectTransform.anchoredPosition + deltaMovement;
        }

        public Piece GetPiece()
        {
            return piece;
        }

        private TileUI GetCurrentTileUI()
        {
            return board.GetTileUI(piece.GetTile());
        }

        public void OnDrag(PointerEventData eventData)
        {
            MovePieceOutOfAnchor(eventData.delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            board.DeselectAllTiles();
            board.SetRayBlockingForAllPieces(true);
            if (board.IsPositionOnBoard(eventData.position))
            {
                Tile newTile = board.GetCurrentlyPointedTile();
                List<Tile> availableMoves = moveManager.GetAvailableMoves(piece, board);
                if (availableMoves.Contains(newTile))
                {
                    moveManager.DoMove(piece.GetTile(), newTile);
                } else
                {
                    MovePiece(currentTileUI);
                }

            } else
            {
                MovePiece(currentTileUI);
            }

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            board.DeselectAllTiles();
            Tile tile = piece.GetTile();
            List<Tile> availableMoves = piece.AvailableMoves(board);
            board.SelectTile(tile);
            foreach(Tile availableTile in availableMoves)
            {
                board.HighlightTile(availableTile);
            }
            board.SetRayBlockingForAllPieces(false);
        }

        public void SetRayBlocking(bool shouldRayBlock)
        {
            currentImage.raycastTarget = shouldRayBlock;
        }
    }
}

