using DG.Tweening;
using Karin;
using Karin.AStar;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tkfkadlsi;
using UnityEngine;

namespace Karin
{
    public class PSpawner : MonoBehaviour, IStructure
    {
        public BuildingBuild StructureData { get => structureData; set => structureData = value; }
        [SerializeField] private BuildingBuild structureData;
        public float CurrentHp { get; set; } = 1;

        [SerializeField] private float summonSpeed = 1;
        private int Cost = 0;
        public Sprite defaultImage;

        public float SetHP = 0;

        [SerializeField] private GameObject summonPrefabs;
        public GameObject SummonPrefabs
        {
            get { return summonPrefabs; }
            set
            {
                summonPrefabs = value;
                if (value != null)
                {

                    if (value.TryGetComponent<ControlableCharacter>(out var agent))
                    {
                        viewer.sprite = agent._characterData.Image;
                        Cost = agent._characterData.WoodCost;
                    }
                }
                else
                {
                    Cost = 0;
                    viewer.sprite = defaultImage;
                }
            }
        }
        public GameObject MygameObject { get; set; }
        CollisionObject colObj;

        private Gauge myGauge;

        [SerializeField] private bool EnemySummoner = false;
        [SerializeField] public int Count;

        [SerializeField] SpriteRenderer viewer;

        private void Awake()
        {
            MygameObject = gameObject;
            if (structureData != null)
            {
                CurrentHp = StructureData.maxHealth;
            }
            myGauge = GetComponentInChildren<Gauge>();
            colObj = GetComponent<CollisionObject>();
        }

        private void OnEnable()
        {
            myGauge.GaugeEnd += Summon;
        }

        private void OnDisable()
        {
            myGauge.GaugeEnd -= Summon;
        }

        private void Start()
        {
            StartCoroutine("SummonCorutine");
        }
        private void Update()
        {
            if (CurrentHp <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        private IEnumerator SummonCorutine()
        {
            while (true)
            {
                yield return null;
                myGauge.GaugeDecrease(summonSpeed * Time.deltaTime);
            }
        }

        private void Summon()
        {
            if (CanSummon(out var pos))
            {
                if (EnemySummoner == true)
                {
                    if (Count <= 0)
                    {
                        return;
                    }
                    Count--;
                }
                if (SummonPrefabs != null && ResourceManager.Instance.BranchCount >= Cost)
                {
                    ResourceManager.Instance.BranchCount -= Cost;
                    var obj = Instantiate(SummonPrefabs, MapManager.Instance.GetWorldPos(pos), Quaternion.identity);
                    if (EnemySummoner == true)
                        obj.GetComponent<EnemyAgent>().CurrentHP = SetHP;
                }
            }
        }

        private bool CanSummon(out Vector3Int movePos)
        {
            var pos = MapManager.Instance.GetTilePos(transform.position);

            foreach (var dir in Direction.Directions)
            {
                if (MapManager.Instance.collisionMap.GetTile(pos + (Vector3Int)dir) == null)
                {
                    movePos = pos + (Vector3Int)dir;
                    return true;
                }
            }
            movePos = Vector3Int.zero;
            return false;
        }
    }


}
