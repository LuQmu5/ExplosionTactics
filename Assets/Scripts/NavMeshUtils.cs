using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUtils
{
    public static Vector3 GetRandomPointOnNavMesh(Vector3 center, float range)
    {
        int attempts = 30;

        for (int i = 0; i < attempts; i++) 
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
            randomPoint.y = center.y;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return center;
    }
}
