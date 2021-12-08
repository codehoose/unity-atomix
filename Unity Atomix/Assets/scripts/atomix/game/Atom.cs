using System;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour
{
    private Dictionary<string, Action> joints;
    private Dictionary<string, Action> atoms;

    public GameObject leftJoin;
    public GameObject rightJoin;
    public GameObject bottomJoin;
    public GameObject topJoin;
    public GameObject cube;

    public Material red;
    public Material green;

    public TMPro.TextMeshPro text;

    [Tooltip("This is the reference used to determine the 'win' condition")]
    public string indexRef;

    void Awake()
    {
        atoms = new Dictionary<string, Action>()
        {
            {"1", () => SetupAtom("H", green) },
            {"3", () => SetupAtom("O", red) }
        };

        joints = new Dictionary<string, Action>()
        {
            { "c", () =>rightJoin.SetActive(true) },
            { "g", () => leftJoin.SetActive(true) }
        };
    }

    public void SetupShape(string indexRef, string atom, string connections)
    {
        this.indexRef = indexRef;
        atoms[atom]();

        for (int i = 0; i < connections.Length; i++)
        {
            string connection = connections[i].ToString();
            joints[connection]();
        }
    }

    private void SetupAtom(string atomName, Material material)
    {
        text.text = atomName;
        cube.GetComponent<Renderer>().material = material;
    }
}
