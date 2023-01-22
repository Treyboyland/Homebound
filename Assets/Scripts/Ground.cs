using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ground : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer frontSprite;

    [SerializeField]
    Collider2D groundCollider;

    [SerializeField]
    AK.Wwise.Event digSoundEvent;

    [SerializeField]
    AK.Wwise.RTPC digTimeRTPC;

    [SerializeField]
    float secondsToRestore;

    public UnityEvent OnDirtBroken;

    public UnityEvent<float> OnProgressUpdate = new UnityEvent<float>();

    bool isDigging = false;

    bool isDugOut = false;

    public bool IsDugOut { get { return isDugOut; } }

    Vector3 location;

    Coroutine diggingRoutine;

    bool isDone = true;

    public bool IsDone { get { return isDone; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Dig(float digTime)
    {
        if (!isDigging && !isDugOut)
        {
            diggingRoutine = StartCoroutine(DiggingCoroutine(digTime));
        }
    }

    public void SetLocation(Vector3 location)
    {
        this.location = location;
        transform.position = location;
    }

    public void StopDigging()
    {
        isDigging = false;
        if (diggingRoutine != null)
        {
            StopCoroutine(diggingRoutine);
        }
        digTimeRTPC.SetGlobalValue(0);
        OnProgressUpdate.Invoke(0);
    }

    public void ResetGround()
    {
        groundCollider.enabled = true;
        frontSprite.enabled = true;
        isDugOut = false;
        StartCoroutine(UndoProgress());
    }

    IEnumerator UndoProgress()
    {
        isDone = false;
        float elapsed = 0;
        while (elapsed < secondsToRestore)
        {
            elapsed += Time.deltaTime;
            OnProgressUpdate.Invoke(Mathf.Lerp(100.0f, 0, elapsed / secondsToRestore));
            yield return null;
        }

        OnProgressUpdate.Invoke(0);
        isDone = true;
    }

    void DigOut()
    {
        groundCollider.enabled = false;
        frontSprite.enabled = false;
        isDugOut = true;
        digSoundEvent.Post(gameObject);
    }

    IEnumerator DiggingCoroutine(float secondsToWait)
    {
        isDigging = true;
        float elapsed = 0;
        while (elapsed < secondsToWait)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / secondsToWait * 100;
            digTimeRTPC.SetGlobalValue(Mathf.Min(progress, 100));
            OnProgressUpdate.Invoke(progress);
            yield return null;
        }

        OnProgressUpdate.Invoke(100);
        DigOut();
        digTimeRTPC.SetGlobalValue(0);
        isDigging = false;
        OnDirtBroken.Invoke();
    }
}
