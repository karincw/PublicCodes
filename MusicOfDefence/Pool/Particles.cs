
public class Particles : Poolable
{

    private void OnParticleSystemStopped()
    {
        Release();
    }
}
