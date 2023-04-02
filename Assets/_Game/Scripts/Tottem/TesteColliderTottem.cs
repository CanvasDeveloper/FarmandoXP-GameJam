using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteColliderTottem : MonoBehaviour
{
    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!controller.HasBullets())
            return;

        if(collision.tag == "RechargeTottem")
        {
            collision.transform.parent.GetComponent<Tottem>().TriggerPlayerRecharged(controller, true); //OnPlayerRecharged?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "RechargeTottem")
        {
            collision.transform.parent.GetComponent<Tottem>().TriggerPlayerRecharged(controller, false); //OnPlayerRecharged?.Invoke(false);
        }
    }

}
