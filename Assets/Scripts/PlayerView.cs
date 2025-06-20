using UnityEngine;

[RequireComponent(typeof(Animator))] 
public class PlayerView : MonoBehaviour
{
    private const string Velocity = nameof(Velocity);

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
        _animator.Play("StandUp"); // убедись, что у тебя есть такая анимация
    }

}
