using karin;
using Karin;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    private readonly Food[] SkillRecipeSO = { Food.HealthySoup, Food.ColdAndHotPot, Food.BraisedDragonTail };

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public int npcCount = 0;
    public int gold = 0;
    public float time = 0, maxTime = 0;
    public string text;
    public NPCdataSO npcdata = null;

    public List<RecipeSO> openRecipes = new List<RecipeSO>();
    public List<ResourceSO> resources = new List<ResourceSO>();
    public bool[] skillUnlockState = new bool[3] { false , false , false };

    public void UpdateSkillUnlock(List<RecipeSO> openRecipes)
    {
        this.openRecipes = openRecipes;

        for (int i = 0; i < 3; i++)
        {
            skillUnlockState[i] = openRecipes.Any(x => x.recipeName == SkillRecipeSO[i]);
        }
    }

    public void CombatScene()
    {
        SceneManager.LoadScene(2);
    }
}
