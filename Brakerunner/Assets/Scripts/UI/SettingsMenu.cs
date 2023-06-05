using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Button musicButton;
    [SerializeField] private Button cameraShakeButton;

    private static bool cameraShake = true;
    public static bool CameraShake => cameraShake;

    private void Start()
    {
        musicButton.GetComponentInChildren<TextMeshProUGUI>().text = "Music: " + (int)(MusicManager.Volume * 100 + 0.01f);
        cameraShakeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Camera Shake: " + (cameraShake ? "On" : "Off");
    }

    public void OnClick_MusicButton()
    {
        TextMeshProUGUI text = musicButton.GetComponentInChildren<TextMeshProUGUI>();
        int volume = int.Parse(text.text.Replace("Music: ", ""));
        volume -= 20;

        if (volume == -20)
            volume = 100;

        text.text = "Music: " + volume;
        MusicManager.Instance.SetVolume(volume * 0.01f);
    }

    public void OnClick_CameraShake()
    {
        TextMeshProUGUI text = cameraShakeButton.GetComponentInChildren<TextMeshProUGUI>();

        if (text.text == "Camera Shake: On")
        {
            text.text = "Camera Shake: Off";
            cameraShake = false;
        }
        else
        {
            text.text = "Camera Shake: On";
            cameraShake = true;
        }
            
    }

}
