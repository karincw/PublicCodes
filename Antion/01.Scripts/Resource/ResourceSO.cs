using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{
    public enum ResourceType : ushort
    {
        Wood,
        Stone, 
    }

    [CreateAssetMenu(menuName = "Karin/ResourceSO")]
    public class ResourceSO : ScriptableObject
    {
        public int ResourceCount;
        public float MiningSpeed;
        public ResourceType ResourceType;
    }
}
