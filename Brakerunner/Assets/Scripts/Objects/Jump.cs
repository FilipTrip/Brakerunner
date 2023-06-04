using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class Jump : MonoBehaviour
{
    [SerializeField] private float cooldownTime;

    private bool cooldown = true;
    private bool canJump = false;
    private bool oldJump = false;

    public bool CanJump => canJump && cooldown;

    public UnityEvent Landing = new UnityEvent();

    private void FixedUpdate()
    {
        oldJump = canJump;
        canJump = false;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (canJump)
            return;

        if (collider.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            canJump = true;

            if (oldJump == false)
            {
                Debug.Log("Landing");
                Landing.Invoke();
            }
        }
    }

    public void Cooldown()
    {
        cooldown = false;
        DelayedCall.Create(this, () => cooldown = true, cooldownTime);
    }
}
