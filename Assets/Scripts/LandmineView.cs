using NUnit.Framework;
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

    public void PlayExplosionEffect()
    {
        _audioSource.PlayOneShot(_explosionSFX);
        _explosionVFX.Play();
    }
}