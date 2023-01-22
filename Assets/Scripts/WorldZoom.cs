using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldZoom : MonoBehaviour
{
    [SerializeField]
    Player player;

    [SerializeField]
    MapCreator creator;

    [SerializeField]
    TrackObject trackObject;

    [SerializeField]
    WorldReset worldReset;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    float worldCameraSize;

    [SerializeField]
    float originalSize;

    [SerializeField]
    float secondsToMove;

    [SerializeField]
    float secondsBetweenGrounds;

    bool isResetting = false;

    private void Awake()
    {
        worldReset.OnResetWorld.AddListener(ResetWorld);
    }

    void ResetWorld()
    {
        if (!isResetting)
        {
            Debug.LogWarning("Reset");
            StartCoroutine(PerformReset());
        }
    }



    IEnumerator MoveMainCamera()
    {
        float elapsed = 0;
        Vector3 start = trackObject.transform.position;

        Vector3 mapCenter = creator.Center;
        mapCenter.z = start.z;
        while (elapsed < secondsToMove)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / secondsToMove;
            mainCamera.orthographicSize = Mathf.Lerp(originalSize, worldCameraSize, progress);
            trackObject.transform.position = Vector3.Lerp(start, mapCenter, progress);
            yield return null;
        }

        mainCamera.orthographicSize = worldCameraSize;
        trackObject.transform.position = mapCenter;
    }

    IEnumerator MoveBack()
    {
        float elapsed = 0;
        Vector3 start = trackObject.transform.position;

        Vector3 mapCenter = creator.Center;
        mapCenter.z = start.z;
        while (elapsed < secondsToMove)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / secondsToMove;
            mainCamera.orthographicSize = Mathf.Lerp(worldCameraSize, originalSize, progress);
            trackObject.transform.position = Vector3.Lerp(mapCenter, trackObject.TargetPos, progress);
            yield return null;
        }

        mainCamera.orthographicSize = originalSize;
        trackObject.transform.position = trackObject.TargetPos;
    }

    IEnumerator PerformReset()
    {
        isResetting = true;
        trackObject.ShouldTrack = false;

        yield return StartCoroutine(MoveMainCamera());
        yield return StartCoroutine(creator.ResetUsedGrounds(secondsBetweenGrounds));
        yield return new WaitForSeconds(3.0f);
        yield return StartCoroutine(MoveBack());
        trackObject.ShouldTrack = true;

        isResetting = false;
    }
}
