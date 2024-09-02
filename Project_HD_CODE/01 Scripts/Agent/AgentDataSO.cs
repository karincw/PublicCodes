using UnityEngine;

[CreateAssetMenu(menuName = "Karin/SO/AgentData")]
public class AgentDataSO : ScriptableObject
{
    public string characterName;
    public int Cost;
    public int Sight;
    public int MoveRange;
    public int Atk;
    public int AtkRange;
    public int Hp;
    public int Defence;
    public Sprite[] Sptires = new Sprite[3];
    [TextArea] public string Explain;
}
