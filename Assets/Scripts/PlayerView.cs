using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerView : MonoBehaviour
{
    private static readonly int Velocity = Animator.StringToHash("Velocity");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int HealthPercent = Animator.StringToHash("HealthPercent");

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        if (_animator == null)
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

    public void SetJumpingState(bool isJumping)
    {
        _animator.SetBool(IsJumping, isJumping);
    }

    public void SetHealthPercentParam(float value)
    {
        _animator.SetFloat(HealthPercent, value);
    }

    public float GetAnimationClipLength(string clipName)
    {
        var controller = _animator.runtimeAnimatorController;
        if (controller == null) return 0f;

        foreach (var animClip in controller.animationClips)
        {
            if (animClip.name.Equals(clipName))
            {
                return animClip.length;
            }
        }

        Debug.LogWarning($"Animation clip '{clipName}' not found.");
        return 0f;
    }

    public void Deactivate() => _animator.enabled = false;
    public void Activate() => _animator.enabled = true;
}
