using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Level
{
    public int time;
    public int level;
    public int row;
    public int column;
    public string[] squares;

}
