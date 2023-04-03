using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ColorTottemEnum { Red, Green, Blue, Cyan, Magenta, Yellow }

[System.Serializable]
public class TottemProgress
{
    public ColorTottemEnum tottemColor;
    public bool isCompleted;
}

public class TottemManager : Singleton<TottemManager>
{
    public List<TottemProgress> listTottemProgress;

    #region EVENTS
    public static Action<ColorTottemEnum> OnTottemRecharged; // quando todos slots foram carregados
    public static Action OnAllTotemsComplete; // todos totens completos
    #endregion EVENTS

    private void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        OnTottemRecharged += SetTottemCompleted;
    }

    private void Unsubscribe()
    {
        OnTottemRecharged -= SetTottemCompleted;
    }

    private void SetTottemCompleted(ColorTottemEnum tottem)
    {
        foreach (var item in listTottemProgress)
        {
            if(tottem == item.tottemColor)
            {
                item.isCompleted = true;
                Debug.LogFormat($"<color={item.tottemColor}>{item.tottemColor.ToString().ToUpper()}!!!!</color>");
            }
        }
        CheckIsAllTotems();
    }

    private void CheckIsAllTotems()
    {
        int amountTotemCompleted = 0;

        foreach (var tot in listTottemProgress)
        {
            if (tot.isCompleted)
                amountTotemCompleted++;

            if (amountTotemCompleted >= listTottemProgress.Count)
            {
                OnAllTotemsComplete?.Invoke();
                Debug.Log("<color=white>Todos Tottems completo</color>");
            }
        }
    }

    public bool CheckSubTottem(ColorTottemEnum tottem)
    {
        bool isValid = false;
        switch (tottem)
        {
            case ColorTottemEnum.Cyan:
                isValid = CheckColors(ColorTottemEnum.Green, ColorTottemEnum.Blue);
                break;
            case ColorTottemEnum.Magenta:
                isValid = CheckColors(ColorTottemEnum.Red, ColorTottemEnum.Blue);
                break;
            case ColorTottemEnum.Yellow:
                isValid = CheckColors(ColorTottemEnum.Red, ColorTottemEnum.Green);
                break;
        }

        return isValid;
    }

    private bool CheckColors(ColorTottemEnum color1, ColorTottemEnum color2)
    {
        bool hasColor1 = false;
        bool hasColor2 = false;

        foreach (var tottem in listTottemProgress)
        {
            if(tottem.tottemColor == color1 && tottem.isCompleted)
            {
                hasColor1 = true;
            }

            if (tottem.tottemColor == color2 && tottem.isCompleted)
            {
                hasColor2 = true;
            }
        }

        if(hasColor1 && hasColor2 == false)
        {
            Debug.Log($"Cor {color1} = {hasColor1} / Cor {color2} = {hasColor2}");
        }

        return hasColor1 && hasColor2;
    }
}
