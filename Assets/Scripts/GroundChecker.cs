using UnityEngine;

public class GroundChecker : MonoBehaviour
{

    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Transform _legs;
    [SerializeField] private float _legsRadius = 0.5f;
}