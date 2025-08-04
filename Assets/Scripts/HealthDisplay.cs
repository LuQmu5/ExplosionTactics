using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private Image _filledImage;

    private HealthSystem _healthActor;

    private Vector3 _offset;

    public void Init(HealthSystem healthActor)
    {
        _healthActor = healthActor;
        _offset = transform.position;

        _healthActor.HealthChanged += OnHealthChanged;
        _healthActor.Died += OnDied;
    }


    private void OnDestroy()
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