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

    private void Start()
    {
        _mover = new AIMover(_agent, _movementSpeed);
        _marker = new ClickPointMarkerController(_clickPointMarkerView);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(RightMouseButtonNumber))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _groundMask))
            {
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

        if (_health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
