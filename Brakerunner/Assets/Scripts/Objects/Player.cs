using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Jump jump;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float duckForce;
    [SerializeField] private float speed;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedAcceleration;
    [SerializeField] private float minSpeedAcceleration;
    [SerializeField] private Transform cameraParent;
    [SerializeField] private float duckDuration;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootDuration;
    [SerializeField] private GameObject explosionPrefab;

    private float duckTimer;
    private bool dead;
    private float shootTimer;

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
            Explode();
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
        playerAnimator.UpdateRunSpeed(speed);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Jumping");
            Jump();
        }

        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Ducking");
            StartDuck();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Shooting");
            StartCoroutine(Coroutine_Shoot());
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

            StopShoot();
            StartCoroutine(Coroutine_Jump());
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            playerAnimator.Jump();
            jump.Landing.AddListener(playerAnimator.Run);
            jump.Landing.AddListener(StopJump);
        }
    }

    private void StartDuck()
    {
        if (duckTimer != 0f)
            return;

        StopJump();
        StopShoot();

        duckTimer = duckDuration;
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, -duckForce);
        playerAnimator.Duck();
    }

    private void StopDuck()
    {
        duckTimer = 0f;
        playerAnimator.Run();
    }

    private void Brake()
    {
        speed = minSpeed;
    }

    public void Die()
    {
        dead = true;
        GameManager.Instance.StopRun();
        DelayedCall.Create(this, () => SceneTransitioner.Instance.FadeToScene("End"), 1f);
    }

    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position + new Vector3(0f, 1.5f, 0f), Quaternion.identity, null);
        playerAnimator.HideAll();
        rigidbody.velocity = Vector2.zero;
        rigidbody.gravityScale = 0f;
        Die();
    }

    private IEnumerator Coroutine_Jump()
    {
        float jumpTimer = 0f;

        while (jumpTimer < jumpDuration && duckTimer == 0)
        {
            jumpTimer += Time.deltaTime;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpCurve.Evaluate(jumpTimer/jumpDuration));
            yield return null;
        }

        rigidbody.gravityScale = 1f;
    }

    private void StopJump()
    {
        StopCoroutine(Coroutine_Jump());
        rigidbody.gravityScale = 1f;
        jump.Landing.RemoveListener(playerAnimator.Run);
    }

    private IEnumerator Coroutine_Shoot()
    {
        if (shootTimer != 0f)
            yield break;

        StopJump();
        StopDuck();

        shootTimer = shootDuration;
        playerAnimator.Shoot();
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity, null).GetComponent<Bullet>();
        bullet.AddVelocity(new Vector2(rigidbody.velocity.x, 0f));

        yield return new WaitForSeconds(shootDuration);
        shootTimer = 0f;
        playerAnimator.Run();
    }

    private void StopShoot()
    {
        StopCoroutine(Coroutine_Shoot());
        shootTimer = 0f;
        playerAnimator.Run();
    }

}
