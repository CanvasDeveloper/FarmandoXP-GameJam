public class ReturnToMenuButton : ButtonBase
{
    protected override void ButtonBehaviour()
    {
        SceneLoader.Instance.LoadTitle();
    }
}
