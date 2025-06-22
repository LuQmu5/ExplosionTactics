using System;
using UnityEngine;

public interface IHealth
{
    float MaxHealth { get; }
    float CurrentHealth { get; }

    void TakeDamage(float amount, Vector3? forceOrigin = null, float force = 0f);

    event Action<float, float> HealthChanged;
}