using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRT : MonoBehaviour
{
    public Texture2D tex;

    public GameObject cubePrefab;
    public int rows;
    public int cols;

    public GameObject[,] pixelSurface;


    public int placeInBoxes;
    public int numberToUpdateEachTick;

    public List<GameObject> boxes = new List<GameObject>();

    public Vector3 newPos;
    public float moveMult;

    Vector2 storeVec;

    // Start is called before the first frame update
    void Start()
    {
        pixelSurface = new GameObject[rows, cols];
        boxes = new List<GameObject>();


        


        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                GameObject g = Instantiate(cubePrefab);
                // g.GetComponent<Renderer>().material.color = tex.GetPixel(x, y);
                Color c = tex.GetPixel((int)Mathf.Lerp(0, tex.width, (float)x / rows), (int)Mathf.Lerp(0, tex.height, (float)y / cols));
                g.transform.position = new Vector3(x, y, 0);
                pixelSurface[x, y] = g;
                g.transform.parent = this.transform;
                boxes.Add(g);
                g.GetComponent<BabyPixelMover>().StyleMe(c, g.transform.position);

            }
        }



    }

    public void Return()
    {
        for (int y = rows - 1; y >= 0; y--)
        {
            for (int x = cols - 1; x >= 0; x--)
            {
                Vector2 targ = pixelSurface[x, y].GetComponent<BabyPixelMover>().originPos;
                TradePlaces(new Vector2(x, y), targ);
            }
        }
    }

    public void SlowReturn()
    {
        storeVec = Vector2.zero;

        for (int y = rows - 1; y > 0; y--)
        {
            for (int x = cols - 1; x > 0; x--)
            {
                pixelSurface[x, y].GetComponent<BabyPixelMover>().hasMovedThisTick = false;
            }
        }

        for (int y = rows - 1; y >= 0; y--)
        {
            for (int x = cols - 1; x >= 0; x--)
            {
                storeVec.x = x;
                storeVec.y = y;
                Vector2 targ = pixelSurface[x, y].GetComponent<BabyPixelMover>().originPos;
                Vector2 current = pixelSurface[x, y].GetComponent<BabyPixelMover>().transform.position;

                if (pixelSurface[x, y].GetComponent<BabyPixelMover>().hasMovedThisTick == true)
                {

                }else if (Random.Range(0,10) > 5)
                {

                }
                else
                {
                    if (current.x == targ.x)
                    {

                    }
                    else if (current.x > targ.x)
                    {
                        if (pixelSurface[(int)storeVec.x-1,(int)storeVec.y].GetComponent<BabyPixelMover>().hasMovedThisTick == true)
                        {

                        }
                        else {
                            TradePlaces(storeVec, storeVec + new Vector2(-1, 0));
                        }
                       
                    }else if (current.x < targ.x)
                    {
                        if (pixelSurface[(int)storeVec.x + 1, (int)storeVec.y].GetComponent<BabyPixelMover>().hasMovedThisTick == true)
                        {

                        }
                        else
                        {
                            TradePlaces(storeVec, storeVec + new Vector2(1, 0));
                        }
                    }

                    if (current.y == targ.y)
                    {

                    }else if (current.y < targ.y)
                    {
                        if (pixelSurface[(int)storeVec.x, (int)storeVec.y + 1].GetComponent<BabyPixelMover>().hasMovedThisTick == true)
                        {

                        }
                        else
                        {
                            TradePlaces(storeVec, storeVec + new Vector2(0,1));
                        }
                    }
                    else if (current.y > targ.y)
                    {
                        if (pixelSurface[(int)storeVec.x, (int)storeVec.y - 1].GetComponent<BabyPixelMover>().hasMovedThisTick == true)
                        {

                        }
                        else
                        {
                            TradePlaces(storeVec, storeVec + new Vector2(0, -1));
                        }
                    }
                }
            }
        }
    }

    public void SortLogic()
    {
        storeVec = Vector2.zero;
        for (int y = rows - 1; y > 0; y--)
        {
            for (int x = cols - 1; x > 0; x--)
            {
                pixelSurface[x, y].GetComponent<BabyPixelMover>().hasMovedThisTick = false;
            }
        }

        for (int y = rows - 1; y >= 0; y--)
        {
            for (int x = cols - 1; x >= 0; x--)
            {

                float rand = Random.Range(0, 10);

                if (pixelSurface[x, y].GetComponent<BabyPixelMover>().hasMovedThisTick == true)
                {

                }
                else if (rand < 10)
                {
                    storeVec.x = x;
                    storeVec.y = y;

                    if (TestIfTopIsHotter(storeVec))
                    {
                        TradePlaces(storeVec, storeVec + new Vector2(0, 1));
                    }

                }
                else if (rand < 7)
                {
                    storeVec.x = x;
                    storeVec.y = y;
                    if (TestRight(storeVec))
                    {
                        TradePlaces(storeVec, storeVec + new Vector2(1, 0));
                    }
                }
                else if (rand < 11)
                {
                    storeVec.x = x;
                    storeVec.y = y;
                    if (TestLeft(storeVec))
                    {
                        TradePlaces(storeVec, storeVec + new Vector2(-1, 0));
                    }
                }
                else
                {

                }
            }
        }
    }

    public void Update()
    {



        if (Input.GetKey(KeyCode.Space)){
            SlowReturn();
        }
        else
        {
            SortLogic();
        }



    }

    public bool TestLeft(Vector2 origin)
    {
        if (origin.x == 0 || origin.x == 1)
        {
            return false;
            
        }
        Color32 c = pixelSurface[(int)origin.x, (int)origin.y].GetComponent<Renderer>().material.color;
        Color32 c2 = pixelSurface[(int)origin.x - 1, (int)origin.y].GetComponent<Renderer>().material.color;
        int orig = c.r + c.g + c.b;
        int targ = c2.r + c2.b + c2.g;

        if (pixelSurface[(int)origin.x - 1, (int)origin.y].GetComponent<BabyPixelMover>().hasMovedThisTick == true)
        {
            return false;
        }

        if (c.g > c2.g)
        {
            //return true;
        }
        else if (c.g + c.r > c2.g + c2.r)
        {
            //return true;
        }
        else if (c.b < c2.b)
        {
            //return false;
        }
        else if (orig > targ)
        {
            //return true;
        }




        return false;
    }

    public bool TestRight(Vector2 origin)
    {
        if (origin.x == cols - 1)
        {
            return false;
        }

        Color32 c = pixelSurface[(int)origin.x, (int)origin.y].GetComponent<Renderer>().material.color;
        Color32 c2 = pixelSurface[(int)origin.x + 1, (int)origin.y].GetComponent<Renderer>().material.color;
        int orig = c.r + c.g + c.b;
        int targ = c2.r + c2.b + c2.g;

        if (pixelSurface[(int)origin.x + 1, (int)origin.y].GetComponent<BabyPixelMover>().hasMovedThisTick == true)
        {
            return false;
        }

        if (c.b > c2.b)
        {
            //return true;
        }
        else if (c.r + c.g > c2.r + c2.b)
        {
          // return true;
        }
        else if (c.b < c2.b)
        {
            //return false;
        }
        else if (orig > targ)
        {
            //return true;
        }




        return false;
    }

    public bool TestIfTopIsHotter(Vector2 origin)
    {
        //uncomment this for funny looping shit
        if (origin.y == rows -1)
        {
            return false;
        }

        Color32 c = pixelSurface[(int)origin.x, (int)origin.y].GetComponent<Renderer>().material.color;
        Color32 c2 = pixelSurface[(int)origin.x, (int)origin.y+1].GetComponent<Renderer>().material.color;

        if (pixelSurface[(int)origin.x, (int)origin.y + 1].GetComponent<BabyPixelMover>().hasMovedThisTick == true)
        {
            return false;
        }

        int orig = c.r + c.g + c.b;
        int targ = c2.r + c2.b + c2.g;

            if (c.r > c2.r)
            {
            return true;
        }else if (c.r + c.g > c2.r + c2.b)
        {
           //return true;
        }else if (c.b < c2.b){
            //return false;
        }
        else if (orig > targ)
        {
            //return true;
        }




        return false;
    }


    public void TradePlaces(Vector2 origin, Vector2 target)
    {
        //Color c = pixelSurface[(int)origin.x, (int)origin.y].GetComponent<Renderer>().material.color;
        //Color c2 = pixelSurface[(int)target.x, (int)target.y].GetComponent<Renderer>().material.color;
        //pixelSurface[(int)origin.x, (int)origin.y].GetComponent<Renderer>().material.color = c2;
        //pixelSurface[(int)target.x, (int)target.y].GetComponent<Renderer>().material.color = c;


        pixelSurface[(int)origin.x, (int)origin.y].GetComponent<BabyPixelMover>().hasMovedThisTick = true;
        pixelSurface[(int)target.x, (int)target.y].GetComponent<BabyPixelMover>().hasMovedThisTick = true;
        GameObject g = pixelSurface[(int)origin.x, (int)origin.y];
        pixelSurface[(int)origin.x, (int)origin.y] = pixelSurface[(int)target.x, (int)target.y].gameObject;
        pixelSurface[(int)target.x, (int)target.y] = g;
        pixelSurface[(int)target.x, (int)target.y].transform.position = target;
        pixelSurface[(int)origin.x, (int)origin.y].transform.position = origin;

        //pixelSurface[(int)target.x, (int)target.y].GetComponent<BabyPixelMover>().newPos = target;
        //pixelSurface[(int)origin.x, (int)origin.y].GetComponent<BabyPixelMover>().newPos = origin;
    }

    // Update is called once per frame
    public void Jitter()
    {
        Vector3 actualPos;
        int initial = placeInBoxes;
        for (int i = placeInBoxes; i < initial+numberToUpdateEachTick; i++)
        {
            if (i < boxes.Count-1)
            {
                newPos = Vector3.zero;
                actualPos = boxes[i].transform.position;

                if (actualPos.x < 0)
                {
                    boxes[i].transform.position = new Vector3(1, actualPos.y, actualPos.z);

                }
                else if (actualPos.x > rows)
                {
                    boxes[i].transform.position = new Vector3(rows-1, actualPos.y, actualPos.z);
                }
                else
                {
                    newPos.x = 1f;
                    if (Random.Range(0, 10) >= 5)
                    {
                        newPos.x = -1f;
                    }
                }

                if (actualPos.y < 0)
                {
                    boxes[i].transform.position = new Vector3(actualPos.x, 1, actualPos.z);
                }
                else if (actualPos.y > cols)
                {
                    boxes[i].transform.position = new Vector3(actualPos.x, cols-1, actualPos.z);

                }
                else
                {
                    newPos.y = 1f;
                    if (Random.Range(0, 10) >= 5)
                    {
                        newPos.y = -1f;
                    }
                }

                boxes[i].transform.position += (newPos * moveMult);
                placeInBoxes++;
            }
            else
            {
                placeInBoxes = 0;
                return;
            }
        }
    }
}
