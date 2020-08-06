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
            this.piece.pieceEnabledStateChanged += OnPieceEnabledStateChanged;
        }

        private void OnPieceCaptured(bool captured)
        {
            this.gameObject.SetActive(!captured);
        }

        private void OnPieceEnabledStateChanged(bool enabled)
        {
            this.gameObject.SetActive(enabled);
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
            if (!moveManager.CanPieceMove(piece.GetColor()))
            {
                return;
            }
            MovePieceOutOfAnchor(eventData.delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!moveManager.CanPieceMove(piece.GetColor()))
            {
                return;
            }
            board.DeselectAllTiles();
            board.SetRayBlockingForAllPieces(true);
            if (board.IsPositionOnBoard(eventData.position))
            {
                Tile newTile = board.GetCurrentlyPointedTile();
                List<ChessMove> availableMoves = moveManager.AvailableMovesForPiece(piece);
                bool validMove = false;
                for (int i = 0; i < availableMoves.Count; i++)
                {
                    if (availableMoves[i].toTile == newTile)
                    {
                        moveManager.DoMove(availableMoves[i]);
                        validMove = true;
                        break;
                    }
                }
                if (!validMove)
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
            if (!moveManager.CanPieceMove(piece.GetColor()))
            {
                return;
            }
            board.DeselectAllTiles();
            Tile tile = piece.GetTile();
            List<ChessMove> availableMoves = moveManager.AvailableMovesForPiece(piece);
            board.SelectTile(tile);
            foreach(ChessMove move  in availableMoves)
            {
                board.HighlightTile(move.toTile);
            }
            board.SetRayBlockingForAllPieces(false);
        }

        public void SetRayBlocking(bool shouldRayBlock)
        {
            currentImage.raycastTarget = shouldRayBlock;
        }
    }
}

