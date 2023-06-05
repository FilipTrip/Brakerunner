using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AudioClip audioClip;

    private bool broken = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (broken)
            return;

        if (collision.collider.tag == "Bullet")
            Break(true);
    }

    public void Break(bool directHit)
    {
        if (broken)
            return;

        if (directHit)
            SoundManager.Instance.PlayDummy(transform, audioClip, 0.8f, 0.5f);

        broken = true;
        particleSystem.Play();
        particleSystem.transform.parent = null;
        Destroy(gameObject);

        Collider2D collider;
        Glass glass;

        if (collider = Physics2D.OverlapPoint(transform.position + new Vector3(0, 1, 0)))
        {
            if (glass = collider.GetComponent<Glass>())
                glass.Break(false);
        }

        if (collider = Physics2D.OverlapPoint(transform.position + new Vector3(0, -1, 0)))
        {
            if (glass = collider.GetComponent<Glass>())
                glass.Break(false);
        }

        if (collider = Physics2D.OverlapPoint(transform.position + new Vector3(1, 0, 0)))
        {
            if (glass = collider.GetComponent<Glass>())
                glass.Break(false);
        }

        if (collider = Physics2D.OverlapPoint(transform.position + new Vector3(-1, 0, 0)))
        {
            if (glass = collider.GetComponent<Glass>())
                glass.Break(false);
        }
    }
}
