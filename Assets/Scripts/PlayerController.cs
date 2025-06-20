using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private const int RightMouseButtonNumber = 1;

    [SerializeField] private PlayerView _view;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private ClickPointMarker _clickPointMarkerPrefab;

    public Vector3 Velocity => _agent.velocity;

    private ClickPointMarker _activeMarker;

    private void Start()
    {
        _agent.speed = _movementSpeed;
    }

    private void Update()
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
