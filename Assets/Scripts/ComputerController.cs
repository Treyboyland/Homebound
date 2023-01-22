using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.InputSystem;


public class ComputerController : MonoBehaviour
{
    [SerializeField]
    PlayerController player;

    [SerializeField]
    PlayerDig playerDig;

    [SerializeField]
    bool isComputerControlled;

    [SerializeField]
    MapCreator map;

    [SerializeField]
    WorldReset worldReset;

    [SerializeField]
    WorldZoom worldZoom;

    public bool IsComputerControlled { get => isComputerControlled; set => isComputerControlled = value; }

    [SerializeField]
    float secondsToMove;

    [SerializeField]
    float secondsToWait;

    float elapsed = 0;

    enum Direction
    {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN,
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.ContextVector != Vector2.zero)
        {
            RelinquishControl();
        }
        else
        {
            elapsed += Time.deltaTime;
            if (elapsed >= secondsToWait && !isComputerControlled)
            {
                isComputerControlled = true;
                StartCoroutine(AutomaticDig());
            }
        }
    }

    public void RelinquishControl()
    {
        StopAllCoroutines();
        elapsed = 0;
        isComputerControlled = false;
        //Debug.LogWarning("Manual");
    }

    void OnComputerControl(InputValue context)
    {
        bool pressed = context.Get<float>() != 0;
        if (pressed)
        {
            elapsed = secondsToWait;
        }
    }

    Direction GetDirectionToGround(Ground ground)
    {
        Direction toMove;
        var diff = ground.transform.position - player.transform.position;

        if (Mathf.Abs(diff.x) <= .6)
        {
            toMove = diff.y < 0 ? Direction.DOWN : Direction.UP;
            //Debug.LogWarning(toMove);
            return toMove;
        }
        else if (Mathf.Abs(diff.y) <= .6)
        {
            toMove = diff.x < 0 ? Direction.LEFT : Direction.RIGHT;
            //Debug.LogWarning(toMove);
            return toMove;
        }

        List<Direction> directions = new List<Direction>();

        directions.Add(diff.x < 0 ? Direction.LEFT : Direction.RIGHT);
        directions.Add(diff.y < 0 ? Direction.DOWN : Direction.UP);

        directions.Shuffle();

        toMove = directions[0];

        //Debug.LogWarning(toMove);
        return toMove;
    }

    void MoveTo(Direction direction)
    {
        switch (direction)
        {
            case Direction.LEFT:
                player.MovementVector = new Vector2(-1, 0);
                break;
            case Direction.RIGHT:
                player.MovementVector = new Vector2(1, 0);
                break;
            case Direction.UP:
                player.MovementVector = new Vector2(0, 1);
                break;
            case Direction.DOWN:
                player.MovementVector = new Vector2(0, -1);
                break;
            case Direction.NONE:
                player.MovementVector = Vector2.zero;
                break;
        }
    }

    IEnumerator WaitUntilGroundFound(Direction direction)
    {
        float elapsed = 0;

        while (playerDig.CurrentGround == null)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= 1.75)
            {

                Debug.LogWarning("Stuck");
                MoveTo(GetRandomDirection(direction));
                yield return new WaitForSeconds(.5f);

                yield break;
            }
            yield return null;
        }
    }

    Direction GetRandomDirection(Direction notThisOne)
    {
        List<Direction> directions = new List<Direction>((Direction[])System.Enum.GetValues(typeof(Direction)));
        directions.Remove(notThisOne);

        directions.Shuffle();
        return directions[0];
    }

    IEnumerator WaitUntilGroundDug(Ground ground)
    {
        float elapsed = 0;

        while (ground.IsDugOut)
        {
            elapsed += Time.deltaTime;
            var direction = GetDirectionToGround(playerDig.CurrentGround);
            MoveTo(direction);

            if (elapsed >= 3)
            {
                MoveTo(GetRandomDirection(direction));
                yield return new WaitForSeconds(.5f);
                yield break;
            }
        }
    }

    Direction GetDirectionToImmediateGround(Ground ground)
    {
        Direction toMove;
        var diff = ground.transform.position - player.transform.position;
        var absX = Mathf.Abs(diff.x);
        var absY = Mathf.Abs(diff.y);


        if (absX > absY)
        {
            toMove = diff.x < 0 ? Direction.LEFT : Direction.RIGHT;
        }
        else
        {
            toMove = diff.y < 0 ? Direction.DOWN : Direction.UP;
        }

        //Debug.LogWarning(toMove);
        return toMove;
    }

    IEnumerator DigToGround(Ground ground)
    {
        while (!ground.IsDugOut)
        {
            if (playerDig.CurrentGround != null)
            {
                //Debug.LogWarning("Ground Pos (Found): " + playerDig.CurrentGround.transform.position);
                MoveTo(GetDirectionToImmediateGround(playerDig.CurrentGround));
                yield return StartCoroutine(WaitUntilGroundDug(playerDig.CurrentGround));
                elapsed = 0;
            }
            else
            {
                //Debug.LogWarning("Ground Pos (Null): " + ground.transform.position);
                var groundDir = GetDirectionToGround(ground);
                MoveTo(groundDir);
                yield return StartCoroutine(WaitUntilGroundFound(groundDir));
            }
            yield return null;
        }
    }

    IEnumerator MoveToWholeNum()
    {
        Vector3 pos = player.transform.position;

        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);

        float elapsed = 0;

        Vector3 start = player.transform.position;

        while (elapsed < secondsToMove)
        {
            elapsed += Time.deltaTime;
            player.transform.position = Vector3.Lerp(start, pos, elapsed / secondsToMove);
            yield return null;
        }

        player.transform.position = pos;
    }

    IEnumerator AutomaticDig()
    {
        while (true)
        {
            yield return StartCoroutine(MoveToWholeNum());
            var undugGround = map.GetActiveGround();
            undugGround.Shuffle();
            foreach (var ground in undugGround)
            {
                yield return StartCoroutine(DigToGround(ground));
            }

            yield return new WaitForSeconds(3.0f);
            worldReset.OnResetWorld.Invoke();
            while (worldZoom.IsResetting)
            {
                yield return null;
            }
            yield return null;
        }
    }
}
