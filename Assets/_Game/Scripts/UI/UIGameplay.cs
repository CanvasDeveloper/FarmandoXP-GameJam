using UnityEngine;

public class UIGameplay : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;    
    [SerializeField] private GameObject optionsPanel;    
    [SerializeField] private GameObject gameoverPanel;

    private void OnEnable()
    {
        GameManager.Instance.OnPauseStatusChange += UpdatePauseMenu;
        GameManager.Instance.OnGameOver += OpenGameoverMenu; 
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPauseStatusChange -= UpdatePauseMenu;
        GameManager.Instance.OnGameOver -= OpenGameoverMenu;
    }

    private void UpdatePauseMenu(bool value)
    {
        DisableAllMenus();

        pausePanel.SetActive(value);
    }

    private void OpenGameoverMenu()
    {
        DisableAllMenus();

        gameoverPanel.SetActive(true);
    }

    private void DisableAllMenus()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        gameoverPanel.SetActive(false);
    }
}
