using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LandmineView : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource _audioSource;

    [Space(20)]
    [Header("Settings")]
    [SerializeField] private ParticleSystem _explosionVFX;
    [SerializeField] private AudioClip _explosionSFX;
    [SerializeField] private AudioClip _idleSFX;
    [SerializeField] private AudioClip _activatingSFX;

    public void PlayExplosionEffect()
    {
        _audioSource.Stop();
        _audioSource.clip = null;

        _audioSource.PlayOneShot(_explosionSFX);
        _explosionVFX.Play();
    }

    public void SetIdleSound()
    {
        _audioSource.clip = _idleSFX;
        _audioSource.Play();
    }

    public void SetActivatingSound()
    {
        _audioSource.clip = _activatingSFX;
        _audioSource.Play();
    }

    internal float GetExplosionSoundLength()
    {
        return _explosionSFX.length;
    }
}