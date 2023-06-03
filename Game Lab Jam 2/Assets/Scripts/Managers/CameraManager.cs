using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float maxDistance;

    private void Start()
    {
        transform.position = Player.Instance.CameraParent.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = Player.Instance.CameraParent.position;

        float x = targetPosition.x;
        float y = Mathf.Clamp(transform.position.y, targetPosition.y - maxDistance, targetPosition.y + maxDistance);
        float z = transform.position.z;

        y += (targetPosition.y - y) * Time.deltaTime;

        if (Mathf.Abs(targetPosition.y - y) < 0.001f)
            y = targetPosition.y;

        transform.position = new Vector3(x, y, z);
    }

}
