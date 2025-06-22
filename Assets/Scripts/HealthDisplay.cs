using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private PlayerController _playerHealth;
    [SerializeField] private Image _filledImage;

    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position;

        _playerHealth.Changed += OnHealthChanged;
    }

    private void OnDestroy()
    {
        _playerHealth.Changed -= OnHealthChanged;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            transform.position = _target.position + _offset;
            transform.forward = Camera.main.transform.forward;
        }
    }

    private void OnHealthChanged(float current, float max)
    {
        if (current == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        _filledImage.fillAmount = current / max;
    }
}