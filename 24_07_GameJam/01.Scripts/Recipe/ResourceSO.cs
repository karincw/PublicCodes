using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace karin
{

    [CreateAssetMenu(menuName = "SO/Karin/Item/Resource")]
    public class ResourceSO : ItemSO
    {
        public ResourceSO()
        {
        }

        public ResourceSO(InvenData data) : base(data)
        {
        }
    }

}