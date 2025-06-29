using System;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationNames
{
    Idle = 0,
    Run = 1,
    Hit = 2,
}


[RequireComponent(typeof(Animator))] 
public class PlayerView : MonoBehaviour
{
    private const string Velocity = nameof(Velocity);
    private const string Hit = nameof(Hit);
    private const string HitSpeedMultiplier = nameof(HitSpeedMultiplier);

    [SerializeField] private PlayerController _player;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _hitSpeedMultiplier = 2;
    [SerializeField] private float _idleSpeedMultiplier = 0.5f;

    private Dictionary<AnimationNames, float> _animationMultipliersMap;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat(HitSpeedMultiplier, _hitSpeedMultiplier);

        _animationMultipliersMap = new Dictionary<AnimationNames, float>()
        {
            [AnimationNames.Hit] = _hitSpeedMultiplier,
            [AnimationNames.Idle] = _idleSpeedMultiplier,
        };

    }

    public void SetVelocity(float value)
    {
        _animator.SetFloat(Velocity, value);
    }

    public void SetHitTrigger()
    {
        _animator.SetTrigger(AnimationNames.Hit.ToString());
    }

    public void SetHealthPercentParam(float value)
    {
        _animator.SetFloat("HealthPercent", value);
    }

    public float GetAnimationClipLength(AnimationNames clipName)
    {
        RuntimeAnimatorController ac = _animator.runtimeAnimatorController;

        foreach (var clip in ac.animationClips)
        {
            if (clip.name == clipName.ToString())
            {
                float defaultMultiplier = 1f;
                float multiplier = _animationMultipliersMap.ContainsKey(clipName) ? _animationMultipliersMap[clipName] : defaultMultiplier;
                float totalLength = clip.length / multiplier;

                /*
                Debug.Log(clip.name);
                Debug.Log(clip.length);
                Debug.Log(totalLength);
                */

                return totalLength;
            }
        }

        Debug.LogWarning($"Анимация с именем '{clipName}' не найдена.");
        return 0f;
    }

    public void Deactivate()
    {
        _animator.enabled = false;
    }

    public void Activate()
    {
        _animator.enabled = true;
    }
}
