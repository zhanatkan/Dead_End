using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioMixerGroup mixer;
    [SerializeField] private Slider slider;

    private const string VolumePrefKey = "SoundVolume";

    void Start()
    {
        LoadVolume();
    }

    private void OnEnable()
    {
        slider.onValueChanged.AddListener(SliderMusic);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(SliderMusic);
    }

    public void SliderMusic(float volume)
    {
        mixer.audioMixer.SetFloat("SoundVolume", Mathf.Lerp(-80, 0, volume));
        SaveVolume(volume);
    }

    private void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.5f); // Значение по умолчанию - 0.5
        slider.value = savedVolume;
        mixer.audioMixer.SetFloat("SoundVolume", Mathf.Lerp(-80, 0, savedVolume));
    }
}
