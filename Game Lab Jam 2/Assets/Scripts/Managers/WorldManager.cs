using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameObject[] segmentPrefabs;
    [SerializeField] private int instantiateDistance;
    [SerializeField] private int destroyDistance;
    [SerializeField] private List<Segment> activeSegments;
    [SerializeField] private Segment latestSegment;

    private void Start()
    {
        foreach (Segment segment in activeSegments)
        {
            segment.FindEntryAndExit();
        }
    }

    private void Update()
    {
        // Destroy segments

        while (Player.Instance.transform.position.x - destroyDistance > activeSegments[0].ExitTile.transform.position.x)
        {
            Debug.Log("Destroying segment");
            Destroy(activeSegments[0].gameObject);
            activeSegments.RemoveAt(0);
        }

        // Instantiate segments

        while (Player.Instance.transform.position.x + instantiateDistance > latestSegment.ExitTile.transform.position.x)
        {
            Debug.Log("Instantiating segment");
            Segment newSegment = Instantiate(segmentPrefabs[Random.Range(0, segmentPrefabs.Length)], transform).GetComponent<Segment>();
            newSegment.FindEntryAndExit();

            Vector3 relativeEntryTilePosition = newSegment.EntryTile.transform.position - newSegment.transform.position;
            newSegment.transform.position = latestSegment.ExitTile.transform.position + new Vector3(2f, 0f, 0f) - relativeEntryTilePosition;

            activeSegments.Add(newSegment);
            latestSegment = newSegment;
        }
    }


}
