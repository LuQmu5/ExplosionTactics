using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsView : MonoBehaviour
{
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _effectsSlider;

    private AudioHandler _audioHandler;

    public void Init(AudioHandler audioHandler)
    {
        _audioHandler = audioHandler;

        _masterSlider.onValueChanged.AddListener(OnMasterSliderValueChanged);
        _musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
        _effectsSlider.onValueChanged.AddListener(OnEffectsSliderValueChanged);
    }

    private void OnDestroy()
    {
        _masterSlider.onValueChanged.RemoveListener(OnMasterSliderValueChanged);
        _musicSlider.onValueChanged.RemoveListener(OnMusicSliderValueChanged);
        _effectsSlider.onValueChanged.RemoveListener(OnEffectsSliderValueChanged);
    }

    private void OnMasterSliderValueChanged(float value)
    {
        _audioHandler.SetMasterVolume(value);
    }

    private void OnMusicSliderValueChanged(float value)
    {
        _audioHandler.SetMusicVolume(value);
    }

    private void OnEffectsSliderValueChanged(float value)
    {
        _audioHandler.SetEffectsVolume(value);
    }
}