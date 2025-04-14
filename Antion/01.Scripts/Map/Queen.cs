using System.Collections;
using System.Collections.Generic;
using Tkfkadlsi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Queen : MonoBehaviour, IStructure
{
    public BuildingBuild StructureData { get => structureData; set => structureData = value; }
    [SerializeField] private BuildingBuild structureData;
    private float currentHp = 1;
    public float CurrentHp { get => currentHp; set => currentHp = value; }

    public GameObject MygameObject { get; set; }

    private void Awake()
    {
        MygameObject = gameObject;
        CurrentHp = structureData.maxHealth;
    }

    private void Update()
    {
        if (CurrentHp <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }
}
