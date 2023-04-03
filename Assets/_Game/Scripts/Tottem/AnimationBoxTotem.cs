using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBoxTotem : MonoBehaviour
{

  private Animator an;
  private Tottem _Tottem;
    // Start is called before the first frame update
    void Start()
    {
        an = GetComponent<Animator>();
        _Tottem = GetComponentInParent<Tottem>();
    }

    // Update is called once per frame
    void Update()
    {
        an.SetBool("totemAtivo", _Tottem.IsCompletedTottem);
    }
}
