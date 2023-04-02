using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int amount;
    private int index;

    public void SetSpawnIndex(int i)
    {
        index = i;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            player.AddMana(amount);
            SpawnCollectablesManager.Instance.AddToAvaliable(index);

            gameObject.SetActive(false);
        }
    }
}
