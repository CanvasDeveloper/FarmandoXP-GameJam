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
            var tottem = collision.transform.parent.GetComponent<Tottem>();

            //tottem.TriggerPlayerRecharged(controller, true); //OnPlayerRecharged?.Invoke(true);
            controller.SetTottem(tottem);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "RechargeTottem")
        {
            var tottem = collision.transform.parent.GetComponent<Tottem>();

            tottem.TriggerPlayerRecharged(controller, false); //OnPlayerRecharged?.Invoke(false);
            controller.SetTottem(null);
        }
    }

}
