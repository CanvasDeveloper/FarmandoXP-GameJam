public class ResumePauseButton : ButtonBase
{
    protected override void ButtonBehaviour()
    {
        GameManager.Instance.ResumeGame();
    }
}
