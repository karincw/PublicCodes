using HS;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CW
{
    public class DragAndDrop : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _detectRadius;
        [SerializeField] private LayerMask _dropToPlantLayer;
        [SerializeField] private LayerMask _dropToSellLayer;
        [SerializeField] private Tilemap _tileMap;
        [SerializeField] private TileBase _canPlantingTile;
        [HideInInspector] public CropInven _cropInven;
        private UtilityButton _utility;
        CardSO currentCard;

        private void Awake()
        {
            _utility = GameObject.Find("UtilityPanel").GetComponent<UtilityButton>();
        }

        private void Update()
        {
            var DMInstance = DragAndDropManager.Instance;
            currentCard = DMInstance.Card;
            if (DMInstance.CanDrop && Input.GetMouseButtonUp(0))
            {
                switch (DMInstance.currentType)
                {
                    case CardType.None:
                        DropToSelling(_cropInven);
                        break;

                    case CardType.Seed:
                        DropToPlanting();
                        break;

                    case CardType.Item:
                        DropToAction();
                        break;

                    case CardType.Building:
                        DropToBuilding();
                        break;
                }

            }
        }

        private void DropToBuilding()
        {
            if (currentCard.cardType != CardType.Building)
                Debug.LogError("Building");

            DragAndDropManager.Instance.SetImage();

            bool isHit = Physics2D.OverlapCircle(transform.position, _detectRadius, _dropToPlantLayer);
            if (isHit)
            {
                Vector3Int tilePos = _tileMap.WorldToCell(transform.position);

                bool isNull = true;
                Crop crop = CropManager.Instance.GetPosToCrop(tilePos, ref isNull);
                if (!isNull)
                {
                    CropManager.Instance.AddBuilding(tilePos, currentCard);

                    CardManager.Instance.UpdateCard();
                    CropManager.Instance.ActiveCooldown();
                }

            }

        }

        private void DropToAction()
        {
            if (currentCard.cardType != CardType.Item)
                Debug.LogError("Item");

            DragAndDropManager.Instance.SetImage();

            bool isHit = Physics2D.OverlapCircle(transform.position, _detectRadius, _dropToPlantLayer);
            if (isHit)
            {
                Vector3Int tilePos = _tileMap.WorldToCell(transform.position);

                bool isNull = true;
                Crop crop = CropManager.Instance.GetPosToCrop(tilePos, ref isNull);
                if (!isNull)
                {

                    switch (currentCard.itemType)
                    {
                        case ItemType.Shovel:
                            CropManager.Instance.SetGroundTile(tilePos);
                            break;

                        case ItemType.WateringCan:
                        case ItemType.Mnure:
                            crop.water += DragAndDropManager.Instance.Card.action_water_changeValue;
                            crop.nutrition += DragAndDropManager.Instance.Card.action_nutrition_changeValue;
                            CropManager.Instance.ChangeCrop(tilePos, crop);
                            break;
                        default:
                            break;
                    }
                    CardManager.Instance.UpdateCard();
                    CropManager.Instance.ActiveCooldown();
                    DragAndDropManager.Instance.PlaySound();

                }

            }

        }

        private void DropToPlanting()
        {
            if (currentCard.cardType != CardType.Seed)
                Debug.LogError("Planting");

            DragAndDropManager.Instance.SetImage();
            bool isHit = Physics2D.OverlapCircle(transform.position, _detectRadius, _dropToPlantLayer);
            if (isHit)
            {
                Vector3Int cellPos = _tileMap.WorldToCell(transform.position);
                if (_tileMap.GetTile(cellPos) != _canPlantingTile) return;

                _tileMap.SetTile(cellPos, currentCard.tileBase);
                CropManager.Instance.AddCrop(cellPos, currentCard);
                CardManager.Instance.UpdateCard();
                CropManager.Instance.ActiveCooldown();
                DragAndDropManager.Instance.PlaySound();

            }
        }

        private void DropToSelling(CropInven cropInven)
        {
            bool isHit = Physics2D.OverlapCircle(transform.position, _detectRadius, _dropToSellLayer);
            if (isHit)
            {
                _utility.SellOpen(cropInven);
            }
            DragAndDropManager.Instance.SetImage();
        }
    }
}