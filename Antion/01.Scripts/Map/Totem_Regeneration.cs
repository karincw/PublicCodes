using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tkfkadlsi;
using UnityEngine;

namespace Karin

{
    public class Totem_Regeneration : MonoBehaviour, IDetecterUser, IStructure
    {
        public float CurrentHp { get; set; } = 0;

        public BuildingBuild StructureData { get => structureData; set => structureData = value; }
        [SerializeField] private BuildingBuild structureData;
        private List<GameObject> detectedObject = new List<GameObject>();
        private List<ControlableCharacter> controlableAgent => detectedObject.Select(t => t.GetComponentInParent<ControlableCharacter>()).ToList();
        private List<ControlableCharacter> bonusAgent = new List<ControlableCharacter>();

        [SerializeField] private float regenerationDelay = 1;
        [SerializeField] private float regeneration = 1;

        public List<GameObject> DetectedObject
        {
            get => detectedObject;
            set
            {
                detectedObject = value;
                bonusAgent.Clear();

                if (controlableAgent != null && controlableAgent.Count > 0)
                {
                    foreach (var t in controlableAgent)
                    {
                        if (t == null)
                        {
                            continue;
                        }
                        bonusAgent.Add(t);
                    };
                }
            }
        }

        private float timer = 0;

        public GameObject MygameObject { get; set; }

        private void Awake()
        {
            MygameObject = gameObject;
            CurrentHp = StructureData.maxHealth;
        }

        private void Update()
        {
            if (CurrentHp <= 0)
            {
                Destroy(this.gameObject);
            }
            timer += Time.deltaTime;
            if (timer >= regenerationDelay)
            {
                timer = 0;
                if (bonusAgent.Count == 0)
                {
                    return;
                }
                bonusAgent.ForEach(t =>
                {
                    if (t.TryGetComponent<ControlableCharacter>(out var ct))
                    {
                        ct.CurrentHP += regeneration;
                    }
                });
            }
        }
    }
}
