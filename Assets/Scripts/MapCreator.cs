using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [SerializeField]
    Vector2Int xRange;

    public Vector2Int XRange { get { return xRange; } }

    [SerializeField]
    int depth;

    public int Depth { get { return depth; } }

    [SerializeField]
    Ground groundPrefab;


    List<Ground> groundPool = new List<Ground>();


    public Vector2 Center
    {
        get
        {
            return new Vector2((xRange.y - xRange.x) / 2.0f, -depth / 2);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateMap();
    }

    Ground CreateGround()
    {
        var instance = Instantiate(groundPrefab, transform);
        instance.gameObject.SetActive(false);

        groundPool.Add(instance);

        return instance;
    }

    Ground GetGround()
    {
        foreach (var ground in groundPool)
        {
            if (!ground.gameObject.activeInHierarchy)
            {
                return ground;
            }
        }

        return CreateGround();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateMap()
    {
        if (xRange.x > xRange.y)
        {
            throw new System.Exception("Starting bound should be less than end bound");
        }
        for (int x = xRange.x; x < xRange.y; x++)
        {
            for (int y = -1; y >= -depth; y--)
            {
                Vector3 position = new Vector3(x, y, transform.position.z);
                var ground = GetGround();
                ground.transform.position = position;
                ground.gameObject.SetActive(true);
            }

        }
    }

    bool AreAnyNotDone(List<Ground> grounds)
    {
        foreach (var temp in grounds)
        {
            if (!temp.IsDone)
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerator ResetUsedGrounds(float secondsToWait)
    {
        List<Ground> used = new List<Ground>();

        foreach (var ground in groundPool)
        {
            if (ground.gameObject.activeInHierarchy && ground.IsDugOut)
            {
                used.Add(ground);
            }
        }

        used.Shuffle();

        foreach (var ground in used)
        {
            ground.ResetGround();
            yield return new WaitForSeconds(secondsToWait);
        }

        while (AreAnyNotDone(used))
        {
            yield return null;
        }
    }
}
