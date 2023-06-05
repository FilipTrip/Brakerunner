using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class Explosion : MonoBehaviour
{
    [SerializeField] private ShakeData shakeData;

    private void Start()
    {
        Destroy(gameObject, 4f);

        if (SettingsMenu.CameraShake)
            CameraShakerHandler.Shake(shakeData);
    }

}
