using DG.Tweening;
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
    [SerializeField] private float _idleTimeToPatrol = 5;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private ClickPointMarkerView _clickPointMarkerViewPrefab;
    [SerializeField] private float _jumpUpOffset = 0.5f;
    [SerializeField] private float _jumpDownPeak = 1f;


    private AIMover _mover;
    private ClickPointMarkerController _markerController;
    private PlayerNavMeshPatrolSystem _patrolSystem;

    private Coroutine _takingDamageCoroutine;
    private Coroutine _jumpingCoroutine;

    private Vector3 _currentMovePoint;
    private float _currentIdleTimer;

    private void Start()
    {
        _mover = new AIMover(_agent, _movementSpeed);
        _currentMovePoint = transform.position;

        _view.Activate();
        _ragdollController.Deactivate();

        _markerController = new ClickPointMarkerController(_clickPointMarkerViewPrefab);

        _healthSystem.TakedDamage += OnTakedDamage;
        _healthSystem.Died += OnDeath;

        _patrolSystem = new PlayerNavMeshPatrolSystem(this, _mover);
        _currentIdleTimer = _idleTimeToPatrol;
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

        if (_jumpingCoroutine == null && TryGetMovePoint(out _currentMovePoint))
        {
            _currentIdleTimer = _idleTimeToPatrol;
            _patrolSystem.StopPatrol();

            _mover.MoveToPoint(_currentMovePoint);
            _markerController.SetMarkerToPosition(_currentMovePoint);
        }

        _markerController.CheckMarkerForDeactivate(transform.position);
        _view.SetVelocity(_mover.Velocity.magnitude);

        if (_currentIdleTimer > 0)
            _currentIdleTimer -= Time.deltaTime;

        if (_currentIdleTimer <= 0)
            _patrolSystem.StartPatrol();

        if (_agent.enabled && _agent.isOnOffMeshLink && _jumpingCoroutine == null)
        {
            _jumpingCoroutine = StartCoroutine(JumpAcrossLink());
        }
    }

    private IEnumerator JumpAcrossLink()
    {
        _view.SetJumpingState(true);

        OffMeshLinkData link = _agent.currentOffMeshLinkData;
        Vector3 startPos = _agent.transform.position;
        Vector3 endPos = link.endPos;

        // Только горизонтальный поворот
        Vector3 flatDir = new Vector3(endPos.x - startPos.x, 0f, endPos.z - startPos.z);
        Quaternion targetRotation = Quaternion.LookRotation(flatDir);

        _agent.enabled = false;

        // Быстрый разворот
        float rotationDuration = 0.1f * _view.GetAnimationClipLength(AnimationNames.Jump);
        transform.DORotateQuaternion(targetRotation, rotationDuration)
                 .OnUpdate(() =>
                 {
                     var euler = transform.eulerAngles;
                     transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
                 });

        yield return new WaitForSeconds(rotationDuration);

        float animationStartDelay = 0.15f;
        yield return new WaitForSeconds(animationStartDelay);

        // Прыжок
        float jumpDuration = _view.GetAnimationClipLength(AnimationNames.Jump) * 0.5f;
        float verticalDelta = endPos.y - startPos.y;

        float jumpPower = verticalDelta > 0
            ? verticalDelta + _jumpUpOffset
            : _jumpDownPeak;

        Tween jumpTween = transform
            .DOJump(endPos, jumpPower, 1, jumpDuration)
            .SetEase(Ease.OutQuad);

        yield return jumpTween.WaitForCompletion();

        transform.position = endPos;
        transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

        _agent.enabled = true;
        _agent.CompleteOffMeshLink();
        _jumpingCoroutine = null;

        _mover.MoveToPoint(_currentMovePoint);

        _view.SetJumpingState(false);
    }




    private bool TryGetMovePoint(out Vector3 result)
    {
        result = _currentMovePoint;

        if (Input.GetMouseButton(RightMouseButtonNumber)) // DOwn
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
            _ragdollController.AplyForce(Vector3.up + direction + velocityComponent, force);
        }
        else
        {
            _ragdollController.AplyForce(Vector3.up, force);
        }
    }

    public void OnTakedDamage() 
    {
        if (_takingDamageCoroutine != null)
            StopCoroutine(_takingDamageCoroutine);

        _mover.Stop();

        _view.SetHitTrigger();
        _view.SetHealthPercentParam(_healthSystem.CurrentHealth / _healthSystem.MaxHealth);

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
