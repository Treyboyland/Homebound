using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour
{
    [SerializeField]
    GameObject objectToTrack;

    [SerializeField]
    bool shouldTrack = true;

    public bool ShouldTrack { get { return shouldTrack; } set { shouldTrack = value; } }

    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - objectToTrack.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldTrack)
        {
            transform.position = objectToTrack.transform.position + offset;
        }
    }

    public Vector3 TargetPos { get { return objectToTrack.transform.position + offset; } }
}
