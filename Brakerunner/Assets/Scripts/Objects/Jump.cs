using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class Jump : MonoBehaviour
{
    [SerializeField] private float cooldownTime;

    private bool cooldown = true;
    private bool onGround = false;
    private bool oldOnGround = false;

    public bool OnGround => onGround;
    public bool CanJump => onGround && cooldown;

    public UnityEvent Landing = new UnityEvent();

    private void FixedUpdate()
    {
        oldOnGround = onGround;
        onGround = false;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (onGround)
            return;

        if (collider.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            onGround = true;

            if (oldOnGround == false)
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
