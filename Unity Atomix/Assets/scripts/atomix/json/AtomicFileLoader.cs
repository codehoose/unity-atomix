using System.Collections.Generic;
using UnityEngine;

public class AtomicFileLoader : MonoBehaviour
{
    private List<GameObject> squares;

    public TextAsset levelFile;

    public AtomixFile levelData;

    public GameObject square;

    public GameObject atomPrefab;

    void Start()
    {
        squares = new List<GameObject>();

        var json = levelFile.text;
        levelData = JsonSerialization.Deserialize<AtomixFile>(json);
        DescribeLevel(0); // TODO: Make this any level
    }

    private void DescribeLevel(int levelIndex)
    {
        // TODO: Can we pool these instead of destroying them?
        foreach (var sq in squares)
        {
            Destroy(sq);
        }
        squares.Clear();

        levelIndex = Mathf.Max(0, levelIndex) % levelData.levels.Length; // Assumes length != 0
        var arena = levelData.levels[levelIndex].arena;

        for (int y = 0; y < arena.Length; y++)
        {
            var row = arena[y];
            for (int x = 0; x < row.Length; x++)
            {
                if (row[x] == '#')
                {
                    var go = Instantiate(square);
                    go.transform.position = new Vector3(x, 0, -y);
                    squares.Add(go);
                } else if (row[x] != '.')
                {
                    string atom = row[x].ToString();
                    string atomType = levelData.levels[levelIndex].atoms[atom][0];
                    string connections = levelData.levels[levelIndex].atoms[atom][1];

                    var go = Instantiate(atomPrefab);
                    go.transform.position = new Vector3(x, 0, -y);
                    go.GetComponent<Atom>().SetupShape(atom, atomType, connections);
                    // TODO: ADD PIECES
                }
            }
        }
    }
}
