using UnityEngine;
using System;
using UnityEngine.Playables;

public class GameManager : Singleton<GameManager>
{
    public event Action<bool> OnPauseStatusChange;
    public event Action OnGameOver;

    public event Action OnActiveCutScene;
    
    public event Action OnGameWin;

    public PlayableDirector cutsceneFinal;
    public GameObject temploLevel;

    public bool cutscene;
    public bool Paused { get; private set; }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        TottemManager.OnAllTotemsComplete += ActiveCutscene;
    }

    private void ActiveCutscene()
    {
        cutsceneFinal.Play();

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Templo/Cutscene", transform.position);

        OnActiveCutScene?.Invoke();

        cutscene = true;
        temploLevel.SetActive(false);
    }

    public void FinishCutscene()
    { 
        cutscene = false;
        temploLevel.SetActive(false);
        //cutsceneFinal.gameObject.SetActive(false);

        GameWin();
    }

    private void OnDisable()
    {
        TottemManager.OnAllTotemsComplete -= ActiveCutscene;
    }

    /// <summary>
    /// Use pra evitar que o player pause em horas que nao deve.
    /// </summary>
    public bool CanPause = true;

    private void PauseGame()
    {
        Paused = true;
        Time.timeScale = 0;
        OnPauseStatusChange?.Invoke(Paused);
    }

    public void ResumeGame()
    {
        Paused = false;
        Time.timeScale = 1;
        OnPauseStatusChange?.Invoke(Paused);
    }

    public void GameOver()
    {
        Paused = true;
        Time.timeScale = 0;
        OnPauseStatusChange?.Invoke(Paused);
        OnGameOver?.Invoke();
    }

    public void PauseResume()
    {
        if (!CanPause)
            return;

        if (Paused)
            ResumeGame();
        else
            PauseGame();
    }

    public void GameWin()
    {
        OnGameWin?.Invoke();
    }
}
