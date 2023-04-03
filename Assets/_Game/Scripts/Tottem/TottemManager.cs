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
    public Dictionary<ColorTottemEnum, bool> tottemProgress;
    public List<TottemProgress> listTottemProgress;

    #region EVENTS
    public static Action<ColorTottemEnum> OnTottemRecharged; // quando todos slots foram carregados
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
        OnTottemRecharged += Tottem;
    }

    private void Unsubscribe()
    {
        OnTottemRecharged -= Tottem;
    }

    private void Tottem(ColorTottemEnum tottem)
    {
        foreach (var item in listTottemProgress)
        {
            if(tottem == item.tottemColor)
            {
                item.isCompleted = true;
                Debug.LogFormat($"<color={item.tottemColor}>{item.tottemColor.ToString().ToUpper()}!!!!</color>");
            }
        }

        //switch (tottem)     
        //{
        //    case ColorTottemEnum.Red:
        //        Debug.LogFormat("<color=red>VERMELHO!!!!</color>");
        //        break;
        //    case ColorTottemEnum.Green:
        //        Debug.LogFormat("<color=green>VERDE!!!!</color>");
        //        break;
        //    case ColorTottemEnum.Blue:
        //        Debug.LogFormat("<color=blue>AZUL!!!!</color>");
        //        break;
        //    case ColorTottemEnum.Cyan:
        //        Debug.LogFormat("<color=cyan>CIANO!!!!</color>");
        //        break;
        //    case ColorTottemEnum.Magenta:
        //        Debug.LogFormat("<color=magenta>MAGENTA!!!!</color>");
        //        break;
        //    case ColorTottemEnum.Yellow:
        //        Debug.LogFormat("<color=yellow>AMARELO!!!!</color>");
        //        break;
        //    default:
        //        break;
        //}
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
            else if (tottem.tottemColor == color2 && tottem.isCompleted)
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
