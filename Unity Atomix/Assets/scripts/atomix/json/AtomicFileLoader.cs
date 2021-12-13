using System.Collections.Generic;
using UnityEngine;

public class AtomicFileLoader : MonoBehaviour
{
    private List<GameObject> squares;

    private List<Atom> atoms;

    public TextAsset levelFile;

    public AtomixFile levelData;

    public GameObject square;

    public GameObject atomPrefab;

    public DirectionsAllowed directionsAllowed;

    public DirectionLines directionLines;

    void Start()
    {
        squares = new List<GameObject>();
        atoms = new List<Atom>();

        var json = levelFile.text;
        levelData = JsonSerialization.Deserialize<AtomixFile>(json);
        DescribeLevel(0); // TODO: Make this any level

        directionLines.Clicked += Direction_Clicked;
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
                    Atom a = go.GetComponent<Atom>();
                    a.SetupShape(atom, atomType, connections);
                    a.Clicked += Atom_Clicked;
                    atoms.Add(a);
                }
            }
        }
    }

    private void Atom_Clicked(object sender, string e)
    {
        // Cancel all other cursor direction lines
        directionsAllowed.TurnOff();

        var gameObject = sender as Atom;
        directionsAllowed.TurnOn(gameObject.gameObject);
    }


    private void Direction_Clicked(object sender, Vector3 e)
    {
        print(e);
    }
}
