using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FmodPlayer : MonoBehaviour
{


    FMOD.Studio.EventInstance Footsteps;
    void PlayFootstepsEvent(string path)
    {
        Footsteps = FMODUnity.RuntimeManager.CreateInstance(path);
        Footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(Footsteps, transform, true);
        Footsteps.start();
        Footsteps.release();
    }
    
}