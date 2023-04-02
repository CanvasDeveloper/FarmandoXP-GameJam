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
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image playerManaBar;

    [SerializeField] private List<TotemImage> totenImage;

    [SerializeField] private GameObject pausePanel;            
    [SerializeField] private GameObject gameoverPanel;

    private HealthSystem healthSystem;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        healthSystem = playerController.GetComponent<HealthSystem>();
    }

    private void OnEnable()
    {
        playerController.OnTriggerShootEvent += UpdateManaBar;
        healthSystem.OnChangeHealth += UpdateLifeBar;

        GameManager.Instance.OnPauseStatusChange += UpdatePauseMenu;
        GameManager.Instance.OnGameOver += OpenGameoverMenu;
    }

    private void OnDisable()
    {
        playerController.OnTriggerShootEvent -= UpdateManaBar;
        healthSystem.OnChangeHealth -= UpdateLifeBar;

        GameManager.Instance.OnPauseStatusChange -= UpdatePauseMenu;
        GameManager.Instance.OnGameOver -= OpenGameoverMenu;
    }

    private void Start()
    {
        totenImage.ForEach(x => x.image.gameObject.SetActive(false));
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
