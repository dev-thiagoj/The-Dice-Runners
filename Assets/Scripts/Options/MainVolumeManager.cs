using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainVolumeManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource clickSource;
    public Slider volumeSlider;
    public TextMeshProUGUI sliderValue;

    private void Update()
    {
        ShowSliderValue();
        SetMainVolume();
    }

    public void ShowSliderValue()
    {
        sliderValue.text = volumeSlider.value.ToString("0.0");
    }

    public void SetMainVolume()
    {
        musicSource.volume = volumeSlider.value;
        clickSource.volume = volumeSlider.value;
        SFXPool.Instance.poolVolume = volumeSlider.value;
    }

    public void VolumeAplly()
    {
        PlayerPrefs.SetFloat("masterVolume", musicSource.volume);
    }
}
