using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    public FMODUnity.EventReference Click;
    public FMODUnity.EventReference HoverIn;
    public FMODUnity.EventReference HoverOut;

    public void ClickSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(Click, gameObject);
    }
    public void HoverInSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(HoverIn, gameObject);
    }
    public void HoverOutSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(HoverOut, gameObject);
    }
}
