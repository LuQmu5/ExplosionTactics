using System;

public interface IHealth
{
    float Max { get; }
    float Current { get; }

    void TakeDamage(float amount);

    event Action<float, float> Changed;
}