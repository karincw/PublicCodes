using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{
    public enum AntType : ushort
    {
        Collect,
        Attack
    }
    [CreateAssetMenu(menuName = "Karin/CharacterSO")]
    public class CharacterSO : ScriptableObject
    {
        public Sprite Image;
        public float MoveSpeed;
        public float atk;
        public float atkspeed;
        public float range;
        public float maxHealth;

        public AntType TYPE;
        public int WoodCost;
    }
}