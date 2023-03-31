using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FadeControllerSimple : MonoBehaviour
{
    public Animator _animator;


    public void StarFade()
    {


        _animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
       
      
        _animator.SetTrigger("FadeIn");
    }

    
}
