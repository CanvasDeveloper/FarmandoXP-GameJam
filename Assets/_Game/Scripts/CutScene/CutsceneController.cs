using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector playableDirector;

    void Update()
    {
        if(GameManager.Instance.cutscene)
        {
            if (playableDirector.state == PlayState.Paused)
            {
                GameManager.Instance.FinishCutscene();
            }
        }
    }
}
