using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float MaxHealth { get; set; }

    public float CurrentHealth { get; set; }
    public bool IsDie { get ; set ; }

    public event Action<float, float> OnChangeHealth;
    public event Action<IDamageable> OnDie;
    public event Action<Vector3> OnTakeDamage;
    public event Action OnHeal;

    [SerializeField] private bool destroyOnDie;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth);
    }

    public void TakeDamage(Vector3 direction, float damage)
    {
        if (damage <= 0)
            return;

        CurrentHealth -= damage;

        if (CurrentHealth < 0)
        {
            Die();
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
        OnDie?.Invoke(this);


        if (destroyOnDie)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

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
