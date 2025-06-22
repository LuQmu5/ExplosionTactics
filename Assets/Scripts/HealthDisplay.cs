using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private HealthSystem _healthActor;
    [SerializeField] private Image _filledImage;

    private Vector3 _offset;

    private void OnEnable()
    {
        _offset = transform.position;

        _healthActor.HealthChanged += OnHealthChanged;
        _healthActor.Died += OnDied;
    }


    private void OnDisable()
    {
        _healthActor.HealthChanged -= OnHealthChanged;
    }

    private void LateUpdate()
    {
        if (_pivot != null)
        {
            transform.position = _pivot.position + _offset;
            transform.forward = Camera.main.transform.forward;
        }
    }

    private void OnHealthChanged(float current, float max)
    {
        _filledImage.fillAmount = current / max;
    }

    private void OnDied(Vector3? forceOrigin, float force)
    {
        gameObject.SetActive(false);
    }
}