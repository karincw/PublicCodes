using System.Linq;
using UnityEngine;

namespace karin
{

    [CreateAssetMenu(menuName = "SO/Karin/Recipe")]
    public class RecipeSO : ScriptableObject
    {
        public bool this[int index]
        {
            set => resourcesOpen[index] = value;
        }

        public Food recipeName;
        public ResourceSO[] resources;
        public int[] resourceCounts;
        public bool[] resourcesOpen;
        public FoodSO food;

        public bool IsOpen => resourcesOpen.All(r => r == true);

        private void OnEnable()
        {
            resourcesOpen = new bool[resources.Length];
        }

        public bool RecipeOpen(ResourceSO resource)
        {
            if (resource == null) return false;

            bool allOpen = true;
            if (resourcesOpen == null)
            {
                resourcesOpen = new bool[resources.Length - 1];
            }

            for (int i = 0; i < resources.Length; i++)
            {
                if (resources[i].itemName.Equals(resource.itemName))
                {
                    resourcesOpen[i] = true;
                }

                if (resourcesOpen[i] == false)
                {
                    allOpen = false;
                }
            }

            return allOpen;
        }

        public ResourceSO GetResource(int index)
        {
            if (resourcesOpen[index] == true)
            {
                return resources[index];
            }
            return null;
        }

        public bool RecipeValidate(ResourceSO r1, ResourceSO r2, ResourceSO r3)
        {
            bool isValidate = true;

            bool v1 = false, v2 = false, v3 = false;

            foreach (ResourceSO resource in resources)
            {
                if (resource.itemName.Equals(r1.itemName) && resource.count <= r1.count && !v1)
                {
                    v1 = true;
                }
                else if (resource.itemName.Equals(r2.itemName) && resource.count <= r2.count && !v2)
                {
                    v2 = true;
                }
                else if (resource.itemName.Equals(r3.itemName) && resource.count <= r3.count && !v3)
                {
                    v3 = true;
                }
                else
                {
                    isValidate = false;
                    break;
                }
            }

            return isValidate;

        }

    }

}