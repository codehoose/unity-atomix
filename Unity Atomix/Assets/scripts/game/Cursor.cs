using System;
using System.Collections;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public static string PlayPiece = "PlayPiece";

    private bool _moving = false;

    public event CursorOverPieceDelegate CursorOverPiece;

    public event CursorOverPieceDelegate CursorExitPiece;

    public event DirectionSelectedDelegate DirectionSelected;

    public bool noMove;

    void Update()
    {
        if (_moving)
            return;

        var newPos = Vector3.zero;

        if (IsDirection(Vector3.left, KeyCode.A, KeyCode.LeftArrow))
        {
            newPos += Vector3.left;
            if (DirectionSelected != null)
                DirectionSelected(this, new DirectionSelectedEventArgs(Vector3.left));
        }
        else if (IsDirection(Vector3.right, KeyCode.D, KeyCode.RightArrow))
        {
            newPos += Vector3.right;
            if (DirectionSelected != null)
                DirectionSelected(this, new DirectionSelectedEventArgs(Vector3.right));
        }
        else if (IsDirection(Vector3.forward, KeyCode.W, KeyCode.UpArrow))
        {
            newPos += Vector3.forward;
            if (DirectionSelected != null)
                DirectionSelected(this, new DirectionSelectedEventArgs(Vector3.forward));
        }
        else if (IsDirection(Vector3.back, KeyCode.S, KeyCode.DownArrow))
        {
            newPos += Vector3.back;
            if (DirectionSelected != null)
                DirectionSelected(this, new DirectionSelectedEventArgs(Vector3.back));
        }

        var target = transform.position + newPos;

        if (noMove || newPos == Vector3.zero)
            return;

        MessageQueue.Instance.SendMessage(CursorMoveMessage.Instance);
        StartCoroutine(Move(transform.position, target));
    }

    public void MoveAllowed()
    {
        StartCoroutine(ToggleMove(true));
    }

    /// <summary>
    /// Did the player press one of the keys, and can the cursor travel in that direction?
    /// </summary>
    /// <param name="direction">Direction</param>
    /// <param name="keys">Keys</param>
    /// <returns>True if the player hit the cursor key and they CAN travel in that direction</returns>
    public bool IsDirection(Vector3 direction, params KeyCode[] keys)
    {
        bool keyDown = false;
        foreach (var key in keys)
        {
            keyDown |= Input.GetKey(key);
        }

        return keyDown && CursorNoWallHit(direction);
    }

    /// <summary>
    /// Check to see if the cursor hits a wall. If so, the function returns false.
    /// </summary>
    /// <param name="direction">The direction the cursor wants to travel</param>
    /// <returns>True if the cursor can make travel in that direction</returns>
    private bool CursorNoWallHit(Vector3 direction)
    {
        RaycastHit hitInfo = new RaycastHit();
        int layerMask = 1 << LayerMask.NameToLayer(PlayPiece);
        if (Physics.Raycast(transform.position, direction, out hitInfo, 1, layerMask))
        {
            return false;
        }

        return true;
    }

    private IEnumerator ToggleMove(bool move)
    {
        yield return null;
        Input.ResetInputAxes();
        yield return null;

        noMove = !move;
    }

    private IEnumerator Move(Vector3 source, Vector3 target)
    {
        _moving = true;
        float speed = 0.1f;

        float time = 0f;
        while (time <1f)
        {
            transform.position = Vector3.Lerp(source, target, time);
            time += Time.deltaTime / speed;
            yield return null;
        }

        transform.position = target;
        _moving = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (CursorOverPiece == null || other.tag != PlayPiece)
            return;

        CursorOverPiece.Invoke(this, new PieceEventArgs(other.gameObject));
    }

    public void OnTriggerExit(Collider other)
    {
        if (CursorExitPiece == null || other.tag != PlayPiece)
            return;

        CursorExitPiece.Invoke(this, new PieceEventArgs(other.gameObject));
    }
}
