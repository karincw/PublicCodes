using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace karin
{

    [CreateAssetMenu(menuName = "SO/Karin/Item/Food")]
    public class FoodSO : ScriptableObject
    {
        public Food itemName;
        public Weight weight;
        public int price;
        [field: SerializeField] public Sprite image;
    }
}