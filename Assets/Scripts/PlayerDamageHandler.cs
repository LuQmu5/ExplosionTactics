using UnityEngine;

public class PlayerDamageHandler
{
    private HealthSystem _health;
    private PlayerMovement _movement;
    private PlayerView _view;

    private Coroutine _takingDamage;
    public bool IsDeadOrStaggered => _health.CurrentHealth <= 0 || _takingDamage != null;

    public PlayerDamageHandler(HealthSystem health, PlayerMovement movement, PlayerView view)
    {
        _health = health;
        _movement = movement;
        _view = view;

        _health.TakedDamage += OnTakedDamage;
    }

    private void OnTakedDamage()
    {
        _movement.Stop();
        _view.SetHitTrigger();
        _view.SetHealthPercentParam(_health.CurrentHealth / _health.MaxHealth);
    }
}
