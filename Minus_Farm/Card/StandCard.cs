using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CW
{

    public class StandCard : MonoBehaviour
    {
        [SerializeField] Card[] _standImages;
        [SerializeField] TextMeshProUGUI _descriptionText;
        private CardInven _cardInven;

        private void Awake()
        {
            _cardInven = FindObjectOfType<CardInven>();

        }


        private void Update()
        {
            CropManager manager = CropManager.Instance;
            Transform trm = _standImages[0].transform.parent.Find("CooltimeViewer");
            if (manager.CanPlanting == false)
            {
                float current = manager.Cooldown / manager.Cooltime;
                trm.GetComponent<Image>().fillAmount = current;
            }
            else
            {
                trm.GetComponent<Image>().fillAmount = 0;
            }
        }


        [ContextMenu("Stand")]
        public void Stand()
        {
            _cardInven.Suffle();

            int curCardSize = _cardInven.inventory.Count - 1;
            for (int i = 0; i < 6; i++)
            {
                int cur = curCardSize - i;
                CardSO nextCard = _cardInven.inventory[cur];
                _standImages[i].CurrentCard = nextCard;
                _cardInven.inventory.RemoveAt(cur);
            }
        }

        [ContextMenu("Update")]
        public void UpdateCard(bool shop = false)
        {
            if (shop)
            {
                int cur = _cardInven.inventory.Count - 1;
                foreach (var card in _standImages)
                {
                    if(card.CurrentCard == null)
                    {
                        card.CurrentCard = _cardInven.inventory[cur];
                        _cardInven.inventory.RemoveAt(cur);
                        return;
                    }
                }
            }
            else
            {
                _cardInven.Suffle();
                for (int i = 0; i < 6; ++i)
                {
                    int cur = _cardInven.inventory.Count - 1;
                    int next = i + 1;
                    if (i == 5)
                    {
                        if (_cardInven.inventory.Count > 0)
                        {
                            _standImages[i].CurrentCard = _cardInven.inventory[cur];
                            _cardInven.inventory.RemoveAt(cur);
                        }
                        else
                        {
                            _standImages[i].CurrentCard = null;
                        }

                        return;
                    }
                    if (_standImages[next].CurrentCard == null
                        && _cardInven.inventory.Count > 0) //다음카드가 비어있음
                    {
                        _standImages[next].CurrentCard = _cardInven.inventory[cur];
                        _cardInven.inventory.RemoveAt(cur);
                    }

                    _standImages[i].CurrentCard = _standImages[next].CurrentCard;

                }

            }

        }

    }

}