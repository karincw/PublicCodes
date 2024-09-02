using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace karin
{

    public class SlotUI : MonoBehaviour
    {
        public ItemSO resource;

        public int maxCount = 0;

        public RectTransform IconRect
        {
            get
            {
                _iconRect = _icon.rectTransform;
                return _iconRect;
            }
            set
            {
                _iconRect = value;
            }
        }
        private RectTransform _iconRect;

        public Image Icon
        {
            get
            {
                if (_icon == null)
                    _icon = transform.Find("ItemImage").GetComponent<Image>();

                return _icon;
            }
            private set
            {
                _icon = value;
            }
        }
        private Image _icon;

        public Image Border
        {
            get
            {
                if (_border == null)
                    _border = transform.Find("Border").GetComponent<Image>();

                return _border;
            }
            private set
            {
                _border = value;
            }
        }
        private Image _border;

        public TextMeshProUGUI CountText
        {
            get
            {
                if (_countText == null)
                    _countText = transform.GetComponentInChildren<TextMeshProUGUI>();

                return _countText;
            }
            private set
            {
                _countText = value;
            }
        }
        private TextMeshProUGUI _countText;

        public bool HasItem => resource != null;

        private void OnEnable()
        {
            Refresh();
        }

        public bool CanAdd(int count) => maxCount >= resource.count + count;
        public bool CanRemove(int count) => resource.count - count >= 0;
        public int MaxAdd()
        {
            if (resource != null)
                return maxCount - resource.count;
            else
                return maxCount;
        }

        public void Refresh()
        {
            if (resource == null || resource.count == 0)
            {
                resource = null;
                Icon.sprite = null;
                CountText.text = string.Empty;

                return;
            }
            Icon.sprite = resource.image;
            CountText.text = resource.count.ToString();
        }

        public void EnableBorder(bool state)
        {
            Border.gameObject.SetActive(state);
        }
    }
}