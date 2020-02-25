using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NBPChess
{
	public class TileUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{

        //public Sprite defaultBlackSprite, defaultWhiteSprite;
        //public Sprite noEffectSprite, hoverSprite, selectedDirectSprite, selectedIndirectSprite;
        private TileArtVariant tileArt;
		public Image tileBkgImage;
		public Image tileSelectionEffectImage;
		public Image tileHoverEffectImage;
		[SerializeField]
		public Tile tile;
		private bool hoverState = false;
		private TileSelectionState selectedState;
		public RectTransform rectTransform;
		private static float percentStep = 0.125f;
        private Board board;

        public void Initialize(Board board, Tile tile, TileArtVariant artVariant)
		{
            this.board = board;
			this.tile = tile;
			int row = tile.RowNum();
			int col = tile.ColNum();
            ChangeArt(artVariant);
            SetPosition(row, col);
		}

		private void SetPosition(int row, int col)
		{
			rectTransform.anchorMin = new Vector2(col * percentStep, row * percentStep);
			rectTransform.anchorMax = new Vector2((col + 1) * percentStep, (row + 1) * percentStep);
		}

        public RectTransform GetRectTransform()
        {
            return rectTransform;
        }

		public void ChangeTileSelectionState(TileSelectionState newState)
		{
			selectedState = newState;
			ChangeSprite();
		}
		private void ChangeTileHoverState(bool isBeingHovered)
		{
			hoverState = isBeingHovered;
			ChangeSprite();
		}
		private void ChangeSprite()
		{
			if (hoverState)
			{
				tileHoverEffectImage.sprite = tileArt.hoverTile;
			} else
			{
				tileHoverEffectImage.sprite = tileArt.noEffectSprite;
			}

			if (selectedState == TileSelectionState.Default)
			{
				tileSelectionEffectImage.sprite = tileArt.noEffectSprite;
			} else if (selectedState == TileSelectionState.Selected)
			{
				tileSelectionEffectImage.sprite = tileArt.selectedTile;
			} else if (selectedState == TileSelectionState.Highlighted)
			{
				tileSelectionEffectImage.sprite = tileArt.markedSprite;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			ChangeTileHoverState(true);
            board.SetPointedTile(this);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			ChangeTileHoverState(false);
		}

        public void ChangeArt(TileArtVariant artVariant)
        {
            this.tileArt = artVariant;
            this.tileBkgImage.sprite = tileArt.normalTile;
            ChangeSprite();
        }

        public void DeselectTile()
        {
            ChangeTileSelectionState(TileSelectionState.Default);
        }
	}
}