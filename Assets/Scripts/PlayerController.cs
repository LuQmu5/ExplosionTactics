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
    [SerializeField] private float _health = 10;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private ClickPointMarkerView _clickPointMarkerView;

    private AIMover _mover;
    private ClickPointMarkerController _marker;
    private Coroutine _takingDamageCoroutine;
    private Vector3 _currentMovePoint;

    private void Start()
    {
        _mover = new AIMover(_agent, _movementSpeed);
        _marker = new ClickPointMarkerController(_clickPointMarkerView);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(RightMouseButtonNumber) && _takingDamageCoroutine == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _groundMask))
            {
                _currentMovePoint = hit.point;

                _mover.MoveToPoint(hit.point);
                _marker.SetMarkerToPosition(hit.point);
            }
        }

        _marker.CheckMarkerForDeactivate(transform.position);
        _view.SetVelocity(_mover.Velocity.magnitude);
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        _view.SetHitTrigger();
        _mover.Stop();

        if (_takingDamageCoroutine != null)
            StopCoroutine(_takingDamageCoroutine);

        _takingDamageCoroutine = StartCoroutine(TakingDamage());

        if (_health <= 0)
        {
            _marker.Deactivate();
            gameObject.SetActive(false);
        }
    }

    private IEnumerator TakingDamage()
    {
        float delay = 0.3f;

        yield return new WaitForSeconds(delay);

        _mover.MoveToPoint(_currentMovePoint);
        _takingDamageCoroutine = null;
    }
}
