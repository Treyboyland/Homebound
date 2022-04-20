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
    float secondsToMove;

    [SerializeField]
    float speed;

    public float Speed { get { return speed; } set { speed = value; } }

    bool isMoving = false;

    Vector2 movementVector;

    public Vector2 MovementVector { get { return movementVector; } }

    public bool IsLeftPressed { get { return movementVector.x < 0; } }
    public bool IsRightPressed { get { return movementVector.x > 0; } }

    public bool IsUpPressed { get { return movementVector.y > 0; } }
    public bool IsDownPressed { get { return movementVector.y < 0; } }

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
        movementVector = movement;
    }

    void DoMove2()
    {
        body.AddForce(movementVector * speed);
    }

    void DoMove1()
    {
        if (!isMoving && movementVector != Vector2.zero)
        {
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        isMoving = true;

        Vector3 start = transform.position;
        Vector3 end = start;

        if (movementVector.y != 0)
        {
            end.y += gameConstantsSO.UnitDistance * Mathf.Sign(movementVector.y);
        }
        else if (movementVector.x != 0)
        {
            end.x += gameConstantsSO.UnitDistance * Mathf.Sign(movementVector.x);
        }

        float elapsed = 0;

        while (elapsed < secondsToMove)
        {
            elapsed += Time.deltaTime;
            body.MovePosition(Vector3.Lerp(start, end, elapsed / secondsToMove));
            yield return null;
        }

        body.MovePosition(end);

        isMoving = false;
    }
}
