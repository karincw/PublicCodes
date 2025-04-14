using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tkfkadlsi;
using UnityEngine;

namespace Karin

{
    public class Totem_Attack : MonoBehaviour, IDetecterUser, IStructure
    {
        public float CurrentHp { get; set; } = 0;
        public float MaxHP { get; set; } = 0;

        public BuildingBuild StructureData { get => structureData; set => structureData = value; }
        [SerializeField] private BuildingBuild structureData;
        private List<GameObject> detectedObject = new List<GameObject>();
        private List<ControlableCharacter> controlableAgent => detectedObject.Select(t => t.GetComponent<ControlableCharacter>()).ToList();
        private List<ControlableCharacter> bonusAgent = new List<ControlableCharacter>();
        public List<GameObject> DetectedObject
        {
            get => detectedObject;
            set
            {
                detectedObject = value;
                bonusAgent.ForEach(t => t.AttackBonus = 1);
                bonusAgent.Clear();

                if (controlableAgent != null && controlableAgent.Count > 0)
                {
                    foreach (var t in controlableAgent)
                    {
                        if (t == null) continue;
                        t.AttackBonus = 1.5f;
                        bonusAgent.Add(t);
                    }
                }
            }
        }
        public GameObject MygameObject { get; set; }

        private void Awake()
        {
            MaxHP = structureData.maxHealth;
            MygameObject = gameObject;
            CurrentHp = StructureData.maxHealth;
        }

        private void Update()
        {
            if (CurrentHp <= 0)
            {
                Destroy(this.gameObject);
            }
        }

    }
}
