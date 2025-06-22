using Unity.Cinemachine;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private CinemachineCamera _camera;

    private void Awake()
    {
        PlayerController player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        _camera.Target.TrackingTarget = player.transform;
    }
}