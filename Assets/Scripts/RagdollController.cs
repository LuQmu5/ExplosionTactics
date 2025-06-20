using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Transform _root;
    [SerializeField] private Animator _animator;

    private Rigidbody[] _rigidbodies;

    private void Awake()
    {
        _rigidbodies = _root.GetComponentsInChildren<Rigidbody>();

        Deactivate();
    }

    public void Activate()
    {
        _animator.enabled = false;

        foreach (Rigidbody rb in _rigidbodies)
        {
            rb.isKinematic = false;
        }
    }

    public void Deactivate()
    {
        foreach (Rigidbody rb in _rigidbodies)
        {
            rb.isKinematic = true;
        }

        _animator.enabled = true;
    }
}