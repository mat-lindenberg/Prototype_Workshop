using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenFromTile : MonoBehaviour
{
    public Grid g;
    public Tilemap t;


    public Transform grassHolder;
    public List<Vector3> tileWorldLocations;

    public GameObject grassPrefab;

    public Tile grassTileIndicator;
    public Tile grassTile;

    public Tile dirtTile;
    public Tile dirtIndicatorTile;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void MakeGrass()
    {
        tileWorldLocations = new List<Vector3>();

        foreach (Vector3Int pos in t.cellBounds.allPositionsWithin)
        {
            if (t.GetTile(pos) == grassTileIndicator)
            {
                Debug.Log("match!");
                GameObject g = Instantiate(grassPrefab);
                g.transform.position = t.CellToWorld(pos);
                g.transform.position += new Vector3(.25f, .25f, .25f);
                g.transform.parent = grassHolder;
                t.SetTile(pos, grassTile);
            }
            else
            {
                //Debug.Log("not match of " + t.GetTile(pos) + "");
            }


            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = t.CellToWorld(localPlace);
            if (t.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);
            }
        }

        //print(tileWorldLocations);
    }
    // Update is called once per frame
    void Update()
    {

    }
}