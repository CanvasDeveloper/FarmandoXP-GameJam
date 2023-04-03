using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : IDamageable
{
    public float CurrentHealth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float MaxHealth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsDie { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event Action<float, float> OnChangeHealth;
    public event Action<Vector3> OnTakeDamage;
    public event Action OnHeal;
    public event Action<IDamageable> OnDie;

    public void Die()
    {
        
    }

    public void Heal(float amount)
    {
        
    }

    public void TakeDamage(Vector3 direction, float damage)
    {
        OnTakeDamage?.Invoke(direction);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
