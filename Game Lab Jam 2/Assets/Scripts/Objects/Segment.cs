using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Segment : MonoBehaviour
{
    [SerializeField] private EntryTile entryTile;
    [SerializeField] private ExitTile exitTile;

    public EntryTile EntryTile => entryTile;
    public ExitTile ExitTile => exitTile;

    public void FindEntryAndExit()
    {
        entryTile = GetComponentInChildren<EntryTile>();
        exitTile = GetComponentInChildren<ExitTile>();
    }

}
