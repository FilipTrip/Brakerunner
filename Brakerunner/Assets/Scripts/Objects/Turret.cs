using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject idleAnimation;
    [SerializeField] private GameObject shootAnimation;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private float detectionRange;
    [SerializeField] private float fireRange;
    [SerializeField] private float speedPercentageThreshold;
    [SerializeField] private AudioSource audioSource;

    private enum State { Idle, Ready, Shoot, Clear };
    private State state;

    private void Start()
    {
        lineRenderer.enabled = false;
        lineRenderer.SetPosition(0, firePoint.position);
    }

    private void Update()
    {
        // Wait to flip scale X
        if (transform.localScale.x > 0 && Player.Instance.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lineRenderer.SetPosition(0, firePoint.position);
            detectionRange *= 0.5f;
            fireRange *= 0.5f;
        }

        // If State Idle
        if (state == State.Idle)
        {
            if (Mathf.Abs(Player.Instance.transform.position.x - transform.position.x) < detectionRange)
            {
                state = State.Ready;
                idleAnimation.SetActive(false);
                shootAnimation.SetActive(true);
                DelayedCall.Create(this, () => lineRenderer.enabled = true, 0.3f);
            }
        }

        // If State Ready
        else if (state == State.Ready)
        {
            // Aim at player
            lineRenderer.SetPosition(1, Player.Instance.transform.position + new Vector3(0f, 1.8f, 0f));

            // Of out of range
            if (Player.Instance.transform.position.x > transform.position.x + fireRange)
            {
                state = State.Clear;
                lineRenderer.enabled = false;
            }

            // In range
            else if (Player.Instance.transform.position.x > transform.position.x - fireRange)
            {
                // If player too fast when passing
                if (Player.Instance.GetSpeedPercentage() >= speedPercentageThreshold)
                {
                    state = State.Shoot;
                    StartCoroutine(Coroutine_Shoot());
                }
            }
        }

        // If State Shoot
        else if (state == State.Shoot)
        {
            // Aim at player
            lineRenderer.SetPosition(1, Player.Instance.transform.position + new Vector3(0f, 1.8f, 0f));
        }

        // If State Clear
        else if (state == State.Clear)
        {

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange);
    }

    private IEnumerator Coroutine_Shoot()
    {
        audioSource.Play();
        yield return new WaitForSeconds(0.3f);
        lineRenderer.enabled = false;
        Player.Instance.Explode();
    }
}
