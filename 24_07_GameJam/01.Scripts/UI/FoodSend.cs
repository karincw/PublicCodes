using DG.Tweening;
using karin;
using UnityEngine;
using UnityEngine.UI;
using static Spine.Unity.Examples.SpineboyFootplanter;

namespace Karin
{

    public class FoodSend : MonoBehaviour
    {
        private Image _foodImage;
        private RectTransform _rectTrm;
        private Vector2 _startPos;
        private Vector2 _movePos;
        private FoodSO _food;
        private void Awake()
        {
            _foodImage = GetComponent<Image>();
            _rectTrm = transform as RectTransform;
            _movePos = _rectTrm.anchoredPosition + new Vector2(0, 500);
            _startPos = _rectTrm.anchoredPosition;
        }

        public void Send(FoodSO food)
        {
            _foodImage.sprite = food.image;

            UIOpenManager.Instance.CloseAll();

            _rectTrm.DOAnchorPos(_movePos, 2.5f);
            _food = food;

            Invoke("ASD", 2.5f);
        }

        public void ASD()
        {
            if (NPC.Instance.existNpc)
            {
                NPC.Instance.FoodEvaluation(_food);
                NPC.Instance.ResultGold(_food.price);
            }

            _rectTrm.anchoredPosition = _startPos;
        }
    }

}