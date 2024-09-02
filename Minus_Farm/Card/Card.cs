using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CW
{

    public class Card : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _descriptionText;

        private Image _currentImage;

        public bool IsCurrentImage = false;

        private CardSO _currentCard;
        public CardSO CurrentCard
        {
            get { return _currentCard; }
            set
            {
                _currentCard = value;

                if (_currentCard == null)
                {
                    _currentImage.sprite = null;
                    _currentImage.color = new Color(1, 1, 1, 0);
                    if (IsCurrentImage == true)
                    {
                        _descriptionText.text = "";
                        GetComponent<Drag>().currentCard = _currentCard;
                    }
                    return;
                }

                _currentImage.color = new Color(1, 1, 1, 1);
                _currentImage.sprite = _currentCard.sprite;

                if (IsCurrentImage == true)
                {
                    _descriptionText.text = _currentCard.description;
                    GetComponent<Drag>().currentCard = _currentCard;
                }
            }
        }

        private void Awake()
        {
            _currentImage = GetComponent<Image>();
        }

        public void SetDescription(string text)
        {
            _descriptionText.text = text;
        }

    }

}