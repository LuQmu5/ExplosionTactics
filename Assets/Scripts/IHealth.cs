using System;

public interface IHealth
{
    float MaxHealth { get; }
    float CurrentHealth { get; }

    void TakeDamage(float amount);

    event Action<float, float> HealthChanged;
}