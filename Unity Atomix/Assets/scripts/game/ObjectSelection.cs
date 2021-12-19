using System;
using System.Collections;
using UnityEngine;

[Obsolete]
public class ObjectSelection : MonoBehaviour
{
    private GameObject _piece;
    private DirectionsAllowed _directions;

    void Awake()
    {
        _directions = GetComponent<DirectionsAllowed>();
    }

    private void Cursor_DirectionSelected(object sender, DirectionSelectedEventArgs e)
    {
        if (IsNormalMovement(e.Direction))
        {
        }
        else
        {
            _piece.GetComponent<Bounce>().StopIt();
            _directions.TurnOff();
            //cursor.MoveAllowed();
            var newPos = _directions.Get(e.Direction);

            UndoRedoStack.Instance.Add(new MovePieceCommand(_piece, _piece.transform.position, newPos));
            _piece = null;
        }
    }

    private bool IsNormalMovement(Vector3 direction)
    {
        //return !cursor.noMove || _piece == null || !_directions.IsAvailable(direction);
        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_piece != null)
            {
                if (_piece.GetComponent<Bounce>().bouncing)
                {
                    _directions.TurnOff();
                    _piece.GetComponent<Bounce>().StopIt();
                    //cursor.noMove = false;
                }
                else
                {
                    //cursor.noMove = true;
                    _piece.GetComponent<Bounce>().StartIt();
                    _directions.TurnOn(_piece);
                }
            }
        }
    }

    private void Cursor_CursorExitPiece(object sender, PieceEventArgs e)
    {
        _piece = null;
    }

    private void Cursor_CursorOverPiece(object sender, PieceEventArgs e)
    {
        _piece = e.Piece;
    }
}
