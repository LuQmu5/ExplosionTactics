using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Transform _root;

    private Rigidbody[] _rigidRagdolParts;

    private Dictionary<Rigidbody, Vector3> _baseLocalRotations;
    private Dictionary<Rigidbody, Vector3> _baseLocalPositions;

    private void Awake()
    {
        _rigidRagdolParts = _root.GetComponentsInChildren<Rigidbody>();
        SaveBaseTransforms();
    }

    public void Activate()
    {
        foreach (Rigidbody rigidbody in _rigidRagdolParts)
        {
            rigidbody.isKinematic = false;
        }
    }

    public void Deactivate()
    {
        foreach (Rigidbody rigidbody in _rigidRagdolParts)
        {
            rigidbody.isKinematic = true;

            ResetBaseTransformFor(rigidbody);
        }
    }

    private void ResetBaseTransformFor(Rigidbody rigidbody)
    {
        rigidbody.transform.localEulerAngles = _baseLocalRotations[rigidbody];
        rigidbody.transform.localPosition = _baseLocalPositions[rigidbody];
    }

    private void SaveBaseTransforms()
    {
        _baseLocalPositions = new Dictionary<Rigidbody, Vector3>();
        _baseLocalRotations = new Dictionary<Rigidbody, Vector3>();

        foreach (Rigidbody rigidbody in _rigidRagdolParts)
        {
            _baseLocalPositions.Add(rigidbody, rigidbody.transform.localPosition);
            _baseLocalRotations.Add(rigidbody, rigidbody.transform.localEulerAngles);
        }
    }

    public void AplyForce(Vector3 direction, float force)
    {
        foreach (var rigidbody in _rigidRagdolParts)
        {
            rigidbody.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}