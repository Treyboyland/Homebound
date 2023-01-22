using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameConstantsSO gameConstantsSO;

    [SerializeField]
    Rigidbody2D body;

    [SerializeField]
    ComputerController computerController;

    [SerializeField]
    float secondsToMove;

    [SerializeField]
    float speed;

    public float Speed { get { return speed; } set { speed = value; } }

    bool isMoving = false;

    Vector2 movementVector;

    public Vector2 MovementVector { get { return movementVector; } set { movementVector = value; } }

    public bool IsLeftPressed { get { return movementVector.x < 0; } }
    public bool IsRightPressed { get { return movementVector.x > 0; } }

    public bool IsUpPressed { get { return movementVector.y > 0; } }
    public bool IsDownPressed { get { return movementVector.y < 0; } }

    Vector2 contextVector = Vector2.zero;

    /// <summary>
    /// Received from input value
    /// </summary>
    /// <value></value>
    public Vector2 ContextVector { get => contextVector; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DoMove2();
    }

    void OnMovement(InputValue context)
    {
        Vector2 movement = context.Get<Vector2>();
        contextVector = movement;
        movementVector = movement;
    }

    void DoMove2()
    {
        body.AddForce(movementVector * speed);
    }
}
