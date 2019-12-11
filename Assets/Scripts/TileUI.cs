using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NBPChess
{
	public class TileUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		private enum TileSelectionState
		{
			Default, SelectedDirect, SelectedIndirect
		}
		public Sprite defaultBlackSprite, defaultWhiteSprite;
		public Sprite noEffectSprite, hoverSprite, selectedDirectSprite, selectedIndirectSprite;
		public Image tileBkgImage;
		public Image tileSelectionEffectImage;
		public Image tileHoverEffectImage;
		[SerializeField]
		public TileID tileID;
		public TileColor color;
		private bool hoverState = false;
		private TileSelectionState selectedState;
		public RectTransform rectTransform;
		private static float percentStep = 0.125f; 

		public void Initialize(TileID tile)
		{
			tileID = tile;
			int row = tile.RowNum();
			int col = tile.ColNum();
			if ((row + col) % 2 == 0)
			{
				color = TileColor.Black;
				tileBkgImage.sprite = defaultBlackSprite;
			} else
			{
				color = TileColor.White;
				tileBkgImage.sprite = defaultWhiteSprite;
			}
			SetPosition(row, col);
		}

		private void SetPosition(int row, int col)
		{
			rectTransform.anchorMin = new Vector2(col * percentStep, row * percentStep);
			rectTransform.anchorMax = new Vector2((col + 1) * percentStep, (row + 1) * percentStep);
		}

		private void ChangeTileSelectionState(TileSelectionState newState)
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
				tileHoverEffectImage.sprite = hoverSprite;
			} else
			{
				tileHoverEffectImage.sprite = noEffectSprite;
			}

			if (selectedState == TileSelectionState.Default)
			{
				tileSelectionEffectImage.sprite = noEffectSprite;
			} else if (selectedState == TileSelectionState.SelectedDirect)
			{
				tileSelectionEffectImage.sprite = selectedDirectSprite;
			} else if (selectedState == TileSelectionState.SelectedIndirect)
			{
				tileSelectionEffectImage.sprite = selectedIndirectSprite;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			ChangeTileHoverState(true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			ChangeTileHoverState(false);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			ChangeTileSelectionState(TileSelectionState.SelectedDirect);
		}
	}
}