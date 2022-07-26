using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("Volume Settings")]
    public AudioSource musicSource;
    public Slider volumeSlider;
    public TextMeshProUGUI sliderValue;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float localVolume = PlayerPrefs.GetFloat("masterVolume");

            sliderValue.text = localVolume.ToString("0.0");
            volumeSlider.value = localVolume;
            musicSource.volume = localVolume;
        }
    }
}
