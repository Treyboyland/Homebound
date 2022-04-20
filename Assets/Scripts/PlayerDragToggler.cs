using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDragToggler : MonoBehaviour
{
    [SerializeField]
    float movingDrag;

    [SerializeField]
    float stoppedDrag;

    [SerializeField]
    Rigidbody2D body;

    [SerializeField]
    PlayerController controller;

    // Update is called once per frame
    void Update()
    {
        body.drag = controller.MovementVector != Vector2.zero ? movingDrag : stoppedDrag;
    }
}
