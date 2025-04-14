using Karin.AStar;
using System.Collections;
using Tkfkadlsi;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Karin
{
    public class Resource : MonoBehaviour, IStructure
    {

        public BuildingBuild StructureData { get => structureData; set => structureData = value; }
        [SerializeField] private BuildingBuild structureData;
        public float CurrentHp { get; set; } = 1;
        public GameObject MygameObject { get; set; }

        [SerializeField] private ResourceSO resourceData;
        [SerializeField] private TileBase tile;
        private Gauge myGauge;

        [SerializeField] private int maxMinner = 8;
        private int collecterCount = 0;

        [SerializeField] private bool TileCreate = true;

        [SerializeField] private int BreakCount = -10;

        [SerializeField] private bool autoMinning = false;

        private void Awake()
        {
            MygameObject = gameObject;
            myGauge = GetComponentInChildren<Gauge>();
            BreakCount = resourceData.ResourceCount;
            if (structureData != null)
            {
                CurrentHp = StructureData.maxHealth;
            }
        }
        private void Start()
        {
            var currentPos = MapManager.Instance.GetTilePos(transform.position);
            transform.position = MapManager.Instance.GetWorldPos(currentPos);

            if (TileCreate == true)
                MapManager.Instance.collisionMap.SetTile(currentPos, tile);

            if (autoMinning == false)
            {
                StartCoroutine("Mining");
            }
            else
            {
                StartCoroutine("AutoMining");
            }
        }

        private void OnEnable()
        {
            myGauge.GaugeEnd += GaugeEnd;
        }

        private void OnDisable()
        {
            myGauge.GaugeEnd -= GaugeEnd;
        }

        private void Update()
        {
            if (CurrentHp <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        private void GaugeEnd()
        {
            switch (resourceData.ResourceType)
            {
                case ResourceType.Wood:
                    ResourceManager.Instance.BranchCount += 1;
                    Breaking();
                    break;
                case ResourceType.Stone:
                    ResourceManager.Instance.RockCount += 1;
                    Breaking();
                    break;
                default:
                    break;
            }
        }

        private void Breaking()
        {
            if (BreakCount == -10) return;

            if (BreakCount > 0)
            {
                BreakCount--;
            }
            else if (BreakCount == 0)
            {

                if (TileCreate == true)
                {
                    Destroy(this.gameObject);
                }

                var currentPos = MapManager.Instance.GetTilePos(transform.position);
                MapManager.Instance.collisionMap.SetTile(currentPos, null);
            }
        }

        private IEnumerator Mining()
        {
            while (true)
            {
                yield return null;

                if (collecterCount == 0)
                    continue;

                myGauge.GaugeDecrease(resourceData.MiningSpeed * ((float)collecterCount / maxMinner) * Time.deltaTime);

            }
        }
        private IEnumerator AutoMining()
        {
            while (true)
            {
                yield return null;

                myGauge.GaugeDecrease(resourceData.MiningSpeed * Time.deltaTime);

            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<ControlableCharacter>(out var character))
            {
                if (character.type == AntType.Collect)
                    collecterCount++;
            }

        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<ControlableCharacter>(out var character))
            {
                if (character.type == AntType.Collect)
                    collecterCount--;
            }
        }

    }
}
