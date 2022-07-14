using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class vRelic
{
    // Start is called before the first frame update
    public string RelicName;

    public int level;

    public string trait;
    public float[] modification = new float[9];

    public vRelic()
    {

    }

    public vRelic(string name, int _level, string _trait, float[] _mod)
    {
        RelicName = name;
        level = _level;
        trait = _trait;
        modification = _mod;
    }
}
