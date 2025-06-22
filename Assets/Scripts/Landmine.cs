using System.Collections;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    [SerializeField] private BoxCollider _collisionDetector;
    [SerializeField] private float _detectAreaRange = 2;
    [SerializeField] private float _exlosionRange = 4;
    [SerializeField] private float _timeBeforeExplosion = 3;
    [SerializeField] private float _damage = 3;

    private Coroutine _activatingCoroutine;

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
        if (other.TryGetComponent(out IHealth actor) && _activatingCoroutine == null)
        {
            _activatingCoroutine = StartCoroutine(Activating());
        }
    }

    private IEnumerator Activating()
    {
        yield return new WaitForSeconds(_timeBeforeExplosion);

        Explode();
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _exlosionRange);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out IHealth actor))
            {
                actor.TakeDamage(_damage);
            }
        }

        // vfx
        gameObject.SetActive(false);
    }
}
