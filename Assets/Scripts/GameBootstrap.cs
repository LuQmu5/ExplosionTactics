using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Audio;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private CinemachineCamera _camera;

    [SerializeField] private SoundSettingsView _soundSettingsView;
    [SerializeField] private AudioMixer _audioMixer;

    private void Awake()
    {
        PlayerController player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        _camera.Target.TrackingTarget = player.transform;

        AudioHandler audioHandler = new AudioHandler(_audioMixer);
        _soundSettingsView.Init(audioHandler);
    }
}
