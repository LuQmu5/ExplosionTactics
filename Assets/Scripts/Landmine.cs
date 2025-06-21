using UnityEngine;

public class Landmine : MonoBehaviour
{

    [SerializeField] private BoxCollider _collisionDetector;
    [SerializeField] private float _detectAreaRange = 2;
    [SerializeField] private float _explosionRadius = 4;
    [SerializeField] private float _explosionForce = 10;

    private void OnValidate()
    {
        _collisionDetector.size = Vector3.one * _detectAreaRange;
    }

    private void Awake()
    {
        _collisionDetector.size = Vector3.one * _detectAreaRange;
        _collisionDetector.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.TryGetComponent(out RagdollController ragdoll))
        {
            ragdoll.Activate();
            ragdoll.Pelvis.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, _explosionForce / 2, ForceMode.Impulse);

            Destroy(gameObject);
        }
    }
}
