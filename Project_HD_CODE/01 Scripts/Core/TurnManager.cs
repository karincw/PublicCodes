using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private ushort turn = 0;

    [ContextMenu("NextTurn")]
    public void NextTurn()
    {
        var agents = FindObjectsOfType<AgentHex>();
        turn++;

        foreach (var agent in agents)
        {
            agent.TurnReset();
        }
    }
}
