using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RailroadGrid : MonoBehaviour
{
    public Tilemap tilemap;

    public GameObject railroadNodeObject;

    public List<RailroadNode> railRoadObjects = new List<RailroadNode>();

    public MatTrain train;
    // Start is called before the first frame update
    void Start()
    {
        int x, y, z;


        for (x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++)
        {
            for (y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++)
            {
                for (z = tilemap.cellBounds.min.z; z < tilemap.cellBounds.max.z; z++)
                {

                    TileBase t = tilemap.GetTile(new Vector3Int(x, y, z));
                    if (t != null)
                    {
                        Debug.Log(t.name);
                        GameObject g = Instantiate(railroadNodeObject);
                        g.transform.position = new Vector3(x, .25f, y);
                        railRoadObjects.Add(g.GetComponent<RailroadNode>());
                        g.GetComponent<RailroadNode>().nameType = t.name;
                        g.GetComponent<RailroadNode>().x = x;
                        g.GetComponent<RailroadNode>().z = y;
                    }
                   

                }
            }

        }

        MakeConnections();

        train.PlaceOnNode(railRoadObjects[0]);
    }

    public void MakeConnections()
    {
        for(int i = 0; i < railRoadObjects.Count; i++)
        {
            RailroadNode r = railRoadObjects[i];
            //figure out what type
            //look in that dir
            //return dudes and add
            if (railRoadObjects[i].nameType == "VerticalTrack")
            {
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x,r.z+1));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x, r.z - 1));
            }
            if (railRoadObjects[i].nameType == "Horizontal1Track")
            {
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x-1, r.z));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x+1, r.z));
            }
            if (railRoadObjects[i].nameType == "CornerBottomToLeft")
            {
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x-1, r.z));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x, r.z-1));
            }
            if (railRoadObjects[i].nameType == "CornerLeftToTop")
            {
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x-1, r.z));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x, r.z+1));
            }
            if (railRoadObjects[i].nameType == "CornerRightToBottom")
            {
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x+1, r.z));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x, r.z-1));
            }
            if (railRoadObjects[i].nameType == "CornerTopToRight")
            {
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x+1, r.z));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x, r.z+1));
            }
            if (railRoadObjects[i].nameType == "bltTJunction")
            {
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x, r.z-1));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x, r.z+1));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x-1, r.z));
            }
            if (railRoadObjects[i].nameType == "ltrTjunction")
            {
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x+1, r.z));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x-1, r.z));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x, r.z+1));
            }
            if (railRoadObjects[i].nameType == "rblTJunction")
            {
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x+1, r.z));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x-1, r.z));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x, r.z-1));
            }
            if (railRoadObjects[i].nameType == "trbTJunction")
            {
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x, r.z+1));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x, r.z-1));
                railRoadObjects[i].connections.Add(returnObjectAtPosition(r.x+1, r.z));
            }

        }
    }

    public RailroadNode returnObjectAtPosition(int x, int z)
    {
        for (int i = 0; i < railRoadObjects.Count; i++)
        {
            if (railRoadObjects[i].x == x && railRoadObjects[i].z == z)
            {
                return railRoadObjects[i];
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
