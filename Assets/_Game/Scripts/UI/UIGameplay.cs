using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class TotemImage
{    
    public Image image;
}

public class UIGameplay : MonoBehaviour
{
    [SerializeField] private Image playerHealth;
    [SerializeField] private Image playerMana;

    [SerializeField] private List<TotemImage> totenImage;

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

    private void Start()
    {
        totenImage.ForEach(x => x.image.gameObject.SetActive(false));
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
