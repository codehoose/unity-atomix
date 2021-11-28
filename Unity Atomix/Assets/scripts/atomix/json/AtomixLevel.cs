using System;
using System.Collections.Generic;

[Serializable]
public class AtomixLevel
{
    public string id;
    public string name;
    public Dictionary<string, string[]> atoms;
    public string[] arena;
    public string[] molecule;
}
