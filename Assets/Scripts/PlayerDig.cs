using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDig : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    [SerializeField]
    public float digTime;

    [SerializeField]
    PlayerRaycast leftRaycast, rightRayCast, topRaycast, bottomRaycast;

    Ground currentGround;

    public Ground CurrentGround { get => currentGround; }

    private void Update()
    {
        CheckDigging();
    }

    void CheckDigging()
    {
        Ground ground = null;

        if (leftRaycast.WasHit && playerController.IsLeftPressed)
        {
            ground = leftRaycast.Hit.collider.gameObject.GetComponent<Ground>();
        }
        else if (rightRayCast.WasHit && playerController.IsRightPressed)
        {
            ground = rightRayCast.Hit.collider.gameObject.GetComponent<Ground>();
        }
        else if (topRaycast.WasHit && playerController.IsUpPressed)
        {
            ground = topRaycast.Hit.collider.gameObject.GetComponent<Ground>();
        }
        else if (bottomRaycast.WasHit && playerController.IsDownPressed)
        {
            ground = bottomRaycast.Hit.collider.gameObject.GetComponent<Ground>();
        }

        if (ground != null)
        {
            ground.Dig(digTime);
        }
        else if (currentGround != null)
        {
            currentGround.StopDigging();
            currentGround = null;
        }
        if (currentGround == null && ground != null)
        {
            currentGround = ground;
        }
        else if (currentGround != ground)
        {
            currentGround.StopDigging();
            currentGround = ground;
        }
    }
}
