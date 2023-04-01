using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : MonoBehaviour
{
    [SerializeField] private Image playerHealth;
    [SerializeField] private Image playerMana;

    [SerializeField] private Image[] totens;

    [SerializeField] private GameObject pausePanel;            
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
        gameoverPanel.SetActive(false);
    }
}
