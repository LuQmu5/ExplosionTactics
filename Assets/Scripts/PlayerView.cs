using UnityEngine;

[RequireComponent(typeof(Animator))] 
public class PlayerView : MonoBehaviour
{
    private const string Velocity = nameof(Velocity);

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetVelocityParam(float value)
    {
        _animator.SetFloat(Velocity, value);
    }
}
