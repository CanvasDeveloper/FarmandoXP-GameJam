using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FmodPlayer : MonoBehaviour
{


   
    void PlayFootstepsEvent(string path)
    {
        FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance(path);
        Footsteps.start();
        Footsteps.release();
    }
    
}