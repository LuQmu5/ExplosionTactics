using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private const int RightMouseButtonNumber = 1;

    [Header("Components")]
    [SerializeField] private PlayerView _view;
    [SerializeField] private RagdollController _ragdollController;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private HealthSystem _healthSystem;

    [Space(20)]
    [Header("Settings")]
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private ClickPointMarkerView _clickPointMarkerViewPrefab;

    private AIMover _mover;
    private ClickPointMarkerController _markerController;

    private Coroutine _takingDamageCoroutine;
    private Vector3 _currentMovePoint;

    private void Start()
    {
        _mover = new AIMover(_agent, _movementSpeed);
        _currentMovePoint = transform.position;

        _view.Activate();
        _ragdollController.Deactivate();

        _markerController = new ClickPointMarkerController(_clickPointMarkerViewPrefab);

        _healthSystem.TakedDamage += OnTakedDamage;
        _healthSystem.Died += OnDeath;
    }

    private void OnDestroy()
    {
        _healthSystem.TakedDamage -= OnTakedDamage;
        _healthSystem.Died += OnDeath;
    }

    private void Update()
    {
        if (_healthSystem.CurrentHealth <= 0 || _takingDamageCoroutine != null)
            return;

        if (TryGetMovePoint(out _currentMovePoint))
        {
            _mover.MoveToPoint(_currentMovePoint);
            _markerController.SetMarkerToPosition(_currentMovePoint);
        }

        _markerController.CheckMarkerForDeactivate(transform.position);
        _view.SetVelocity(_mover.Velocity.magnitude);
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

    private void OnDeath(Vector3? forceOrigin, float force)
    {
        _mover.Stop();
        _markerController.Deactivate();
        _view.Deactivate();
        _ragdollController.Activate();

        if (forceOrigin.HasValue)
        {
            Vector3 direction = (transform.position - forceOrigin.Value).normalized;
            Vector3 velocityComponent = _mover.Velocity.normalized * 0.5f;
            _ragdollController.AplyForce(direction + velocityComponent, force);
        }
        else
        {
            _ragdollController.AplyForce(Vector3.up, 50);
        }
    }

    public void OnTakedDamage() 
    {
        if (_takingDamageCoroutine != null)
            StopCoroutine(_takingDamageCoroutine);

        _mover.Stop();
        _view.SetHitTrigger();
        _takingDamageCoroutine = StartCoroutine(TakingDamagePause());
    }

    private IEnumerator TakingDamagePause()
    {
        float delay = _view.GetAnimationClipLength(AnimationNames.Hit);
        yield return new WaitForSeconds(delay);

        _mover.MoveToPoint(_currentMovePoint);
        _takingDamageCoroutine = null;
    }
}
