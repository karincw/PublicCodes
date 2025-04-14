using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStateManager newPlayer;
        if (collision.TryGetComponent<PlayerStateManager>(out newPlayer))
        {
            newPlayer.Winning();
        }
    }
}
