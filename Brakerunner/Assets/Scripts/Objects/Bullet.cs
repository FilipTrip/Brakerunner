using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Vector2 velocity;

    private void Start()
    {
        AddVelocity(velocity);
    }

    public void AddVelocity(Vector2 velocity)
    {
        rigidbody.velocity += velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet hit");
        Destroy(gameObject);
    }

}
