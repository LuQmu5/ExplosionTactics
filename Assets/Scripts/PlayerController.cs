using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, IHealth
{
    private const int RightMouseButtonNumber = 1;

    [Header("Components")]
    [SerializeField] private PlayerView _view;
    [SerializeField] private RagdollController _ragdollController;
    [SerializeField] private NavMeshAgent _agent;

    [Space(20)]
    [Header("Settings")]
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _maxHealth = 10;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private ClickPointMarkerView _clickPointMarkerViewPrefab;

    private AIMover _mover;
    private ClickPointMarkerController _markerController;
    private Coroutine _takingDamageCoroutine;

    private Vector3 _currentMovePoint;

    public event Action<float, float> HealthChanged;

    public float MaxHealth { get; private set; }

    public float CurrentHealth { get; private set; }

    private void Start()
    {
        _mover = new AIMover(_agent, _movementSpeed);
        _currentMovePoint = transform.position;

        _view.Activate();
        _ragdollController.Deactivate();

        _markerController = new ClickPointMarkerController(_clickPointMarkerViewPrefab);

        MaxHealth = _maxHealth;
        CurrentHealth = _maxHealth;
        HealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    private void Update()
    {
        if (CurrentHealth <= 0 || _takingDamageCoroutine != null)
            return;

        if (TryGetMovePoint(out _currentMovePoint)) 
        {
            _mover.MoveToPoint(_currentMovePoint);
            _markerController.SetMarkerToPosition(_currentMovePoint);
        }

        _markerController.CheckMarkerForDeactivate(transform.position);
        _view.SetVelocity(_mover.Velocity.magnitude);
    }

    public void TakeDamage(float amount, Vector3? forceOrigin = null, float force = 0f)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, CurrentHealth);
        HealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth == 0)
        {
            HandleDeath(forceOrigin, force);
            return;
        }

        _mover.Stop();
        _view.SetHitTrigger();

        if (_takingDamageCoroutine != null)
            StopCoroutine(_takingDamageCoroutine);

        _takingDamageCoroutine = StartCoroutine(TakingDamagePause());
    }


    private bool TryGetMovePoint(out Vector3 result)
    {
        result = _currentMovePoint;

        if (Input.GetMouseButtonDown(RightMouseButtonNumber))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _groundMask))
            {
                result = hit.point;

                return true;
            }
        }

        return false;
    }

    private void HandleDeath(Vector3? forceOrigin = null, float force = 0f)
    {
        _mover.Stop();
        _markerController.Deactivate();
        _view.Deactivate();

        _ragdollController.Activate();

        if (forceOrigin.HasValue)
        {
            Vector3 direction = (transform.position - forceOrigin.Value).normalized;

            float velocityComponentFactor = 0.25f;
            Vector3 velocityComponent = _mover.Velocity.normalized * velocityComponentFactor;

            _ragdollController.AplyForce(Vector3.up + direction + velocityComponent, force);
        }
    }


    private IEnumerator TakingDamagePause()
    {
        float delay = _view.GetAnimationClipLength(AnimationNames.Hit);

        yield return new WaitForSeconds(delay);

        _mover.MoveToPoint(_currentMovePoint);
        _takingDamageCoroutine = null;
    }
}