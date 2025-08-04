using UnityEngine;
using System;


public class HealthSystem : MonoBehaviour, IHealth
{
    private float _maxHealth = 10;
    public float MaxHealth => _maxHealth;
    public float CurrentHealth { get; private set; }

    public event Action<float, float> HealthChanged;
    public event Action<Vector3?, float> Died;
    public event Action TakedDamage;

    private bool _isDead;

    public void Init(float maxHealth)
    {
        _maxHealth = maxHealth;
        CurrentHealth = _maxHealth;
        _isDead = false;
    }

    public void TakeDamage(float amount, Vector3? forceOrigin = null, float force = 0f)
    {
        if (_isDead)
            return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, _maxHealth);
        HealthChanged?.Invoke(CurrentHealth, MaxHealth);
        TakedDamage?.Invoke();

        if (CurrentHealth <= 0f)
        {
            _isDead = true;
            Died?.Invoke(forceOrigin, force);
        }
    }
}
