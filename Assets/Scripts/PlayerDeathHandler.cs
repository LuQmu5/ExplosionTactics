using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private PlayerView _view;
    [SerializeField] private RagdollController _ragdoll;

    private HealthSystem _health;
    private PlayerMovement _mover;

    public void Init(HealthSystem health, PlayerMovement mover)
    {
        _health = health;
        _mover = mover;

        _health.Died += OnDied;
    }

    private void OnDestroy()
    {
        if (_health != null)
            _health.Died -= OnDied;
    }

    private void OnDied(Vector3? forceOrigin, float force)
    {
        _view.Deactivate();

        _mover.Stop();

        Vector3 forceDirection = Vector3.up;

        if (forceOrigin.HasValue)
        {
            Vector3 fromOrigin = (transform.position - forceOrigin.Value).normalized;
            Vector3 velocityDir = _mover.Velocity.normalized * 0.5f;
            forceDirection += fromOrigin + velocityDir;
        }

        _ragdoll.Activate(forceDirection.normalized, force);
    }
}
