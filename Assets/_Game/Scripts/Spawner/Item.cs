using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private float amount;
    private int index;

    [SerializeField] private bool outOfSpawn;

    public void SetSpawnIndex(int i)
    {
        outOfSpawn = false;
        index = i;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            player.AddMana(amount);

            if(outOfSpawn)
            {
                SpawnManager.Instance.AddToAvaliable(index);
            }
            
            gameObject.SetActive(false);
        }
    }
}
