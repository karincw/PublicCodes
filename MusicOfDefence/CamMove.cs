using Cinemachine;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera InstrumentCamera;
    [SerializeField] private CinemachineVirtualCamera FightCamera;

    public bool isInstrummentView;

    private void Awake()
    {
        isInstrummentView = true;
    }

    public void CameraMove()
    {
        if (isInstrummentView == false)
        {
            InstrumentAreaView();
        }
        else
        {
            FightAreaView();
        }
    }

    public void FightAreaView()
    {
        isInstrummentView = false;
        InstrumentCamera.Priority = 5;
        FightCamera.Priority = 10;
    }

    public void InstrumentAreaView()
    {
        isInstrummentView = true;
        InstrumentCamera.Priority = 10;
        FightCamera.Priority = 5;
    }
}
