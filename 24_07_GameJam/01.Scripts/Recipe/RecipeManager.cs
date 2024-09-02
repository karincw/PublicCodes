using Karin;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace karin
{

    public class RecipeManager : MonoSingleton<RecipeManager>
    {

        private RecipePanelUI[] _recipes;

        [SerializeField] private Button _openButton;
        [SerializeField] private SlotUI _openSlot;

        [HideInInspector] public List<RecipeSO> openRecipes;

        private void Awake()
        {
            _recipes = GetComponentsInChildren<RecipePanelUI>();
            openRecipes = new List<RecipeSO>();
            openRecipes = GameManager.Instance.openRecipes;

        }

        private void Start()
        {
            foreach (var item in _recipes)
            {
                item.Refresh();
            }
        }

        private void OnEnable()
        {
            _openButton.onClick.AddListener(HandleRecipeOpen);
            foreach (var item in GameManager.Instance.resources)
            {
                OpenRecipe(item);
            }
        }

        private void OnDisable()
        {
            _openButton.onClick.RemoveListener(HandleRecipeOpen);
            GameManager.Instance.UpdateSkillUnlock(openRecipes);
        }

        public void HandleRecipeOpen()
        {
            OpenRecipe(_openSlot);
        }

        public void OpenRecipe(ResourceSO resource)
        {
            foreach (var recipePanel in _recipes)
            {
                if (recipePanel.RecipeOpen(resource))
                {
                    openRecipes.Add(recipePanel.recipe);
                }
                recipePanel.Refresh();
            }
        }

        public void OpenRecipe(SlotUI slot)
        {
            if (!GameManager.Instance.resources.Contains(slot.resource as ResourceSO))
                GameManager.Instance.resources.Add(slot.resource as ResourceSO);
            foreach (var recipePanel in _recipes)
            {
                if (recipePanel.RecipeOpen(slot.resource as ResourceSO))
                {
                    openRecipes.Add(recipePanel.recipe);
                }
                recipePanel.Refresh();
            }
            Inventory.Instance.RemoveItemWithSlot(slot);

        }

    }
}