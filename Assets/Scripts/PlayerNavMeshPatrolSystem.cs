using System.Collections;
using UnityEngine;

public class PlayerNavMeshPatrolSystem
{
    private readonly MonoBehaviour _actor;
    private readonly AIMover _mover;

    private Coroutine _patrolRoutine;

    public PlayerNavMeshPatrolSystem(MonoBehaviour actor, AIMover mover)
    {
        _actor = actor;
        _mover = mover;
    }

    public void StartPatrol()
    {
        if (_patrolRoutine != null)
            return;

        _patrolRoutine = _actor.StartCoroutine(Patroling());
    }

    public void StopPatrol()
    {
        if (_patrolRoutine == null)
            return;

        _actor.StopCoroutine(_patrolRoutine);
        _mover.Stop();
        _patrolRoutine = null;
    }

    private IEnumerator Patroling()
    {
        float patrolRange = 10;
        float minRange = 0.5f;

        while (true)
        {
            Vector3 randomPoint = NavMeshUtils.GetRandomPointOnNavMesh(_actor.transform.position, patrolRange);
            _mover.MoveToPoint(randomPoint);

            Debug.Log("Moving to: " + randomPoint);

            yield return new WaitUntil(() => Vector3.Distance(_actor.transform.position, randomPoint) < minRange);

            float stopDelay = Random.Range(0.5f, 2f);

            yield return new WaitForSeconds(stopDelay);
        }
    }
}
