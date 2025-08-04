using System;
using UnityEngine;

public class PlayerDeathHandler : IDisposable
{
    private PlayerView _view;
    private RagdollController _ragdoll;
    private HealthSystem _health;
    private PlayerMovement _mover;
    private Transform _transform;

    public PlayerDeathHandler(PlayerView view, RagdollController ragdoll, HealthSystem health, PlayerMovement mover, Transform transform)
    {
        _view = view;
        _ragdoll = ragdoll;
        _health = health;
        _mover = mover;

        _health.Died += OnDied;
        _transform = transform;
    }

    public void Dispose()
    {
        _health.Died -= OnDied;
    }

    private void OnDied(Vector3? forceOrigin, float force)
    {
        _view.Deactivate();
        _mover.Stop();

        Vector3 forceDirection = Vector3.up;

        if (forceOrigin.HasValue)
        {
            Vector3 fromOrigin = (_transform.position - forceOrigin.Value).normalized;
            Vector3 velocityDir = _mover.Velocity.normalized * 0.5f;
            forceDirection += fromOrigin + velocityDir;
        }

        _ragdoll.Activate(forceDirection.normalized, force);
    }
}
