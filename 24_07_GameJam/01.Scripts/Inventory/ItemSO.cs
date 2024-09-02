using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace karin
{

    public class ItemSO : ScriptableObject
    {
        public ItemNames itemName;
        public Sprite image;
        public int count;

        public ItemSO()
        {

        }
        public ItemSO(InvenData data)
        {
            itemName = data.itemName;
            image = data.image;
            count = data.count;
        }

        public static implicit operator ItemSO(InvenData v)
        {
            return new ItemSO(v);
        }
    }
}