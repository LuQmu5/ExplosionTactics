using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private const int RightMouseButtonNumber = 1;

    [SerializeField] private PlayerView _view;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _movementSpeed = 5;

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
            }
        }

        _view.SetVelocityParam(_agent.velocity.magnitude);
    }
}
