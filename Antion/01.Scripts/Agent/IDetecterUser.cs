using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{
    public interface IDetecterUser
    {
        public List<GameObject> DetectedObject { get; set; }
    }
}
