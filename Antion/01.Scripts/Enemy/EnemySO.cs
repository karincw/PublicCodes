using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{
    [CreateAssetMenu(menuName = "Karin/EnemySO")]
    public class EnemySO : ScriptableObject
    {
        public float MoveSpeed;
        public float atk;
        public float atkspeed;
        public float range;
        public float maxHealth;
    }
}
