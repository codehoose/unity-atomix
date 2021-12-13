using System;
using UnityEngine;

public class DirectionLines : MonoBehaviour
{
    int _index = 0;
    public LineRenderer[] lines;
    public float yOffset = 0.26f;

    public event EventHandler<Vector3> Clicked;

    void Start()
    {
        foreach (var line in lines)
        {
            line.GetComponent<Trundle>().Clicked += Line_Clicked;
        }
    }

    public void Clear()
    {
        foreach (var line in lines)
        {
            line.gameObject.SetActive(false);
        }

        _index = 0;
    }

    public void AddLine(Vector3 start, Vector3 end)
    {
        var newStart = start;
        var newEnd = end;
        
        newEnd.y = yOffset;
        newStart.y = yOffset;

        var line = lines[_index++];
        line.SetPosition(0, newStart);
        line.SetPosition(1, newEnd);
        line.gameObject.SetActive(true);

        var trundle = line.GetComponent<Trundle>();
        trundle.AddColliderToLine(newStart, newEnd);
    }

    private void Line_Clicked(object sender, Vector3 direction)
    {
        Clicked?.Invoke(this, direction);
    }
}
