using Karin;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace karin
{

    public class Inventory : MonoSingleton<Inventory>
    {
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private SlotUI[] _slots;
        [SerializeField] private InvenDataSO data;
        [SerializeField] private ResourceSO res;

        [SerializeField] private EventSystem _eventsystem;
        [SerializeField] private bool DebugingMode = true;

        private GraphicRaycaster _graphicRaycaster;
        private PointerEventData _pointerEventData;
        private List<RaycastResult> _rrList;

        private SlotUI _beginDragSlot; // ���� �巡�׸� ������ ����
        private Transform _beginDragIconTransform; // �ش� ������ ������ Ʈ������
        private Transform _beginSlotTransform; // �ش� ������ Ʈ������

        private Vector3 _beginDragIconPoint;   // �巡�� ���� �� ������ ��ġ
        private Vector3 _beginDragCursorPoint; // �巡�� ���� �� Ŀ���� ��ġ
        private int _beginDragSlotSiblingIndex;

        private SlotUI _cursorItem;

        public Dictionary<ItemNames, int> itemCountDic;

        private bool _isDragging;
        private bool _split;

        private Transform _canvasTrm;

        /// <summary>
        /// ������ ������ �������� �˷��ִ� �Լ�
        /// ����
        ///   1. slot1�� �ִ� ���� <= slot2�� �ִ� ����
        ///   2. slot1�� �ִ� ���� <= slot2�� ���� ����
        /// </summary>
        /// <returns> ������ �������� ���� </returns>
        private bool CanSwap(SlotUI slot1, SlotUI slot2)
        {
            bool condition1 = slot1.maxCount <= slot2.maxCount;
            bool condition2 = true;
            if (slot2.resource != null)
            {
                condition2 = slot1.maxCount >= slot2.resource.count;
            }

            return condition1 && condition2;
        }

        #region Debug

        [Header("Debug")]

        public ItemSO testItem1;
        public ItemSO testItem2;
        public ItemSO testItem3;
        public ItemSO testItem4;
        public ItemSO testItem5;
        public ItemSO testItem6;

        #endregion

        private void Awake()
        {
            _graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
            _canvasTrm = GetComponentInParent<Canvas>().transform;
            _pointerEventData = new PointerEventData(_eventsystem);
            itemCountDic = new Dictionary<ItemNames, int>();
            _rrList = new List<RaycastResult>();
            foreach (ItemNames itemEnum in Enum.GetValues(typeof(ItemNames)))
            {
                itemCountDic.Add(itemEnum, 0);
            }
            LoadItemData();
        }

        private void OnEnable()
        {
            _inputReader.OnMLBDownEvent += HandleMouseDownEvent;
            _inputReader.OnMLBHoldEvent += HandleMouseHoldEvent;
            _inputReader.OnMLBUpEvent += HandleMouseUpEvent;
            _inputReader.OnLCtrlEvent += HandleLCtrlEvent;
            _isDragging = false;
        }

        private void OnDisable()
        {
            _inputReader.OnMLBDownEvent -= HandleMouseDownEvent;
            _inputReader.OnMLBHoldEvent -= HandleMouseHoldEvent;
            _inputReader.OnMLBUpEvent -= HandleMouseUpEvent;
            SaveItemData();
        }

        private void Update()
        {
            _pointerEventData.position = Input.mousePosition;

            if (_isDragging)
            {
                OnPointerDrag();
            }

            #region Debug
            if (DebugingMode)
            {
                if (Keyboard.current.qKey.wasReleasedThisFrame)
                    AddItem(testItem1);
                if (Keyboard.current.wKey.wasReleasedThisFrame)
                    AddItem(testItem2);
                if (Keyboard.current.eKey.wasReleasedThisFrame)
                    AddItem(testItem3);
                if (Keyboard.current.rKey.wasReleasedThisFrame)
                    AddItem(testItem4);
                if (Keyboard.current.tKey.wasReleasedThisFrame)
                    AddItem(testItem5);
                if (Keyboard.current.yKey.wasReleasedThisFrame)
                    AddItem(testItem6);

                if (Keyboard.current.aKey.wasReleasedThisFrame)
                    RemoveItem(testItem1);
                if (Keyboard.current.sKey.wasReleasedThisFrame)
                    RemoveItem(testItem2);
                if (Keyboard.current.dKey.wasReleasedThisFrame)
                    RemoveItem(testItem3);
                if (Keyboard.current.fKey.wasReleasedThisFrame)
                    RemoveItem(testItem4);
                if (Keyboard.current.gKey.wasReleasedThisFrame)
                    RemoveItem(testItem5);
                if (Keyboard.current.hKey.wasReleasedThisFrame)
                    RemoveItem(testItem6);
            }
            #endregion
        }

        [ContextMenu("Save")]
        public void SaveItemData()
        {
            data.list.Clear();
            foreach (var slot in _slots)
            {
                if (slot.resource == null) continue;
                InvenData invenData = new InvenData(slot.resource.itemName, slot.resource.image, slot.resource.count);
                data.list.Add(invenData);
            }
        }

        [ContextMenu("Load")]
        public void LoadItemData()
        {
            foreach (var slot in _slots)
            {
                slot.resource = null;
                slot.Refresh();
            }
            for (int i = 0; i < data.list.Count; i++)
            {
                ItemSO so = Instantiate(res);
                so.itemName = data.list[i].itemName;
                so.image = data.list[i].image;
                so.count = 1;
                AddItem(so, data.list[i].count);
            }
        }

        [ContextMenu("Init")]
        public void Init()
        {
            _slots = GetComponentsInChildren<SlotUI>();
            _graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
            _canvasTrm = GetComponentInParent<Canvas>().transform;
            _pointerEventData = new PointerEventData(_eventsystem);
            itemCountDic = new Dictionary<ItemNames, int>();
            _rrList = new List<RaycastResult>();
            foreach (ItemNames itemEnum in Enum.GetValues(typeof(ItemNames)))
            {
                itemCountDic.Add(itemEnum, 0);
            }
        }

        public bool AddItem(ItemSO item, int count = 1)
        {
            bool completeAdd = false;
            if (itemCountDic[item.itemName] > 0)
            {
                foreach (SlotUI slot in _slots)
                {
                    if (slot.resource == null) continue;

                    if (slot.resource.itemName.Equals(item.itemName) && slot.CanAdd(count))
                    {
                        slot.resource.count += count;
                        slot.Refresh();
                        completeAdd = true;

                        break;
                    }

                }
            }

            if (!completeAdd)
            {
                foreach (SlotUI slot in _slots)
                {
                    if (slot.resource == null)
                    {
                        ItemSO slotItem = Instantiate(item);
                        slot.resource = slotItem;
                        slot.resource.count = count;
                        slot.Refresh();

                        break;
                    }
                }
            }

            itemCountDic[item.itemName] += count;
            return completeAdd;
        }

        /// <summary>
        /// �������� �κ��丮�� �տ������� �Ҹ�
        /// </summary>
        /// <param name="item"> ����� ������ </param>
        /// <param name="count"> ����� �������� ����</param>
        public bool RemoveItem(ItemSO item, int count = 1)
        {
            bool completeRemove = false;

            if (itemCountDic[item.itemName] > 0)
            {
                foreach (SlotUI slot in _slots)
                {
                    if (slot.resource == null) continue;

                    if (slot.resource.itemName == item.itemName && slot.CanRemove(count))
                    {
                        slot.resource.count -= count;
                        completeRemove = true;
                        itemCountDic[item.itemName] -= count;
                        slot.Refresh();

                        break;
                    }

                }
            }
            if (!completeRemove)
            {
                Debug.LogError("�κ��丮�� ���¾������̳� ������ �������� �������� ���� �������� �������");
            }

            return completeRemove;
        }

        /// <summary>
        /// �ش罽���� �������� �Ҹ�
        /// </summary>
        /// <param name="slot"> �������� �Ҹ��ų ���� </param>
        /// <param name="count"> �������� �Ҹ��� ���� </param>
        public bool RemoveItemWithSlot(SlotUI slot, int count = 1)
        {
            if (slot.resource == null) return false;
            bool completeRemove = false;

            if (itemCountDic[slot.resource.itemName] > 0)
            {
                if (slot.CanRemove(count))
                {
                    slot.resource.count -= count;
                    completeRemove = true;
                    itemCountDic[slot.resource.itemName] -= count;
                    slot.Refresh();
                }
            }
            if (!completeRemove)
            {
                Debug.LogError("�κ��丮�� ���¾������̳� ������ �������� �������� ���� �������� �������");
            }

            return completeRemove;
        }

        /// <summary>
        /// ���� ���� ��� �߿��� ���� ù��°�� ���� �ָ� ��ȯ�ϴ� �Լ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>���� ���� ��� �߿��� ���� ù��°�� ���� ��</returns>
        private T RaycastAndGetFirstComponent<T>() where T : Component
        {
            _rrList.Clear();

            _graphicRaycaster.Raycast(_pointerEventData, _rrList);

            if (_rrList.Count == 0)
                return null;

            return _rrList[0].gameObject.GetComponent<T>();
        }

        /// <summary>
        /// OnPointerDown
        /// </summary>
        private void HandleMouseDownEvent()
        {
            _beginDragSlot = RaycastAndGetFirstComponent<SlotUI>();

            // �������� ���� �ִ� ���Ը� �ش�
            if (_beginDragSlot != null && _beginDragSlot.HasItem)
            {
                // ��ġ ���, ���� ���
                _beginDragIconTransform = _beginDragSlot.IconRect.transform;
                _beginDragIconPoint = _beginDragIconTransform.position;
                //_beginDragCursorPoint = Input.mousePosition;
                _beginDragCursorPoint = _beginDragIconPoint;

                // �� ���� ���̱�
                _beginDragSlotSiblingIndex = _beginDragSlot.transform.GetSiblingIndex();
                _beginSlotTransform = _beginDragSlot.transform.parent;
                _beginDragSlot.transform.SetParent(_canvasTrm);
                _beginDragSlot.transform.SetAsLastSibling();
            }
            else
            {
                _beginDragSlot = null;
            }
        }

        /// <summary>
        /// OnPointerUp
        /// </summary>
        private void HandleMouseUpEvent()
        {
            // End Drag
            if (_beginDragSlot != null)
            {
                // ��ġ ����
                _beginDragIconTransform.position = _beginDragIconPoint;

                // UI ���� ����
                _beginDragSlot.transform.SetParent(_beginSlotTransform);
                _beginDragSlot.transform.SetSiblingIndex(_beginDragSlotSiblingIndex);

                // �巡�� �Ϸ� ó��
                EndDrag(RaycastAndGetFirstComponent<SlotUI>());
                EndDrag(RaycastAndGetFirstComponent<CraftingPot>());

                // ���� ����
                _cursorItem.EnableBorder(false);
                _cursorItem = null;
                _beginDragSlot = null;
                _beginDragIconTransform = null;
            }
        }

        private void OnPointerDrag()
        {
            if (_beginDragSlot == null) return;

            // ��ġ �̵�
            _beginDragIconTransform.position =
                _beginDragIconPoint + (Input.mousePosition - _beginDragCursorPoint);

            var findItem = RaycastAndGetFirstComponent<SlotUI>();
            if (_cursorItem != findItem && findItem != null)
            {
                if (_cursorItem != null)
                    _cursorItem.EnableBorder(false);

                _cursorItem = findItem;
                _cursorItem.EnableBorder(true);
            }
        }

        private void EndDrag(SlotUI slotUI)
        {
            if (slotUI == null || _beginDragSlot == slotUI) return;

            if (slotUI.resource == null) // �巡���� ������ ����ִٸ�
            {
                if (_split) // �����̵� �̶��
                {
                    SlotResourceMove(_beginDragSlot, slotUI, 1); //���ҽ��� 1���� ����
                }
                else if (CanSwap(_beginDragSlot, slotUI)) // Swap������ �����Ѵٸ�
                {
                    SlotResourceSwap(_beginDragSlot, slotUI); //���ҽ� ����
                }
                else // �����̵��� �ƴϰ� Swap������ �������ϸ�
                {
                    SlotResourceMaxMove(_beginDragSlot, slotUI); //�ű���ִ¸�ŭ ���ҽ� ����
                }
            }
            else if (_beginDragSlot.resource.itemName.Equals(slotUI.resource.itemName)) //������ ���ҽ��� ���� ���ٸ�
            {
                if (_split) // �����̵� �̶��
                {
                    SlotResourceMove(_beginDragSlot, slotUI, 1); //���ҽ��� 1���� ����
                }
                else
                {
                    SlotResourceMaxMove(_beginDragSlot, slotUI); //�ű���ִ¸�ŭ ���ҽ� ����
                }
            }
            else // ������ ���ҽ��� �ٸ��ٸ�
            {
                if (CanSwap(_beginDragSlot, slotUI)) // Swap������ �����Ѵٸ�
                {
                    SlotResourceSwap(_beginDragSlot, slotUI);
                }
                else // �����̵��� �ƴϰ� Swap������ �������ϸ�
                {
                    SlotResourceMaxMove(_beginDragSlot, slotUI); //�ű���ִ¸�ŭ ���ҽ� ����
                }
            }
            _beginDragSlot.Refresh();
            slotUI.Refresh();
        }

        private void EndDrag(CraftingPot pot)
        {
            if (pot == null || _beginDragSlot == null) return;
            pot.AddItem(_beginDragSlot.resource as ResourceSO);
            _beginDragSlot.resource = null;

            _beginDragSlot.Refresh();
        }

        /// <summary>
        /// Slot�� Resource�� ���� ���ҽ�Ŵ
        /// </summary>
        /// <param name="slot1">���ҽ��� ���� ����</param>
        /// <param name="slot2">���ҽ��� ���� ����</param>
        private void SlotResourceSwap(SlotUI slot1, SlotUI slot2)
        {
            (slot1.resource, slot2.resource) = (slot2.resource, slot1.resource);
        }

        /// <summary>
        /// Slot1�� Resource�� �ű���ִ� �ִ밪��ŭ slot2�� �ű�
        /// </summary>
        /// <param name="slot1">���ҽ��� ���� ����</param>
        /// <param name="slot2">���ҽ��� ���� ����</param>
        private void SlotResourceMaxMove(SlotUI slot1, SlotUI slot2)
        {
            if (slot2.resource != null && !slot1.resource.itemName.Equals(slot2.resource.itemName))
            {
                //Ȥ�ö� ���ҽ��� �ٸ����� ���� ���Ϸ��� �ϸ� ���� ��������
                Debug.LogWarning($"Cannot Match Slot Resource {slot1.resource} {slot2.resource}");
                return;
            }

            int value = slot2.MaxAdd(); // slot2�� �������ִ� �ִ� ����
            int currentValue = Mathf.Min(slot1.resource.count, value); // ������ִ� �ִ밪
            slot1.resource.count -= currentValue; // �������Կ��� �׸�ŭ �����

            if (slot2.resource == null) //slot2�� ���ҽ��� ���ٸ� 
            {
                ItemSO slotItem = Instantiate(slot1.resource); // �� ���ҽ��� �����
                slot2.resource = slotItem; //slot2�� ���ҽ��� ���� ���� ���ҽ��� ����
                slot2.resource.count = 0; //���θ��� ������ ���� �ʱ�ȭ
            }
            slot2.resource.count += currentValue; //��¥ ���� ����
        }

        /// <summary>
        /// Slot1�� Resource�� value��ŭ slot2�� �ű�
        /// </summary>
        /// <param name="slot1">���ҽ��� ���� ����</param>
        /// <param name="slot2">���ҽ��� ���� ����</param>
        ///  <param name="value">�ű� ����</param>
        private void SlotResourceMove(SlotUI slot1, SlotUI slot2, int value)
        {
            if (slot2.resource != null && !slot1.resource.itemName.Equals(slot2.resource.itemName))
            {
                //Ȥ�ö� ���ҽ��� �ٸ����� ���� ���Ϸ��� �ϸ� ���� ��������
                Debug.LogWarning($"Cannot Match Slot Resource {slot1.resource} {slot2.resource}");
                return;
            }

            int currentValue = Mathf.Min(slot1.resource.count, value); // ������ִ� �ִ밪

            if (slot2.MaxAdd() < currentValue)
            {
                return;
            }

            slot1.resource.count -= currentValue; // �������Կ��� currentValue��ŭ �����

            if (slot2.resource == null) //slot2�� ���ҽ��� ���ٸ� 
            {
                ItemSO slotItem = Instantiate(slot1.resource); // �� ���ҽ��� �����
                slot2.resource = slotItem; //slot2�� ���ҽ��� ���� ���� ���ҽ��� ����
                slot2.resource.count = 0; //���θ��� ������ ���� �ʱ�ȭ
            }
            slot2.resource.count += currentValue; //��¥ ���� ����
        }

        #region Handle

        private void HandleLCtrlEvent(bool state)
        {
            _split = state;
        }

        private void HandleMouseHoldEvent(bool state)
        {
            _isDragging = state;
        }

        #endregion
    }
}