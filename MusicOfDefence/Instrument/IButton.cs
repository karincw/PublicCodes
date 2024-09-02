using System;

public interface IButton
{
    public void ToneUp();
    public void ToneDown();
    public void SpeedUP();
    public void SpeedDown();
    public void ViewTone(bool value);
    public void ViewSpeed(bool value);

}
