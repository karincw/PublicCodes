using UnityEngine;
using UnityEngine.Tilemaps;

namespace CW
{
    [System.Serializable]
    public struct Crop
    {
        public int growCycle;
        public int growIdx;
        public TileBase[] cropTile;
        public Sprite sprite;
        public CardSO currentCard;

        public int water;
        public int nutrition;

        public Crop(int growCycle, TileBase[] cropTile, Sprite sprite, CardSO card, int water = 50, int nutrition = 50, int growIdx = 0)
        {
            this.growCycle = growCycle;
            this.cropTile = cropTile;
            this.growIdx = growIdx;
            this.sprite = sprite;
            this.currentCard = card;
            this.water = water;
            this.nutrition = nutrition;
        }
    }
}