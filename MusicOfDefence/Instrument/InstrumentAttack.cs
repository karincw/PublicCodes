using UnityEngine;

public class InstrumentAttack : MonoBehaviour
{
    AudioSource _audioSource;
    InstrumentManager _instrumentManager;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _instrumentManager = GetComponent<InstrumentManager>();
    }

    public void SoundPlay(int tone)
    {
        _audioSource.clip = _instrumentManager.instrument.instrumentSound[tone];
        _audioSource.PlayOneShot(_audioSource.clip);
    }
}
