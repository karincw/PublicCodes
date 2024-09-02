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

        private SlotUI _beginDragSlot; // 현재 드래그를 시작한 슬롯
        private Transform _beginDragIconTransform; // 해당 슬롯의 아이콘 트랜스폼
        private Transform _beginSlotTransform; // 해당 슬롯의 트랜스폼

        private Vector3 _beginDragIconPoint;   // 드래그 시작 시 슬롯의 위치
        private Vector3 _beginDragCursorPoint; // 드래그 시작 시 커서의 위치
        private int _beginDragSlotSiblingIndex;

        private SlotUI _cursorItem;

        public Dictionary<ItemNames, int> itemCountDic;

        private bool _isDragging;
        private bool _split;

        private Transform _canvasTrm;

        /// <summary>
        /// 슬롯의 스왑이 가능한지 알려주는 함수
        /// 조건
        ///   1. slot1의 최대 개수 <= slot2의 최대 개수
        ///   2. slot1의 최대 개수 <= slot2의 현재 개수
        /// </summary>
        /// <returns> 스왑이 가능한지 여부 </returns>
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
        /// 아이템을 인벤토리의 앞에서부터 소모
        /// </summary>
        /// <param name="item"> 사용할 아이템 </param>
        /// <param name="count"> 사용할 아이템의 개수</param>
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
                Debug.LogError("인벤토리에 없는아이템이나 보유한 아이템의 개수보다 많은 아이템을 사용했음");
            }

            return completeRemove;
        }

        /// <summary>
        /// 해당슬롯의 아이템을 소모
        /// </summary>
        /// <param name="slot"> 아이템을 소모시킬 슬롯 </param>
        /// <param name="count"> 아이템을 소모할 개수 </param>
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
                Debug.LogError("인벤토리에 없는아이템이나 보유한 아이템의 개수보다 많은 아이템을 사용했음");
            }

            return completeRemove;
        }

        /// <summary>
        /// 레이 맞은 얘들 중에서 가장 첫번째로 맞은 애를 반환하는 함수
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>레이 맞은 얘들 중에서 가장 첫번째로 맞은 애</returns>
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

            // 아이템을 갖고 있는 슬롯만 해당
            if (_beginDragSlot != null && _beginDragSlot.HasItem)
            {
                // 위치 기억, 참조 등록
                _beginDragIconTransform = _beginDragSlot.IconRect.transform;
                _beginDragIconPoint = _beginDragIconTransform.position;
                //_beginDragCursorPoint = Input.mousePosition;
                _beginDragCursorPoint = _beginDragIconPoint;

                // 맨 위에 보이기
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
                // 위치 복원
                _beginDragIconTransform.position = _beginDragIconPoint;

                // UI 순서 복원
                _beginDragSlot.transform.SetParent(_beginSlotTransform);
                _beginDragSlot.transform.SetSiblingIndex(_beginDragSlotSiblingIndex);

                // 드래그 완료 처리
                EndDrag(RaycastAndGetFirstComponent<SlotUI>());
                EndDrag(RaycastAndGetFirstComponent<CraftingPot>());

                // 참조 제거
                _cursorItem.EnableBorder(false);
                _cursorItem = null;
                _beginDragSlot = null;
                _beginDragIconTransform = null;
            }
        }

        private void OnPointerDrag()
        {
            if (_beginDragSlot == null) return;

            // 위치 이동
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

            if (slotUI.resource == null) // 드래그할 슬롯이 비어있다면
            {
                if (_split) // 분할이동 이라면
                {
                    SlotResourceMove(_beginDragSlot, slotUI, 1); //리소스를 1개만 옯김
                }
                else if (CanSwap(_beginDragSlot, slotUI)) // Swap조건이 충족한다면
                {
                    SlotResourceSwap(_beginDragSlot, slotUI); //리소스 스왑
                }
                else // 분할이동이 아니고 Swap조건이 충족안하면
                {
                    SlotResourceMaxMove(_beginDragSlot, slotUI); //옮길수있는만큼 리소스 옯김
                }
            }
            else if (_beginDragSlot.resource.itemName.Equals(slotUI.resource.itemName)) //슬롯의 리소스가 서로 같다면
            {
                if (_split) // 분할이동 이라면
                {
                    SlotResourceMove(_beginDragSlot, slotUI, 1); //리소스를 1개만 옯김
                }
                else
                {
                    SlotResourceMaxMove(_beginDragSlot, slotUI); //옮길수있는만큼 리소스 옯김
                }
            }
            else // 슬롯의 리소스가 다르다면
            {
                if (CanSwap(_beginDragSlot, slotUI)) // Swap조건이 충족한다면
                {
                    SlotResourceSwap(_beginDragSlot, slotUI);
                }
                else // 분할이동이 아니고 Swap조건이 충족안하면
                {
                    SlotResourceMaxMove(_beginDragSlot, slotUI); //옮길수있는만큼 리소스 옯김
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
        /// Slot의 Resource를 서로 스왑시킴
        /// </summary>
        /// <param name="slot1">리소스를 옯길 슬롯</param>
        /// <param name="slot2">리소스를 받을 슬롯</param>
        private void SlotResourceSwap(SlotUI slot1, SlotUI slot2)
        {
            (slot1.resource, slot2.resource) = (slot2.resource, slot1.resource);
        }

        /// <summary>
        /// Slot1의 Resource를 옮길수있는 최대값만큼 slot2로 옮김
        /// </summary>
        /// <param name="slot1">리소스를 옯길 슬롯</param>
        /// <param name="slot2">리소스를 받을 슬롯</param>
        private void SlotResourceMaxMove(SlotUI slot1, SlotUI slot2)
        {
            if (slot2.resource != null && !slot1.resource.itemName.Equals(slot2.resource.itemName))
            {
                //혹시라도 리소스가 다른슬롯 끼리 더하려고 하면 에러 내보내기
                Debug.LogWarning($"Cannot Match Slot Resource {slot1.resource} {slot2.resource}");
                return;
            }

            int value = slot2.MaxAdd(); // slot2에 넣을수있는 최대 개수
            int currentValue = Mathf.Min(slot1.resource.count, value); // 옯길수있는 최대값
            slot1.resource.count -= currentValue; // 원래슬롯에서 그만큼 지우고

            if (slot2.resource == null) //slot2의 리소스가 없다면 
            {
                ItemSO slotItem = Instantiate(slot1.resource); // 새 리소스를 만들고
                slot2.resource = slotItem; //slot2의 리소스를 새로 만든 리소스로 넣음
                slot2.resource.count = 0; //새로만들 슬롯의 개수 초기화
            }
            slot2.resource.count += currentValue; //진짜 개수 삽입
        }

        /// <summary>
        /// Slot1의 Resource를 value만큼 slot2로 옮김
        /// </summary>
        /// <param name="slot1">리소스를 옯길 슬롯</param>
        /// <param name="slot2">리소스를 받을 슬롯</param>
        ///  <param name="value">옮길 개수</param>
        private void SlotResourceMove(SlotUI slot1, SlotUI slot2, int value)
        {
            if (slot2.resource != null && !slot1.resource.itemName.Equals(slot2.resource.itemName))
            {
                //혹시라도 리소스가 다른슬롯 끼리 더하려고 하면 에러 내보내기
                Debug.LogWarning($"Cannot Match Slot Resource {slot1.resource} {slot2.resource}");
                return;
            }

            int currentValue = Mathf.Min(slot1.resource.count, value); // 옯길수있는 최대값

            if (slot2.MaxAdd() < currentValue)
            {
                return;
            }

            slot1.resource.count -= currentValue; // 원래슬롯에서 currentValue만큼 지우고

            if (slot2.resource == null) //slot2의 리소스가 없다면 
            {
                ItemSO slotItem = Instantiate(slot1.resource); // 새 리소스를 만들고
                slot2.resource = slotItem; //slot2의 리소스를 새로 만든 리소스로 넣음
                slot2.resource.count = 0; //새로만들 슬롯의 개수 초기화
            }
            slot2.resource.count += currentValue; //진짜 개수 삽입
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