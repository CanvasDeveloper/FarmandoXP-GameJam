using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VCA : MonoBehaviour
{
    FMOD.Studio.VCA vca;
    public string VCAName;

    private Slider slider;

    void Awake()
    {
        vca = FMODUnity.RuntimeManager.GetVCA("vca:/" + VCAName);
        slider = GetComponent<Slider>();
        //slider.value = 0.1f;
    }


    public void SetVolume(float volume)
    {
        vca.setVolume(volume);
    }

}