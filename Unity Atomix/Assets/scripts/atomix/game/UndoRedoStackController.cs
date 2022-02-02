using UnityEngine;
using UnityEngine.UI;

public class UndoRedoStackController : MonoBehaviour
{
    private Button _button;

    public bool isUndo = true;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Undo()
    {
        UndoRedoStack.Instance.Undo();
    }

    public void Redo()
    {
        UndoRedoStack.Instance.Redo();
    }

    void Update()
    {
        if (isUndo)
        {
            _button.interactable = UndoRedoStack.Instance.CanUndo;
        }
        else
        {
            _button.interactable = UndoRedoStack.Instance.CanRedo;
        }
    }
}
