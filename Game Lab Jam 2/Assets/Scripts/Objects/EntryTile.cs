using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryTile : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.one * 0.5f, Vector3.one * 2f);
    }
}
