using UnityEngine;

[RequireComponent(typeof(Animator))] 
public class PlayerView : MonoBehaviour
{
    private const string Velocity = nameof(Velocity);
    private const string StandUpAnimationName = "StandUp";

    [SerializeField] private PlayerController _player;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat(Velocity, _player.Velocity.magnitude);
    }

    public void PlayStandUp()
    {
        _animator.Play(StandUpAnimationName);
    }
}
