using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RailroadNode : MonoBehaviour
{
    public int x;
    public int z;

    public string nameType;


    public List<RailroadNode> connections = new List<RailroadNode>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddConnection(RailroadNode r)
    {
        connections.Add(r);
    }

    public RailroadNode returnNodeAtDirection(Vector3 dir)
    {
        for (int i = 0; i< connections.Count; i++)
        {
            RailroadNode r = connections[i];

            if (dir.z == 0)
            {
                if (r.x == x + dir.x)
                {
                    return r;
                }
            }

            if (dir.x == 0)
            {
                if (r.z == z + dir.z)
                {
                    return r;
                }
            }
        }

        return null;
    }

}
