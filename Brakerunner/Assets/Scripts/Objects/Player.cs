using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Transform cameraParent;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Jump jump;

    [Header("Prefabs")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject explosionPrefab;

    [Header("Values")]
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float duckForce;
    [SerializeField] private float speed;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedAcceleration;
    [SerializeField] private float minSpeedAcceleration;
    [SerializeField] private float duckDuration;
    [SerializeField] private float shootDuration;

    [Header("Sounds")]
    [SerializeField] private SoundData jumpSound;
    [SerializeField] private SoundData duckSound;
    [SerializeField] private SoundData brakeSound;
    [SerializeField] private SoundData landSound;

    private bool slam;
    private bool dead;
    private float duckTimer;
    private float shootTimer;
    private Coroutine jumpCoroutine;
    private AudioSource duckSoundDummy;
    private List<Vector2> oldVelocity = new List<Vector2>();

    public bool Slam => slam;
    public float Speed => speed;
    public Transform CameraParent => cameraParent;

    public float GetSpeedPercentage()
    {
        return speed.Remapped(minSpeed, maxSpeed, 0f, 1f);
    }

    // Start

    private void Awake()
    {
        Instance = this;
        speed = 0f;
        maxSpeed = minSpeed * 2f;
        dead = true; // Disables Update()

        StartCoroutine(Coroutine_Start());
    }

    private IEnumerator Coroutine_Start()
    {
        yield return new WaitForSeconds(1.5f);

        playerAnimator.Run();
        speed = minSpeed;

        while (GetSpeedPercentage() < 0.5f)
        {
            speed += Time.deltaTime * 8f;
            rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
            playerAnimator.UpdateRunSpeed(speed);
            yield return null;
        }

        dead = false; // Enables Update()
        rigidbody.velocity = Vector2.right; // So to not die first frame, will be instantly overwritten
    }

    // Update

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

        // Update timers

        if (duckTimer > 0f)
        {
            if ((duckTimer -= Time.deltaTime) <= 0f)
            {
                duckTimer = 0f;
                StopDuck();
            } 
        }

        if (shootTimer > 0f)
        {
            if ((shootTimer -= Time.deltaTime) <= 0f)
            {
                shootTimer = 0f;
                StopShoot();
            }
        }

        // Increase min and max speed
        minSpeed += minSpeedAcceleration * Time.deltaTime;
        maxSpeed = minSpeed * 2f;

        // Increase and clamp speed
        speed += speedAcceleration * Time.deltaTime;
        speed = Mathf.Min(speed, maxSpeed);
        playerAnimator.UpdateRunSpeed(speed);

        // Start actions

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Jump();
        }

        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Duck();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Shoot();
        }

        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Brake();
        }
    }

    private void FixedUpdate()
    {
        if (dead)
            return;

        rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);

        oldVelocity.Add(rigidbody.velocity);
        if (oldVelocity.Count > 5)
            oldVelocity.RemoveAt(0);
    }

    // Jump

    private void Jump()
    {
        if (!jump.CanJump)
            return;

        // Stop other actions
        StopDuck();
        StopShoot();

        // Jump
        jump.Cooldown();
        playerAnimator.Jump();
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        SoundManager.Instance.PlayDummy(transform, jumpSound);
        jumpCoroutine = StartCoroutine(Coroutine_Jump());

        // Prepare landing
        jump.Landing.AddListener(StopJump);
    }

    private IEnumerator Coroutine_Jump()
    {
        // Perform the jump

        float jumpTimer = 0f;

        while (jumpTimer < jumpDuration && duckTimer == 0)
        {
            jumpTimer += Time.deltaTime;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpCurve.Evaluate(jumpTimer / jumpDuration));
            yield return null;
        }

        rigidbody.gravityScale = 1f;
    }

    private void StopJump()
    {
        // Is called when landing, or when jump is interrupted by another action

        // Stop performing jump, and stop waiting for landing (in case this method was called to interrupt)
        if (jumpCoroutine != null)
            StopCoroutine(jumpCoroutine);

        jump.Landing.RemoveListener(StopJump);
        rigidbody.gravityScale = 1f;

        // Default back to running animation
        playerAnimator.StopJump();
    }

    // Duck

    private void Duck()
    {
        if (duckTimer != 0f)
            return;

        // Stop other actions
        StopJump();
        StopShoot();

        // Start duck
        duckTimer = duckDuration;
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, -duckForce);
        playerAnimator.Duck();

        if (!jump.OnGround)
        {
            slam = true;
            SoundManager.Instance.PlayDummy(transform, jumpSound);
            jump.Landing.AddListener(SlamLand);
        }
        else
        {
            duckSoundDummy = SoundManager.Instance.PlayDummy(transform, duckSound);
        }
    }

    private void StopDuck()
    {
        // Is called when duckTimer reaches zero, or when duck is interrupted by another action

        // Stop duck (in case this method was called to interrupt)
        slam = false;
        duckTimer = 0f;
        SoundManager.Instance.FadeOut(duckSoundDummy, 0.2f);

        // Default back to running animation
        playerAnimator.StopDuck();
    }

    private void SlamLand()
    {
        // Is called when landing after a duck. Removes itself as a landing listener

        if (jump.Collider.tag == "Glass")
        {
            jump.Collider.GetComponent<Glass>().Break(true);
            jump.ContinueJump();
            rigidbody.velocity = oldVelocity[3];
        }

        else
        {
            jump.Landing.RemoveListener(SlamLand);
            SoundManager.Instance.PlayDummy(transform, landSound);
            duckSoundDummy = SoundManager.Instance.PlayDummy(transform, duckSound);
            slam = false;
        }
    }

    // Shoot

    private void Shoot()
    {
        if (shootTimer != 0f)
            return;

        // Stop duck (not jump)
        StopDuck();

        // Start shoot
        shootTimer = shootDuration;
        playerAnimator.Shoot();
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity, null).GetComponent<Bullet>();
        bullet.AddVelocity(new Vector2(rigidbody.velocity.x, 0f));
    }

    private void StopShoot()
    {
        // Is called when shootTimer reaches zero, or when shoot is interrupted by another action

        // Defalt back to running animation
        playerAnimator.StopShoot();
    }

    // Brake

    private void Brake()
    {
        if (!jump.OnGround)
            return;

        speed = minSpeed;
        SoundManager.Instance.PlayDummy(transform, brakeSound);
    }

    // Die

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
        rigidbody.simulated = false;
        Die();
    }

}
