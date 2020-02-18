using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess
{
    [System.Serializable]
    public struct TileArtVariant
    {
        public Sprite normalTile;
        public Sprite selectedTile;
        public Sprite hoverTile;
        public Sprite markedSprite;
        public Sprite noEffectSprite;
    }
    [System.Serializable]
    public struct PieceArtVariant
    {
        public readonly PieceType type;
        public Sprite whiteVersion;
        public Sprite blackVersion;

        public PieceArtVariant(PieceType type)
        {
            this.type = type;
            whiteVersion = null;
            blackVersion = null;
        }

        public PieceArtVariant(PieceType type, Sprite white, Sprite black)
        {
            this.type = type;
            whiteVersion = white;
            blackVersion = black;
        }
    }
    [CreateAssetMenu(fileName = "ChessData", menuName = "ChessData/ChessArtSet", order = 1)]
    public class ChessArtSet : ScriptableObject
    {
        [SerializeField]
        public TileArtVariant whiteTile;
        [SerializeField]
        public TileArtVariant blackTile;

        [SerializeField]
        public PieceArtVariant[] sprites = new PieceArtVariant[6] {
            new PieceArtVariant(PieceType.Pawn),
            new PieceArtVariant(PieceType.Rook),
            new PieceArtVariant(PieceType.Knight),
            new PieceArtVariant(PieceType.Bishop),
            new PieceArtVariant(PieceType.Queen),
            new PieceArtVariant(PieceType.King)
        };

        public PieceArtVariant GetSpriteVariants(PieceType type)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i].type == type)
                {
                    return sprites[i];
                }
            }
            throw new System.Exception("Sprite of type: " + type.ToString() + " doesn't exist for set " + name);
        }

        public Sprite GetSprite(PieceType type, PieceColor color)
        {
            PieceArtVariant variants = GetSpriteVariants(type);
            if (color == PieceColor.White)
            {
                return variants.whiteVersion;
            } else
            {
                return variants.blackVersion;
            }
        }

        public TileArtVariant GetTileArt(TileColor color)
        {
            if (color == TileColor.White)
            {
                return whiteTile;
            } else
            {
                return blackTile;
            }
        }
    }
}

