using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteColliderTottem : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "RechargeTottem")
        {
            collision.transform.parent.GetComponent<Tottem>().OnPlayerRecharged?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "RechargeTottem")
        {
            collision.transform.parent.GetComponent<Tottem>().OnPlayerRecharged?.Invoke(false);
        }
    }

}
