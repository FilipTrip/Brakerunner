using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Jump jump;
    [SerializeField] private float jumpForce;
    [SerializeField] private float duckForce;
    [SerializeField] private float speed;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedAcceleration;
    [SerializeField] private float minSpeedAcceleration;
    [SerializeField] private Transform cameraParent;
    [SerializeField] private float duckDuration;

    private float duckTimer;
    private bool dead;

    public float Speed => speed;
    public Transform CameraParent => cameraParent;

    public float GetSpeedPercentage()
    {
        return speed.Remapped(minSpeed, maxSpeed, 0f, 1f);
    }

    private void Awake()
    {
        Instance = this;
        speed = 0f;
        maxSpeed = minSpeed * 2f;
        dead = true; // So to not update
        DelayedCall.Create(this, Delayed_Start, 2f);
    }

    private void Delayed_Start()
    {
        dead = false;
        rigidbody.velocity = new Vector2(1, 0); // So to not die first frame
        speed = minSpeed;
        playerAnimator.Run();
    }

    private void Update()
    {
        if (dead)
            return;

        // Check if dead

        if (rigidbody.velocity.x <= 0f || rigidbody.velocity.y < -50f)
        {
            Debug.Log("Death velocity: " + rigidbody.velocity);
            Die();
            return;
        }

        // Update duck timer

        if (duckTimer > 0f)
        {
            duckTimer -= Time.deltaTime;

            if (duckTimer <= 0f)
                StopDuck();
        }

        // Increase min and max speed
        minSpeed += minSpeedAcceleration * Time.deltaTime;
        maxSpeed = minSpeed * 2f;

        // Increase and clamp speed
        speed += speedAcceleration * Time.deltaTime;
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Jumping");
            Jump();
        }

        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Ducking");
            StartDuck();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Shooting");
            Shoot();
        }

        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Braking");
            Brake();
        }
    }

    private void FixedUpdate()
    {
        if (dead)
            return;

        rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
    }

    private void Jump()
    {
        if (jump.CanJump)
        {
            if (duckTimer > 0f)
                StopDuck();

            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            playerAnimator.Jump();
            jump.Landing.AddListener(playerAnimator.Run);
        }
    }

    private void StartDuck()
    {
        if (duckTimer != 0f)
            return;

        duckTimer = duckDuration;
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, -duckForce);
        playerAnimator.Duck();
        jump.Landing.RemoveListener(playerAnimator.Run);
    }

    private void StopDuck()
    {
        duckTimer = 0f;
        playerAnimator.Run();
    }

    private void Shoot()
    {

    }

    private void Brake()
    {
        speed = minSpeed;
    }

    private void Die()
    {
        dead = true;
        GameManager.Instance.StopRun();
        SceneTransitioner.Instance.FadeToScene("End");
    }

}
