using UnityEngine;

public class DetectHealthSystem : MonoBehaviour
{
    public enum DetectionType
    {
        Collision,
        Trigger,
    }

    [SerializeField] private DetectionType type;
    [Tooltip("Quantidade de dano")]
    [SerializeField] private float damage;
    [Tooltip("Quantidade de cura")]
    [SerializeField] private float heal;

    [SerializeField] private bool destroyOnCollide = true;
    
    [SerializeField] private GameObject spawnEffectPrefab;

    private void Start()
    {
        //teste de mudanças de dano
       //damage = PowerUpController.Instance.m_player.BulletDamage;
      

    }

  

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type != DetectionType.Collision)
            return;

        if(collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(transform.position, damage);
            damageable.Heal(heal);
        }

        DestroyObject();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type != DetectionType.Trigger)
            return;

        if (collision.gameObject.TryGetComponent(out IDamageable damageable) && !collision.gameObject.CompareTag("Tree"))
        {
            damageable.TakeDamage(transform.position, damage);
            damageable.Heal(heal);
        }

        DestroyObject();
    }

    public void SetChangeDamage(float valor)
    {
        damage += valor;
    }

    private void DestroyObject()
    {
        if (!destroyOnCollide)
            return;

        if (spawnEffectPrefab)
            Instantiate(spawnEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
