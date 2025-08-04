using UnityEngine.AI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private PlayerView _view;
    [SerializeField] private RagdollController _ragdoll;
    [SerializeField] private ClickPointMarkerView _clickMarkerView;
    [SerializeField] private HealthSystem _health;
    [SerializeField] private HealthDisplay _healthDisplay;

    [Header("Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _idleTimeToPatrol;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _jumpUpOffset = 0.5f;
    [SerializeField] private float _jumpDownPeak = 1f;

    private PlayerDeathHandler _deathHandler;
    private PlayerInputHandler _input;
    private PlayerMovement _movement;
    private PlayerDamageHandler _damage;
    private PlayerJumpHandler _jump;
    private ClickMarkerController _marker;

    private Vector3 _currentTarget;

    public void Init()
    {
        _health.Init(10);

        _input = new PlayerInputHandler(_groundMask);
        _movement = new PlayerMovement(_agent, _movementSpeed);
        _damage = new PlayerDamageHandler(_health, _movement, _view);
        _jump = new PlayerJumpHandler(transform, _agent, _view, _jumpUpOffset, _jumpDownPeak);
        _marker = new ClickMarkerController(_clickMarkerView);
        _deathHandler = new PlayerDeathHandler(_view, _ragdoll, _health, _movement, transform);

        _healthDisplay.Init(_health);
    }

    private void Update()
    {
        if (_damage.IsDeadOrStaggered)
            return;

        if (_input.TryGetClickPoint(out Vector3 point))
        {
            _currentTarget = point;
            _movement.MoveTo(point);
            _marker.SetMarkerToPosition(point);
        }

        _marker.CheckMarkerForDeactivate(transform.position);
        _view.SetVelocity(_movement.Velocity.magnitude);
        _jump.CheckAndTryJump(_currentTarget);
    }
}
