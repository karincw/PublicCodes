using System;
using UnityEngine;

namespace CW
{
    public class DragAndDropManager : MonoSingleton<DragAndDropManager>
    {
        public DragAndDrop dragObject;
        private SpriteRenderer _spriteRenderer;
        private CardSO _card;
        public CardSO Card { get => _card; }
        public bool CanDrop { get; private set; }
        private AudioSource _audio;

        

        public CardType currentType;

        public void SetCard(CardSO card)
        {
            _card = card;
            currentType = card.cardType;

            if (_card != null)
            {
                _spriteRenderer.sprite = _card.sprite;
                CanDrop = true;
            }
            else
            {
                _spriteRenderer.sprite = null;
                CanDrop = false;
            }
        }

        public void SetImage(Sprite sprite = null)
        {
            currentType = CardType.None;
            CanDrop = sprite != null;

            _spriteRenderer.sprite = sprite;
        }

        private void Awake()
        {
            _spriteRenderer = dragObject.GetComponent<SpriteRenderer>(); 
            _audio = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (dragObject != null)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                dragObject.transform.position = mousePos;
            }
        }

        public void PlaySound()
        {
            _audio.Play();
        }
    }
}