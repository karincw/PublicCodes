using karin;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Karin
{

    public class CraftingPot : MonoBehaviour
    {
        private Stack<ResourceSO> resources;

        [SerializeField] private Image[] _resourceImages;
        [SerializeField] private Button _arrow;
        [SerializeField] private FoodSend _foodSend;
        private int idx = 0;

        [SerializeField] private UnityEvent resourceAddEvent;

        private void Awake()
        {
            _arrow.interactable = false;
            resources = new Stack<ResourceSO>();
        }

        private void OnEnable()
        {
            _arrow.onClick.AddListener(Craft);
        }

        private void OnDisable()
        {
            _arrow.onClick.RemoveListener(Craft);
        }

        public void AddItem(ResourceSO resource)
        {
            resourceAddEvent?.Invoke();
            resources.Push(resource);

            if (idx > 2)
            {
                _arrow.GetComponent<Image>().color = Color.white;
                _resourceImages[2].sprite = _resourceImages[1].sprite;
                _resourceImages[1].sprite = _resourceImages[0].sprite;
                _resourceImages[0].sprite = resource.image;
                _arrow.interactable = false;
            }
            else
            {
                _resourceImages[idx].color = new Color(1, 1, 1, 1);
                _resourceImages[idx++].sprite = resource.image;
                if (idx == 3)
                {
                    _arrow.GetComponent<Image>().color = Color.green;
                    _arrow.interactable = NPC.Instance.existNpc && true;
                }

            }
        }

        public void Craft()
        {

            if (resources.Count < 3)
                return;

            bool craftSuccess = false;
            ResourceSO r1 = resources.Pop(), r2 = resources.Pop(), r3 = resources.Pop();
            FoodSO resultFood = null;
            foreach (var recipe in RecipeManager.Instance.openRecipes)
            {
                craftSuccess = recipe.RecipeValidate(r1, r2, r3);
                if (craftSuccess == true)
                {
                    resultFood = recipe.food;
                    break;
                }
            }

            if (craftSuccess)
            {
                Debug.Log($"만드는데 성공함 : {resultFood}");
                _foodSend.Send(resultFood);
            }

            resources.Clear();
            _resourceImages[0].sprite = null;
            _resourceImages[1].sprite = null;
            _resourceImages[2].sprite = null;
            _resourceImages[0].color = new Color(1, 1, 1, 0);
            _resourceImages[1].color = new Color(1, 1, 1, 0);
            _resourceImages[2].color = new Color(1, 1, 1, 0);
            idx = 0;
        }
    }
}