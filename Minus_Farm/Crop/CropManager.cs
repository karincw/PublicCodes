using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CW
{
    public class CropManager : MonoSingleton<CropManager>
    {
        [Header("Settings")]
        [SerializeField] private Tilemap _tileMap;
        [SerializeField] private CardSO _groundSO;
        [SerializeField] private CardSO _roatCropSO;
        public bool nextTurn;

        [SerializeField] private SerializedDictionary<Vector3Int, Crop> tiles = new SerializedDictionary<Vector3Int, Crop>();
        [HideInInspector] public CropUtility cropUtility;

        private float plantingCooldown = 0;
        [SerializeField] private float plantingCooltime = .5f;
        public float Cooltime { get => plantingCooltime; }
        public float Cooldown { get => plantingCooldown; }
        public bool CanPlanting => plantingCooldown <= 0;

        [Header("Values")]
        [SerializeField] private int CropStartWater;
        [SerializeField] private int CropStartNutrition;
        [Space(3)]
        [SerializeField] private int CropDecreaseWater;
        [SerializeField] private int CropDecreaseNutrition;

        private void Awake()
        {
            cropUtility = FindObjectOfType<CropUtility>();
        }

        private void Start()
        {
            StartCoroutine(GrowCoroutine());
            StartSetting();
        }

        private void Update()
        {
            if (plantingCooldown > 0)
            {
                plantingCooldown -= Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                NextCycle();
            }
        }


        public void ActiveCooldown()
        {
            plantingCooldown = plantingCooltime;
        }

        private void StartSetting()
        {
            for (int i = -2; i < 1; i++)
            {
                for (int j = -2; j < 1; j++)
                {
                    Vector3Int cellPos = _tileMap.WorldToCell(new Vector3Int(i, j));

                    _tileMap.SetTile(cellPos, _groundSO.tileBase);
                    CropManager.Instance.AddCrop(cellPos, _groundSO);
                }
            }
        }

        public void AddCrop(Vector3Int pos, CardSO card)
        {
            Crop newCropData = cropUtility.cardToCropDataDic[card];
            Crop newCrop = new Crop(newCropData.growCycle, newCropData.cropTile, newCropData.sprite, card, CropStartWater, CropStartNutrition);

            if (tiles.ContainsKey(pos))
            {
                tiles[pos] = newCrop;
                return;
            }

            tiles.Add(pos, newCrop);
        }

        public void AddBuilding(Vector3Int pos, CardSO card)
        {
            Crop building = new Crop(0, null, null, card);

            if (card.cardType != CardType.Building) return;

            if (tiles.ContainsKey(pos))
            {
                tiles[pos] = building;
                _tileMap.SetTile(pos, building.currentCard.building);
                return;
            }

            tiles.Add(pos, building);
        }

        public void SetGroundTile(Vector3Int tilePos)
        {
            AddCrop(tilePos, _groundSO);
            _tileMap.SetTile(tilePos, _groundSO.tileBase);
        }
        public void SetRoatTile(Vector3Int tilePos)
        {
            AddCrop(tilePos, _roatCropSO);
            _tileMap.SetTile(tilePos, _roatCropSO.tileBase);
        }

        public void ChangeCrop(Vector3Int pos, Crop crop)
        {
            if (tiles.ContainsKey(pos))
            {
                tiles[pos] = crop;
                return;
            }
            Debug.LogError($"Dictionary haven't {pos}this positionKey");
        }

        public void DeleteTile(Vector3Int pos)
        {
            if (tiles.ContainsKey(pos))
            {
                tiles.Remove(pos);
                return;
            }
            Debug.LogError($"Dictionary haven't {pos}this positionKey");
        }

        public Crop GetPosToCrop(Vector3Int pos, ref bool IsNull)
        {
            if (tiles.ContainsKey(pos))
            {
                IsNull = false;
            }
            else
            {
                IsNull = true;
                return new Crop();
            }
            return tiles[pos];
        }

        public void NextCycle()
        {
            nextTurn = true;
        }

        public void Harvest(Vector3Int pos, Crop crop)
        {
            if (tiles.ContainsKey(pos))
            {
                SetGroundTile(pos);
                HarvestManager.Instance.Harvest(pos, crop, 4);
            }
            else
            {
                Debug.LogError($"tiles is Not Have {pos}");
            }
        }

        public IEnumerator GrowCoroutine()
        {
            yield return new WaitUntil(() =>
            {
                if (nextTurn == true)
                {
                    nextTurn = false;
                    return true;
                }
                return false;
            });

            for (int i = 0; i < tiles.Count; i++)
            {
                var targetKey = tiles.Keys.ToList()[i];
                Crop crop = tiles[targetKey];

                if (crop.currentCard == _groundSO) continue; //일반땅이면 나가리

                if (crop.currentCard.cardType == CardType.Building)
                {

                    foreach(var dir in Directions.eightDirections)
                    {
                        Vector3Int tileDir = new Vector3Int((int)dir.x, (int)dir.y);

                        if (!tiles.Keys.Contains(targetKey + tileDir)) continue;

                        Crop dirCrop = tiles[targetKey + tileDir];
                        if (dirCrop.currentCard.cardType == CardType.Seed)
                        {
                            dirCrop.water += crop.currentCard.Building_water_changeValue;
                            dirCrop.nutrition += crop.currentCard.Building_Nutrition_changeValue;

                            tiles[targetKey + tileDir] = dirCrop;
                        }

                    }

                    continue;
                }

                crop.growIdx++;

                crop.water -= CropDecreaseWater;
                crop.nutrition -= CropDecreaseNutrition;

                int cropGrowIdx = crop.growIdx / crop.growCycle;


                if (crop.water <= 0 && crop.nutrition <= 0) //썩으면 나가리
                {
                    //Debug.Log($"{targetKey} : 식물 썩음");
                    SetRoatTile(targetKey);
                }

                if (crop.cropTile.Length - 1 >= cropGrowIdx)//자르는중
                {
                    _tileMap.SetTile(targetKey, crop.cropTile[cropGrowIdx]);
                }
                else //다자랐음
                {
                    Harvest(targetKey, crop);
                    continue;
                }

                tiles[targetKey] = crop;
            }

            StartCoroutine(GrowCoroutine());
        }

        public Vector3Int GetRandomCropPos()
        {
            var keys = tiles.Keys.ToArray();

            int randomIndex = Random.Range(0, keys.Length);

            return keys[randomIndex];
        }
    }


    public static class Directions
    {
        public static Vector2[] eightDirections =
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right,
            Vector2.up + Vector2.right,
            Vector2.up + Vector2.left,
            Vector2.down + Vector2.right,
            Vector2.down + Vector2.left,
        };

    }

}