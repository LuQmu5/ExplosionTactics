using UnityEngine;

public class PlayerDamageHandler
{
    private HealthSystem _health;
    private PlayerMovement _movement;
    private PlayerView _view;
    private RagdollController _ragdoll;

    private Coroutine _takingDamage;
    public bool IsDeadOrStaggered => _health.CurrentHealth <= 0 || _takingDamage != null;

    public PlayerDamageHandler(HealthSystem health, PlayerMovement movement, PlayerView view, RagdollController ragdoll)
    {
        _health = health;
        _movement = movement;
        _view = view;
        _ragdoll = ragdoll;

        _health.TakedDamage += OnTakedDamage;
        _health.Died += OnDied;
    }

    private void OnTakedDamage()
    {
        _movement.Stop();
        _view.SetHitTrigger();
        _view.SetHealthPercentParam(_health.CurrentHealth / _health.MaxHealth);
    }

    private void OnDied(Vector3? origin, float force)
    {
        _movement.Stop();
        _view.Deactivate();
        _ragdoll.Activate((Vector3)origin, force);

        Vector3 direction = origin.HasValue
            ? (_view.transform.position - origin.Value).normalized
            : Vector3.up;
    }
}
