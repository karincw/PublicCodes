using UnityEngine;

[CreateAssetMenu(menuName = "SO/InstrumentSO")]
public class InstrumentSO : ScriptableObject
{
    public int[] rankAccordingNoteNumber = new int[4];
    public int damage
    {
        get
        {
            switch (rank)
            {
                case 0:
                    return rankAccordingDamage[0];
                case 1:
                    return rankAccordingDamage[1];
                case 2:
                    return rankAccordingDamage[2];
                case 3:
                    return rankAccordingDamage[3];
                default: return 0;
            }
        }
        private set { }
    }
    [Tooltip("¾Ç±â µ¥¹ÌÁö")]
    public int[] rankAccordingDamage = new int[4];
    public int criticalPersent
    {
        get
        {
            switch (rank)
            {
                case 0:
                    return rankAccordingCriticalPersent[0];
                case 1:
                    return rankAccordingCriticalPersent[1];
                case 2:
                    return rankAccordingCriticalPersent[2];
                case 3:
                    return rankAccordingCriticalPersent[3];
                default: return 0;
            }
        }
        private set { }
    }
    [Tooltip("¾Ç±â Å©¸®Æ¼ÄÃ È®·ü")]
    public int[] rankAccordingCriticalPersent = new int[4];
    public float criticalDamage
    {
        get
        {
            switch (rank)
            {
                case 0:
                    return rankAccordingCriticaldamage[0];
                case 1:
                    return rankAccordingCriticaldamage[1];
                case 2:
                    return rankAccordingCriticaldamage[2];
                case 3:
                    return rankAccordingCriticaldamage[3];
                default: return 0;
            }
        }
        private set { }
    }
    [Tooltip("¾Ç±â Å©¸®Æ¼ÄÃ µ¥¹ÌÁöÁõ°¡·®")]
    public float[] rankAccordingCriticaldamage = new float[4];
    [Tooltip("¾Ç±â ¼Ò¸®")]
    public AudioClip[] instrumentSound;
    [Tooltip("¾Ç±â ÀÌ¹ÌÁö")]
    public Sprite image;
    [Tooltip("¾Ç±â µî±Þ")]
    public int rank;
    public Sprite[] rankImage;
    public int SoundLength
    {
        get
        {
            return instrumentSound.Length - 1;
        }
    }
}
