using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CW
{

    public enum CardType
    {
        None = 0,
        Seed,
        Item,
        Building,
    }

    public enum ItemType
    {
        None = 0,
        Shovel,
        WateringCan,
        Mnure,
    }

    [CreateAssetMenu(menuName = "CW/SO/CardSO")]
    public class CardSO : ScriptableObject
    {
        public CardType cardType;

        [Header("Seed Settings")]
        public string curName;
        [TextArea(1, 3)] public string description;
        public Sprite sprite;
        public TileBase tileBase;
        public int price;
        public int sellMinPrice;
        public int sellMaxPrice;

        [Header("Item Settings")]
        public ItemType itemType;

        [Header("Item Settings")]
        public int action_water_changeValue;
        public int action_nutrition_changeValue;

        [Header("Building Settings")]
        public TileBase building;
        public int Building_water_changeValue;
        public int Building_Nutrition_changeValue;



    }
}
