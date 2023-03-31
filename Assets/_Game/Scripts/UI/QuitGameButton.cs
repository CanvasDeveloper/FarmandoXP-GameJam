using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
