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
    private bool hasStartTriggerAudio = false;
    private bool hasEndTriggerAudio = false;

    #region PUBLIC VARIABLES

    public ColorTottemEnum colorTottem;
    public bool isSubTottem;
    public List<SpriteRenderer> SlotColors;
    //public List<SlotColor> slotColors;

    #endregion PUBLIC VARIABLES


    #region PRIVATE VARIABLES

    private bool _isRecharging; // INDICA SE O PLAYER EST� CARREGANDO O TOTEM
    public bool IsCompletedTottem { get; private set; }
    
    [SerializeField] private float _RechargeValueBySecond; //CONFIGRAR O TEMPO GASTO DE CARREGAR O TOTEM

    [SerializeField] private float startYValue = 1;

    #endregion PRIVATE VARIABLES

    #region EVENTS

    public event Action<bool> OnPlayerRecharged; // quando player est� carregando o totem com a luz

    #endregion EVENTS

    FMOD.Studio.EventInstance ChargingSound;
    FMOD.Studio.EventInstance IdleSound;

    // Start is called before the first frame update
    void Start()
    {
        IdleSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Totem/Totem Aura Dark");
        IdleSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(IdleSound, transform, true);
        IdleSound.start();

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
            SlotColors[i].size = new Vector2(SlotColors[i].size.x, light); //COMPLETA O TOTEM AUTOM�TICO, OU N�O
        }
        //VERIFICA SE � UM SUBTOTEM
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
                ChargingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                ChargingSound.release();
                switch (colorTottem)
                {
                    case ColorTottemEnum.Yellow:
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Totem/Amarelo", transform.position);
                        break;
                    case ColorTottemEnum.Red:
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Totem/Vermelho", transform.position);
                        break;
                    case ColorTottemEnum.Blue:
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Totem/Azul", transform.position);
                        break;
                    case ColorTottemEnum.Cyan:
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Totem/Ciano", transform.position);
                        break;
                    case ColorTottemEnum.Magenta:
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Totem/Magenta", transform.position);
                        break;
                    case ColorTottemEnum.Green:
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Totem/Verde", transform.position);
                        break;
                }
                
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
        if (value && !hasStartTriggerAudio)
        {
            hasStartTriggerAudio = true;

            IdleSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            IdleSound.release();
            ChargingSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Totem/Totem Aura Lit");
            ChargingSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(ChargingSound, transform, true);
            ChargingSound.start();
        }
        OnPlayerRecharged?.Invoke(value);
        
    }
}
/*if (!hasStartTriggerAudio)
{
    hasStartTriggerAudio = true;

    instance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Fada/Cast Start");
    instance.start();
}
if (!hasEndTriggerAudio)
{
    hasEndTriggerAudio = true;

    instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    instance.release();
    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Fada/Cast End", transform.position);
}*/