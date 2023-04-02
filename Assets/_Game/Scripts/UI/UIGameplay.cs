using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class TotemImage
{
    public ColorTottemEnum color;
    public Image image;
}

public class UIGameplay : MonoBehaviour
{
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image playerManaBar;

    [SerializeField] private List<TotemImage> totenImage;

    [SerializeField] private GameObject pausePanel;            
    [SerializeField] private GameObject gameoverPanel;

    [SerializeField] private Button firstButtonPause;
    [SerializeField] private Button firstButtonGameOver;

    private HealthSystem healthSystem;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        healthSystem = playerController.GetComponent<HealthSystem>();
    }

    private void Start()
    {
        totenImage.ForEach(x => x.image.gameObject.SetActive(false));

        playerController.OnTriggerShootEvent += UpdateManaBar;
        healthSystem.OnChangeHealth += UpdateLifeBar;

        GameManager.Instance.OnPauseStatusChange += UpdatePauseMenu;
        GameManager.Instance.OnGameOver += OpenGameoverMenu;

        TottemManager.Instance.OnCompletedTottem += UpdateColorIcons;
    }

    private void OnDestroy()
    {
        playerController.OnTriggerShootEvent -= UpdateManaBar;
        healthSystem.OnChangeHealth -= UpdateLifeBar;

        GameManager.Instance.OnPauseStatusChange -= UpdatePauseMenu;
        GameManager.Instance.OnGameOver -= OpenGameoverMenu;

        TottemManager.Instance.OnCompletedTottem -= UpdateColorIcons;
    }

    private void UpdateColorIcons(ColorTottemEnum colorEnum)
    {
        foreach(TotemImage totem in totenImage)
        {
            if (totem.color != colorEnum)
                continue;

            totem.image.gameObject.SetActive(true);
        }    
    }

    private void UpdateLifeBar(float current, float max)
    {
        playerHealthBar.fillAmount = current / max;
    }

    private void UpdateManaBar(float current, float max)
    {
        playerManaBar.fillAmount = current / max;
    }

    private void UpdatePauseMenu(bool value)
    {
        DisableAllMenus();

        firstButtonPause.Select();
        pausePanel.SetActive(value);
    }

    private void OpenGameoverMenu()
    {
        DisableAllMenus();

        firstButtonGameOver.Select();
        gameoverPanel.SetActive(true);
    }

    private void DisableAllMenus()
    {
        pausePanel.SetActive(false);
        gameoverPanel.SetActive(false);
    }
}
