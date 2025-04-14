using UnityEngine;

public class PlayerInstalliation : Player
{
    Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }


}
