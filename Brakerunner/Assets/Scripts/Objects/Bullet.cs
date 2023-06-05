using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private ParticleSystem particleSystem;

    private bool hit = false;

    private void Start()
    {
        Destroy(gameObject, 2f);
        AddVelocity(velocity);
    }

    public void AddVelocity(Vector2 velocity)
    {
        rigidbody.velocity += velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hit)
            return;

        hit = true;
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        particleSystem.transform.parent = null;
        Destroy(particleSystem.gameObject, 3f);

        Instantiate(explosionPrefab, transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }

}
