using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    private bool broken = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Glass hit");

        if (broken)
            return;

        if (collision.collider.tag == "Bullet")
            Break();
    }

    public void Break()
    {
        if (broken)
            return;

        broken = true;
        Destroy(gameObject);

        Collider2D collider;
        Glass glass;

        if (collider = Physics2D.OverlapPoint(transform.position + new Vector3(0, 1, 0)))
        {
            if (glass = collider.GetComponent<Glass>())
                glass.Break();
        }

        if (collider = Physics2D.OverlapPoint(transform.position + new Vector3(0, -1, 0)))
        {
            if (glass = collider.GetComponent<Glass>())
                glass.Break();
        }

        if (collider = Physics2D.OverlapPoint(transform.position + new Vector3(1, 0, 0)))
        {
            if (glass = collider.GetComponent<Glass>())
                glass.Break();
        }

        if (collider = Physics2D.OverlapPoint(transform.position + new Vector3(-1, 0, 0)))
        {
            if (glass = collider.GetComponent<Glass>())
                glass.Break();
        }
    }
}
