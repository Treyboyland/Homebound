using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoundaries : MonoBehaviour
{
    [SerializeField]
    Player player;

    [SerializeField]
    Rigidbody2D body;

    [SerializeField]
    MapCreator creator;

    [SerializeField]
    float threshhold;

    [SerializeField]
    float forcePower;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        PushPlayer();
    }


    void PushPlayer()
    {
        var pos = player.transform.position;
        bool outOfBounds = false;
        Vector2 force = Vector2.zero;

        if (pos.x < creator.XRange.x - threshhold)
        {
            force.x = forcePower;
            outOfBounds = true;
        }
        else if (pos.x > creator.XRange.y + threshhold)
        {
            force.x = -forcePower;
            outOfBounds = true;
        }

        if (pos.y < -creator.Depth - threshhold)
        {
            force.y = forcePower;
            outOfBounds = true;
        }


        if (outOfBounds)
        {
            body.AddForce(force);
        }
    }
}
