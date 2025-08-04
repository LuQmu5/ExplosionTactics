using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement
{
    private NavMeshAgent _agent;

    public Vector3 Velocity => _agent.velocity;

    public PlayerMovement(NavMeshAgent agent, float speed)
    {
        _agent = agent;
        _agent.speed = speed;
    }

    public void MoveTo(Vector3 point)
    {
        _agent.SetDestination(point);
    }

    public void Stop()
    {
        _agent.ResetPath();
    }
}
