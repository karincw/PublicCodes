using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamColor
{
    red,
    blue
}

public class Team : MonoBehaviour
{
    public TeamColor team;

    public bool IsMyTeam(Team other)
    {
        if (other == null) return false;

        if (other.team == team)
            return true;
        else
            return false;
    }
}
