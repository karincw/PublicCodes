using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CW
{

    public class CropInfoViewer : MonoBehaviour
    {
        private Tilemap _tilemap;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                bool isHit = Physics2D.OverlapCircle(mousePos, 0.1f, targetLayer);
                if (isHit)
                {
                    var tilePos = _tilemap.WorldToCell(mousePos);
                    bool isNull = true;
                    Crop crop = CropManager.Instance.GetPosToCrop(tilePos, ref isNull);
                    if (!isNull)
                    {
                        _descriptionText.text = crop.currentCard.description;
                        _descriptionText.text += $"\nwater : \n{crop.water} / 100\n";
                        _descriptionText.text += $"nutrition : \n{crop.nutrition} / 100";
                    }

                }

            }
        }

    }

}