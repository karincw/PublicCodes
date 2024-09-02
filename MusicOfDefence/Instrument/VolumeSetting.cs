using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider InstrumentSlider;
    [SerializeField] private Slider DrumSlider;
    [SerializeField] private Slider BaseSlider;
    [SerializeField] private Slider GuitarSlider;
    [SerializeField] private Slider HarpSlider;
    [SerializeField] private Slider MarimbaSlider;
    [SerializeField] private Slider PianoSlider;

    private void Start()
    {
        SetMasterVolume();
        SetInstrumentVolume();
        SetDrumVolume();
        SetBaseVolume();
        SetGuitarVolume();
        SetHarpVolume();
        SetMarimbaVolume();
        SetPianoVolume();
    }

    public void SetMasterVolume()
    {
        float volume = MasterSlider.value;
        myAudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetInstrumentVolume()
    {
        float volume = InstrumentSlider.value;
        myAudioMixer.SetFloat("Instrument", Mathf.Log10(volume) * 20);
    }

    public void SetDrumVolume()
    {
        float volume = DrumSlider.value;
        myAudioMixer.SetFloat("Drum", Mathf.Log10(volume) * 20);
    }

    public void SetBaseVolume()
    {
        float volume = BaseSlider.value;
        myAudioMixer.SetFloat("Base", Mathf.Log10(volume) * 20);
    }

    public void SetGuitarVolume()
    {
        float volume = GuitarSlider.value;
        myAudioMixer.SetFloat("Guitar", Mathf.Log10(volume) * 20);
    }

    public void SetHarpVolume()
    {
        float volume = HarpSlider.value;
        myAudioMixer.SetFloat("Harp", Mathf.Log10(volume) * 20);
    }

    public void SetMarimbaVolume()
    {
        float volume = MarimbaSlider.value;
        myAudioMixer.SetFloat("Marimba", Mathf.Log10(volume) * 20);
    }

    public void SetPianoVolume()
    {
        float volume = PianoSlider.value;
        myAudioMixer.SetFloat("Piano", Mathf.Log10(volume) * 20);
    }


}
