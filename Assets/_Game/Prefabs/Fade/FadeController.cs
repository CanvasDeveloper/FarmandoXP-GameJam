using UnityEngine;

public class FadeController : MonoBehaviour
{
    public Animator _animator;

    public void FadeOut()
    {
        _animator.SetTrigger("FadeOut");
    }

    public void FadeIn()
    {
        _animator.SetTrigger("FadeIn");
    }
}
