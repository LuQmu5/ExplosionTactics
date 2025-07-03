using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleWindowButton : MonoBehaviour
{
    [SerializeField] private GameObject _window;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(ToggleWindow);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(ToggleWindow);
    }

    private void ToggleWindow()
    {
        _window.gameObject.SetActive(!_window.gameObject.activeSelf);
    }
}
