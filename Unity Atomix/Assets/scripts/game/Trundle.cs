using System;
using UnityEngine;

public class Trundle : MonoBehaviour
{
    float x = 0f;

    private GameObject col;

    public Vector3 direction;

    public event EventHandler<Vector3> Clicked;

    public void AddColliderToLine(Vector3 start, Vector3 end)
    {
        if (col != null)
        {
            Destroy(col);
        }

        // Create a new box collider
        col = new GameObject();
        var boxCollider = col.AddComponent<BoxCollider>();
        var direction = (end - start).normalized;

        // Set it's transform to be the distance to the end point - 0.5
        var xSize = Mathf.Abs((end.x - start.x) - 0.5f);
        var ySize = Mathf.Abs((end.z - start.z));

        // Set the width or height to be 1. 
        var width = Mathf.Max(xSize, 1);
        var height = Mathf.Max(1, ySize);
        // Set the height or width to be the distance between the start and end
        // Add the "clicked" script to the new collider
        var clicked = col.AddComponent<Clicked>();
        clicked.direction = direction;
        clicked.Click += Line_Clicked;

        col.transform.SetParent(transform);
        col.transform.position = start + (end - start) / 2f;
        boxCollider.size = new Vector3(width, 0, height);
    }

    private void Line_Clicked(object sender, Vector3 e)
    {
        Clicked?.Invoke(this, e);
    }

    void Update()
    {
        x -= Time.deltaTime / 1.5f;
        x = Mathf.Repeat(x, 1f);
        var offset = new Vector2(x, 0);

        var material = GetComponent<LineRenderer>().material;
        material.mainTextureOffset = offset;
    }
}
