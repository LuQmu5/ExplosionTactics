using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Rigidbody[] _ragdollBodies;
    [SerializeField] private Animator _animator;

    private bool _isActive;

    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _ragdollBodies = GetComponentsInChildren<Rigidbody>();
        Deactivate();
    }

    public void Activate(Vector3 direction, float force)
    {
        if (_isActive)
            return;

        _animator.enabled = false;

        foreach (var body in _ragdollBodies)
        {
            body.isKinematic = false;
            body.AddForce(direction * force, ForceMode.Impulse);
        }

        _isActive = true;
    }

    public void Deactivate()
    {
        foreach (var body in _ragdollBodies)
        {
            body.isKinematic = true;
        }

        if (_animator != null)
            _animator.enabled = true;

        _isActive = false;
    }
}
