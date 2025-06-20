using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private const int RightMouseButtonNumber = 1;

    [SerializeField] private PlayerView _view;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private ClickPointMarker _clickPointMarkerPrefab;
    [SerializeField] private RagdollController _ragdollController;
    [SerializeField] private Transform _mainBody; // любая кость тела или трансформ тела
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private float _groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask _groundLayer;

    private ClickPointMarker _activeMarker;
    private bool _isRagdolling = false;
    private float _ragdollStartTime;

    public Vector3 Velocity => _agent.velocity;

    private void Start()
    {
        _agent.speed = _movementSpeed;
    }

    private void Update()
    {
        if (!_isRagdolling)
        {
            if (Input.GetMouseButtonDown(RightMouseButtonNumber))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    _agent.SetDestination(hit.point);
                    CreateMarker(hit.point);
                }
            }

            if (_activeMarker != null)
                CheckMarkerForDelete();
        }


        // Ragdolls test
        if (Input.GetKeyDown(KeyCode.Space) && !_isRagdolling)
        {
            JumpWithRagdoll();
        }

        if (_isRagdolling && Time.time > _ragdollStartTime + 1f && IsGrounded())
        {
            RecoverFromRagdoll();
        }
    }

    private void JumpWithRagdoll()
    {
        _agent.enabled = false;
        _ragdollController.Activate();

        // Применим силу вверх к центральному телу (например, chest, torso и т.д.)
        Rigidbody mainRb = _mainBody.GetComponent<Rigidbody>();
        mainRb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);

        _isRagdolling = true;
        _ragdollStartTime = Time.time;
    }

    private void RecoverFromRagdoll()
    {
        _ragdollController.Deactivate();
        _agent.enabled = true;
        _agent.ResetPath();

        _view.PlayStandUp();

        _isRagdolling = false;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(_mainBody.position, Vector3.down, _groundCheckDistance, _groundLayer);
    }

    private void CheckMarkerForDelete()
    {
        if (Vector3.Distance(transform.position, _activeMarker.transform.position) <= _agent.radius)
        {
            Destroy(_activeMarker.gameObject);
        }
    }

    private void CreateMarker(Vector3 at)
    {
        if (_activeMarker != null)
            Destroy(_activeMarker.gameObject);

        _activeMarker = Instantiate(_clickPointMarkerPrefab, at, Quaternion.identity);
    }
}
