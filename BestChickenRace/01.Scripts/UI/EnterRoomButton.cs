using Packets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomButton : MonoBehaviour
{
    public void EnterRoom()
    {
        C_RoomEnterPacket packet = new C_RoomEnterPacket();

        NetworkManager.Instance.Send(packet);
    }
}
