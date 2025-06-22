using System;
using UnityEngine;
using UnityEngine.AI;

public class AIMover
{
    private NavMeshAgent _agent;

    public Vector3 Velocity => _agent.velocity;

    public AIMover(NavMeshAgent agent, float speed)
    {
        _agent = agent;
        _agent.speed = speed;
    }

    public void MoveToPoint(Vector3 point)
    {
        _agent.isStopped = false;
        _agent.SetDestination(point);
    }

    public void Stop()
    {
        _agent.isStopped = true;
    }
}
