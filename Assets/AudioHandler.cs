using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler
{
    private const float OffVolumeValue = -80;
    private const float OnVolumeValue = 0;

    private const string MusicKey = "MusicVolume";
    private const string EffectsKey = "EffectsVolume";
    private const string MasterKey = "MasterVolume";

    private AudioMixer _audioMixer;

    public AudioHandler(AudioMixer audioMixer)
    {
        _audioMixer = audioMixer;
    }

    public bool IsMusiocOn() => IsVolumeOn(MusicKey);
    public bool IsEffectsOn() => IsVolumeOn(EffectsKey);

    public void OffMusic() => OffVolume(MusicKey);
    public void OffEffects() => OffVolume(EffectsKey);

    public void OnMusic() => OnVolume(MusicKey);
    public void OnEffects() => OnVolume(EffectsKey);

    public void SetMasterVolume(float value)
    {
        value = OnVolumeValue + OffVolumeValue * (1 - value);
        _audioMixer.SetFloat(MasterKey, value);
    }

    public void SetMusicVolume(float value)
    {
        value = OnVolumeValue + OffVolumeValue * (1 - value);
        _audioMixer.SetFloat(MusicKey, value);
    }

    public void SetEffectsVolume(float value)
    {
        value = OnVolumeValue + OffVolumeValue * (1 - value);
        _audioMixer.SetFloat(EffectsKey, value);
    }

    private bool IsVolumeOn(string key)
    {
        return _audioMixer.GetFloat(key, out float value) && Mathf.Abs(value - OnVolumeValue) <= 0.01f;
    }

    private void OnVolume(string key)
    {
        _audioMixer.SetFloat(key, OnVolumeValue);
    }

    private void OffVolume(string key)
    {
        _audioMixer.SetFloat(key, OffVolumeValue);
    }
}
