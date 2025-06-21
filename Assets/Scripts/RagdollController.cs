using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Transform _root;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _pelvis;

    private Rigidbody[] _rigidbodies;

    private Dictionary<Transform, Vector3> _baseLocalRotations;
    private Dictionary<Transform, Vector3> _baseLocalPositions;

    public Rigidbody Pelvis => _pelvis;

    private void Awake()
    {
        _rigidbodies = _root.GetComponentsInChildren<Rigidbody>();
        SetBaseTransforms();

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

            ResetBaseTransformFor(rb);
        }

        _animator.enabled = true;
    }

    private void ResetBaseTransformFor(Rigidbody rb)
    {
        rb.transform.localEulerAngles = _baseLocalRotations[rb.transform];
        rb.transform.localPosition = _baseLocalPositions[rb.transform];
    }

    private void SetBaseTransforms()
    {
        _baseLocalPositions = new Dictionary<Transform, Vector3>();
        _baseLocalRotations = new Dictionary<Transform, Vector3>();

        foreach (Rigidbody rb in _rigidbodies)
        {
            _baseLocalPositions.Add(rb.transform, rb.transform.localPosition);
            _baseLocalRotations.Add(rb.transform, rb.transform.localEulerAngles);
        }
    }
}