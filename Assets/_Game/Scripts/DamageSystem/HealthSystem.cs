using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    //[field:] serve pra deixar variaveis que tem esse { get; set; } no final, visiveis no editor;
    [field:SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float MaxHealth { get; set; }
    public bool IsDie { get ; set ; }

    //diz que esse carinha alterou a vida (dano ou cura), e passa a vida atual
    public event Action<float, float> OnChangeHealth;
    //avisa que o gameObject foi morto
    public event Action<IDamageable> OnDie;
    public event Action<Vector3> OnTakeDamage;
    public event Action OnHeal;

    [SerializeField] private bool destroyOnDie;

    [SerializeField] private int enemyXp;
    public Collider2D collider2d;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
        /*if (gameObject.CompareTag("Player"))
            CurrentHealth = TreeController.Instance.CurrentHealth;*/
        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth);
    }

    private void OnEnable()
    {
        CurrentHealth = MaxHealth;
        collider2d.enabled = true;
    }

    /// <summary>
    /// Tomar dano
    /// </summary>
    /// <param name="damage">dano</param>
    public void TakeDamage(Vector3 direction, float damage)
    {
        if (damage <= 0)
            return;

        CurrentHealth -= damage;

        if (CurrentHealth < 0)
        {
            Die();
            anim.SetBool("isDie", true);
            return;
        }

        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth);
        OnTakeDamage?.Invoke(direction);
    }



    public void Die()
    {
        if (IsDie)
            return;
        IsDie = true;
        collider2d.enabled = false;
        TreeController.Instance.SetAddXp(enemyXp);
        OnDie?.Invoke(this);


        if (destroyOnDie)
        {
        }//evita que o player seja destruido
         //gameObject.SetActive(false);

        if (this.gameObject.name == "EnemyPerto(Clone)")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cogumelo/Cogumelo Explosion", GetComponent<Transform>().position);
        }
        if (this.gameObject.name == "EnemyTiro(Clone)")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cogumelo/Cogumelo Explosion", GetComponent<Transform>().position);
        }
        if (this.gameObject.name == "Kamikaze(Clone)")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Kamikaze/Kamikaze Explosion", GetComponent<Transform>().position);
        }
    }

    public void SetDieAnimation()
    {
        gameObject.SetActive(false);
    }



    /// <summary>
    /// Para curar
    /// </summary>
    /// <param name="amount">quantidade</param>
    public void Heal(float amount)
    {
        if (amount <= 0)
            return;

        CurrentHealth += amount;

        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;

        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth);
        OnHeal?.Invoke();
    }
}
