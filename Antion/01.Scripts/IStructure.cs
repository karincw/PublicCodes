using System.Collections;
using System.Collections.Generic;
using Tkfkadlsi;
using UnityEngine;

public interface IStructure
{
    public BuildingBuild StructureData { get; set; }
    public float CurrentHp { get; set; }
    public GameObject MygameObject { get; set; }
}
