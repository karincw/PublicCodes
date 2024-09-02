using System;
using UnityEngine;

namespace karin
{

    [System.Serializable]
    public struct Weight
    {

        public int meet;     //2  1 = 2
        public int vegetable;//1  1
        public int hot;      //1  1
        public int ice;      //1  1
        public int soft;     //1  1
        public int hard;     //1  1
        public int spice;    //-1  10 = -10


        public Weight(int meet = 0, int vegetable = 0, int hot = 0, int ice = 0, int soft = 0, int hard = 0, int spice = 0)
        {
            this.meet = meet;
            this.vegetable = vegetable;
            this.hot = hot;
            this.ice = ice;
            this.soft = soft;
            this.hard = hard;
            this.spice = spice;
        }
    }

    [System.Serializable]
    public struct InvenData
    {
        public ItemNames itemName;
        public Sprite image;
        public int count;

        public InvenData(ItemNames _itemNames, Sprite _sprite, int _count)
        {
            itemName = _itemNames;
            image = _sprite;
            count = _count;
        }
    }
}

