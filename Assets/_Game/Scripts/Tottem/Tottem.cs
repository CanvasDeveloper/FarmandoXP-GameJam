using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotColor
{
    public SpriteRenderer[] spriteColors;
    public bool isFull;
}

public class Tottem : MonoBehaviour
{
    #region PUBLIC VARIABLES

    public ColorTottemEnum colorTottem;
    public bool isSubTottem;
    public List<SpriteRenderer> SlotColors;
    //public List<SlotColor> slotColors;

    #endregion PUBLIC VARIABLES


    #region PRIVATE VARIABLES

    private bool _isRecharging; // INDICA SE O PLAYER ESTÁ CARREGANDO O TOTEM
    public bool IsCompletedTottem { get; private set; }
    
    [SerializeField] private float _RechargeValueBySecond; //CONFIGRAR O TEMPO GASTO DE CARREGAR O TOTEM

    [SerializeField] private float startYValue = 1;

    #endregion PRIVATE VARIABLES

    #region EVENTS

    public event Action<bool> OnPlayerRecharged; // quando player está carregando o totem com a luz

    #endregion EVENTS

    // Start is called before the first frame update
    void Start()
    {
        Subscribe();

        float light = 0; // usado para setar o valor inicial
        
        foreach (var tottem in TottemManager.Instance.listTottemProgress) // VERIFICA O PROGRESSO DO TOTEM
        {
            if (tottem.tottemColor == colorTottem)
            {
                if (tottem.isCompleted)
                    light = startYValue;
                else
                    light = 0;
            }
        }
        for (int i = 0; i < SlotColors.Count; i++)
        {
            SlotColors[i].size = new Vector2(SlotColors[i].size.x, light); //COMPLETA O TOTEM AUTOMÁTICO, OU NÃO
        }
        //VERIFICA SE É UM SUBTOTEM
        if (colorTottem == ColorTottemEnum.Cyan || colorTottem == ColorTottemEnum.Magenta || colorTottem == ColorTottemEnum.Yellow)
            isSubTottem = true;

        UpdateRecharge();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCompletedTottem)
            return;

        if (_isRecharging == false)
            return;

        UpdateRecharge();
    }

    private void Subscribe()
    {
        OnPlayerRecharged += PlayerRecharging;
    }

    private void Unsubscribe()
    {
        OnPlayerRecharged -= PlayerRecharging;
    }

    private void PlayerRecharging(bool isRecharged)
    {
        _isRecharging = isRecharged;
    }

    public bool IsValidTotem() => TottemManager.Instance.CheckSubTottem(colorTottem);
    public bool IsSubTotem() => isSubTottem;

    private void UpdateRecharge()
    {
        if (IsSubTotem())
        {
            bool isValid = IsValidTotem();
            if (isValid == false)
                return;

        }

        SpriteRenderer current = null;
        int amountSlotCompleted = 0;
        for (int i = 0; i < SlotColors.Count; i++)
        {
            if (SlotColors[i].size.y >= startYValue)
            {
                amountSlotCompleted++; //TEM ALGUM SLOT COMPLETO
            }
            else
            {
                current = SlotColors[i];
                break;
            }
            if (amountSlotCompleted == SlotColors.Count)
            {
                TottemManager.OnTottemRecharged?.Invoke(colorTottem); // TOTEM COMPLETO;
                IsCompletedTottem = true;
                return;
            }
        }

        //CARREGA O TOTTEM
        float currentLight = 0;
        if (current != null)
        {
            currentLight = current.size.y;
            currentLight += Time.deltaTime / _RechargeValueBySecond;
            currentLight = Mathf.Clamp01(currentLight);

            if (currentLight >= startYValue)
                currentLight = startYValue;
            current.size = new Vector2(current.size.x, currentLight);
        }
    }

    public void TriggerPlayerRecharged(PlayerController controller, bool value)
    {
        OnPlayerRecharged?.Invoke(value);
    }
}
