using UnityEngine;

[RequireComponent(typeof(Animator))] 
public class PlayerView : MonoBehaviour
{
    private const string Velocity = nameof(Velocity);
    private const string Hit = nameof(Hit);

    [SerializeField] private PlayerController _player;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetVelocity(float value)
    {
        _animator.SetFloat(Velocity, value);
    }

    public void SetHitTrigger()
    {
        _animator.SetTrigger(Hit);
    }
}
