using UnityEngine;
using System;

public interface IHealth
{
    float MaxHealth { get; }
    float CurrentHealth { get; }
    void TakeDamage(float amount, Vector3? forceOrigin = null, float force = 0f);

    event Action<float, float> HealthChanged;
    event Action<Vector3?, float> Died;
}
