using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialVolume : MonoBehaviour
{
    public float MusicVolume;
    public float SFXVolume;
    FMOD.Studio.VCA MusicVCA;
    FMOD.Studio.VCA SFXVCA;

    void Start()
    {
        MusicVCA = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        SFXVCA = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");
        MusicVCA.setVolume(MusicVolume);
        SFXVCA.setVolume(SFXVolume);
    }

}