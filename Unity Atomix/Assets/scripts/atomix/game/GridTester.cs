using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GridTester
{
    private List<string> _rows;
    private string _molecule;

    public string CurrentState
    {
        get
        {
            return string.Join("\n", _rows.ToArray());
        }
    }

    public GridTester(string[] rows, string[] molecule)
    {

        _rows = new List<string>(rows); // gg.grid = copy_grid(level.arena);

        var regex = new Regex("/./g");
        var ggGrid = regex.Replace(_rows[0], ".").Substring(molecule[0].Length);

        _molecule = string.Join(ggGrid, molecule);


        //gg.molecule = level.molecule.join(
        //    gg.grid[0].replace(/./ g, '.').substring(level.molecule[0].length)
        //);

    }

    public void MovePiece(string tag, Vector3 newPosition)
    {
        if (_rows.Count == 0 || _rows[0].Length == 0)
            return;

        var columns = _rows[0].Length;
        var grid = string.Join("", _rows);
        var index = grid.IndexOf(tag);
        var x = index % columns;
        var y = index / columns;

        var nx = (int)newPosition.x;
        var ny = (int)-newPosition.z;

        // Replace current piece position
        var oldRow = _rows[y];
        _rows[y] = oldRow.Replace(tag[0], '.');

        string newRow = "";

        for (int i = 0; i < _rows[ny].Length; i++)
        {
            if (i == nx)
            {
                newRow += tag;
            }
            else
            {
                newRow += _rows[ny][i];
            }
        }

        _rows[ny] = newRow;
    }

    public bool IsSolution()
    {
        var testGrid = string.Join("", _rows);
        var regex = new Regex("/#/g");
        testGrid = regex.Replace(testGrid, ".");

        return testGrid.IndexOf(_molecule) >= 0;
    }
}
