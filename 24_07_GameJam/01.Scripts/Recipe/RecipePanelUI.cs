using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace karin
{

    public class RecipePanelUI : MonoBehaviour
    {
        [SerializeField] private Image[] _resourceImages;
        [SerializeField] private Image _foodImage;
        [SerializeField] private TextMeshProUGUI[] _countTexts;
        [field: SerializeField] public RecipeSO recipe { get; private set; }

        private void Awake()
        {
            _countTexts = GetComponentsInChildren<TextMeshProUGUI>();
            //recipe = Instantiate(_recipe);
            _foodImage.sprite = recipe.food.image;
            _foodImage.color = Color.black;
        }

        public void Refresh()
        {
            bool allOpen = true;
            for (int i = 0; i < 3; i++)
            {
                var resource = recipe.GetResource(i);

                if (resource == null)
                {
                    allOpen = false;
                    continue;
                }

                _resourceImages[i].sprite = resource.image;
                _resourceImages[i].color = Color.white;
                _countTexts[i].text = recipe.resourceCounts[i].ToString();

            }

            if (allOpen)
            {
                _foodImage.color = Color.white;
            }

        }

        public bool RecipeOpen(ResourceSO resource)
        {
            return recipe.RecipeOpen(resource);
        }
    }
}