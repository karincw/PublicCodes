using karin;
using UnityEngine;

namespace Karin
{

    [CreateAssetMenu(menuName = "SO/NPCdataSO")]
    public class NPCdataSO : ScriptableObject
    {
        [TextArea(1, 10)] public string text;
        public Sprite characterImage;
        public Weight weight;
        public bool QuestNPC;
        public FoodSO successFood;
    }

}