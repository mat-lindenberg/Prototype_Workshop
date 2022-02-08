using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class seasonLevelInit : MonoBehaviour
{
    public GameObject stumpPrefab;
    public GameObject rockPrefab;
    public GameObject weedPrefab;

    public Tilemap tilemap;

    public Tile wetDirt;
    public Tile tilledDirt;
    public Tile dirt;
    public Tile plantableDirt;

    // Start is called before the first frame update
    void Start()
    {
        setupObstacles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setupObstacles()
    {
        float r = Random.Range(0, 100);
        Vector3 offset = new Vector3(.50f, 0, -.50f);

        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
        {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
            {
                TileBase currentTile = tilemap.GetTile(new Vector3Int(n, p, 0));
                if (currentTile == dirt)
                {
                    r = Random.Range(0, 100);

                    if (r < 25)
                    {
                        GameObject g = Instantiate(weedPrefab);
                        g.transform.position = tilemap.CellToWorld(new Vector3Int(n, p, 0));
                        g.transform.position += offset;

                    }
                    else if (r < 45)
                    {
                        GameObject g = Instantiate(rockPrefab);
                        g.transform.position = tilemap.CellToWorld(new Vector3Int(n, p, 0));
                        g.transform.position += offset;
                    }
                    else if (r < 65)
                    {
                        GameObject g = Instantiate(stumpPrefab);
                        g.transform.position = tilemap.CellToWorld(new Vector3Int(n, p, 0));
                        g.transform.position += offset;
                    }
                }
            }
        }
    }


}
