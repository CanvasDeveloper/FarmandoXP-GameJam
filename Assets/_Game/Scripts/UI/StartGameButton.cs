public class StartGameButton : ButtonBase
{
    protected override void ButtonBehaviour()
    {
        SceneLoader.Instance.NextScene();
    }
}
