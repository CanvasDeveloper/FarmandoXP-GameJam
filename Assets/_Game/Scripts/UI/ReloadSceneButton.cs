public class ReloadSceneButton : ButtonBase
{
    protected override void ButtonBehaviour()
    {
        SceneLoader.Instance.ReloadScene();
    }
}
