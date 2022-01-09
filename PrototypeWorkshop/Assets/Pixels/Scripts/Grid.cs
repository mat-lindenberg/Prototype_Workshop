using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {

	public int xSize, ySize;

    public Mesh mesh;
	public Vector3[] vertices;
    public Color[] colors;
    public Texture2D tex;
    public Texture2D targetTex;


    public Texture2D[] textures;
    public int placeInTextureArray;
    public bool[] hasMoved;

    public List<int> unmatchedIds;

    public bool[] matched;

    public Color[] targetColors;

    public bool isSearching;

    public float matchThresh;

    public Camera mainCamera;

    public float darkThresh = .2f;
    public float lowWaterHueValue = .3f;
    public float highWaterHueValue = .8f;

    public void Awake () {
		Generate();
	}

    public void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        colors = new Color[(xSize + 1) * (ySize + 1)];
        targetColors = new Color[(xSize + 1) * (ySize + 1)];
        hasMoved = new bool[(xSize + 1) * (ySize + 1)];
        matched = new bool[(xSize + 1) * (ySize + 1)];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
                colors[i] = tex.GetPixel((int)Mathf.Lerp(0, tex.width, (float)x / xSize), (int)Mathf.Lerp(0, tex.height, (float)y / ySize));
                targetColors[i] = targetTex.GetPixel((int)Mathf.Lerp(0, targetTex.width, (float)x / xSize), (int)Mathf.Lerp(0, targetTex.height, (float)y / ySize));
                //colors[i] = Color.HSVToRGB(Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f));
                unmatchedIds.Add(i);
            }
        }
        mesh.vertices = vertices;
        //mesh.uv = uv;
        //mesh.tangents = tangents;
        mesh.colors = colors;
        

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        mainCamera.transform.position = new Vector3((float)xSize/2f, (float)ySize /2f, -10);
        mainCamera.orthographicSize = ((float)xSize /2f) + 25;
    }

    public void NewTarget()
    {
        targetTex = textures[placeInTextureArray];
        placeInTextureArray++;
        if (placeInTextureArray > textures.Length-1)
        {
            placeInTextureArray = 0;
        }

        unmatchedIds.Clear();


        for (int i = 0; i < matched.Length; i++)
        {
            matched[i] = false;
        }

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                targetColors[i] = targetTex.GetPixel((int)Mathf.Lerp(0, targetTex.width, (float)x / xSize), (int)Mathf.Lerp(0, targetTex.height, (float)y / ySize));
                //colors[i] = Color.HSVToRGB(Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f));
                unmatchedIds.Add(i);
            }
        }


    }


    public void HueTest()
    {
        for (int i = 0; i < hasMoved.Length; i++)
        {
            hasMoved[i] = false;
        }
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                if (hasMoved[i] == true)
                {
                    continue;
                }

                if (x == 0 || x == xSize || y == 0 || y == ySize)
                {
                    continue;
                }

                Color c = colors[i];
                float hue = 0;
                float sat = 0;
                float val = 0;
                Color.RGBToHSV(c, out hue, out sat, out val);

                float r = Random.Range(0, 20);
                if (r < 2)
                {
                    //continue;
                }

                Color below = colors[i - (xSize + 1)];
                float bh = 0;
                float bs = 0;
                float bv = 0;
                Color.RGBToHSV(below, out bh, out bs, out bv);

                

                if (val > bv)
                {
                    if (hasMoved[i - (xSize + 1)] == false)
                    SwapColors(i, i - (xSize + 1));
                    continue;
                }

                if (x > 0)
                {
                    Color.RGBToHSV(colors[i - 1], out bh, out bs, out bv);
                    if (hue > bh)
                    {
                        if (hasMoved[i-1] == false)
                        SwapColors(i, i - 1);
                        continue;
                    }
                }

                if (y > 0 && x > 0)
                {
                    Color.RGBToHSV(colors[i - xSize], out bh, out bs, out bv);
                    if (sat > bs)
                    {
                        if (hasMoved[i - xSize] == false)
                            SwapColors(i, i - xSize);
                    }
                }

            }
        }

        mesh.colors = colors;
    }

    public void HueSpecificTest()
    {
        for (int i = 0; i < hasMoved.Length; i++)
        {
            hasMoved[i] = false;
        }

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                if (hasMoved[i] == true)
                {
                   continue;
                }

                if (x == 0 || x == xSize || y == 0 || y == ySize)
                {
                    //continue;
                }

                Color c = colors[i];
                float hue = 0;
                float sat = 0;
                float val = 0;
                Color.RGBToHSV(c, out hue, out sat, out val);

                float r = Random.Range(0, 20);
                if (r < 2)
                {
                    continue;
                }

                //if (hue < 164f && hue > 212f)
                if (sat < .001f)
                {
                    if (y > 0)
                    {
                        //SwapColors(i, i - (xSize + 1));
                    }
                }
                else if (val < .001f)
                {
                    if (y > 0)
                    {
                        if (hasMoved[i - (xSize + 1)] == true)
                        {
                            continue;
                        }
                        //SwapColors(i, i - (xSize + 1));
                    }
                }
                else if (val > .99f && sat < .02f)
                {
                    if (y < ySize && x < xSize)
                    {
                        if (hasMoved[i + (xSize + 2)] == true)
                        {
                            continue;
                        }
                        //SwapColors(i, i + (xSize + 2));
                    }
                }
                else if (hue > .0f && hue < .25f)
                {
                    //yellow
                    if (y < ySize)
                    {
                        if (hasMoved[i + (xSize + 1)] == true)
                        {
                            continue;
                        }

                        float th = 0;
                        float tv = 0;
                        float ts = 0;
                        Color.RGBToHSV(colors[i + (xSize + 1)], out th, out tv, out ts);

                        if (hue > th)
                        {
                           // SwapColors(i, i + (xSize + 1));
                        }
                        else
                        {
                            //SwapColors(i + (xSize + 1), 1);
                        }
                        //SwapColors(i, i + (xSize + 1));
                    }

                }
                else if (hue > .40f && hue < .8f)
                {
                    //blue
                    if (y > 0)
                    {
                        if (hasMoved[i - (xSize + 1)] == true)
                        {
                            //continue;
                        }

                        float th = 0;
                        float tv = 0;
                        float ts = 0;
                        Color.RGBToHSV(colors[i - (xSize + 1)], out th, out tv, out ts);

                        if ( hue > th)
                        {
                            SwapColors(i, i - (xSize + 1));
                        }


                     
                    }
                }
                else if (hue > .65f && hue < .9f)
                {
                    if (y > 0)
                    {
                        if (hasMoved[i - (xSize + 1)] == true)
                        {
                            continue;
                        }

                        float th = 0;
                        float tv = 0;
                        float ts = 0;
                        Color.RGBToHSV(colors[i - (xSize + 1)], out th, out tv, out ts);

                        if (val < tv)
                        {
                            //SwapColors(i, i - (xSize + 1));
                        }
                        else
                        {
                            //SwapColors(i + (xSize + 1), 1);
                        }
                    }

                }
                else if (hue < .1f || hue > .9f)
                {
                    //RED
                    if (x > 0)
                    {
                        if (hasMoved[i - 1] == true)
                        {
                            continue;
                        }

                        float th = 0;
                        float tv = 0;
                        float ts = 0;
                        Color.RGBToHSV(colors[i - 1], out th, out tv, out ts);

                        if (hue > th)
                        {
                            //SwapColors(i, i - 1);
                        }
                        else
                        {
                            //SwapColors(i + 1, i);
                        }


                    }
                }

            }
        }

        mesh.colors = colors;
    }

    public void realWater()
    {
        for (int i = 0; i < hasMoved.Length; i++)
        {
            hasMoved[i] = false;
        }

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {

                if (hasMoved[i] == true)
                {
                    continue;
                }else if (y == 0)
                {
                    //colors[i] = Color.black;
                }else if (y == ySize)
                {
                    //colors[i] = Color.black;
                }
                else
                {
                    Color c = colors[i];
                    Color below = colors[i - (xSize + 1)];

                    //if (c.b > below.b + .1f)
                    if (c.b > below.b)
                    {
                        if (hasMoved[i - (xSize + 1)] == false)
                        SwapColors(i, i - (xSize + 1));
                        continue;
                    }else
                    {

                        if (x == 0)
                        {
                            continue;
                        }

                        if (hasMoved[i] == true)
                        {
                           // continue;
                        }

                        Color leftBelow = colors[i - 1];

                        if (c.b > leftBelow.b)
                         {
                            if (hasMoved[i-1]== true)
                            {
                               continue;
                            }

                            SwapColors(i, i - 1);
                            continue;
                        }

                        if (x == xSize)
                        {
                            continue;
                        }


                        if (hasMoved[i] == true)
                        {
                            continue;
                        }

                        if (i > colors.Length - 1)
                        {
                            continue;
                        }

                        leftBelow = colors[i + 1];
                        if (c.b > leftBelow.b)
                        {
                            if (hasMoved[i + 1] == true)
                            {
                                
                                continue;
                                
                            }


                            SwapColors(i, i + 1);
                            continue;
                        }



                    }
                }
            }
        }

        mesh.colors = colors;
    }

    public void Water()
    {
        float blackThresh = .03f;
        float rand = Random.Range(0, 100);
        Color c = colors[0];
        Color below = colors[0];

        for (int i = 0; i < hasMoved.Length; i++)
        {
            hasMoved[i] = false;
        }

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                if (hasMoved[i] == true)
                {
                   // continue;
                }

                 rand = Random.Range(0, 100);

                if (rand < 50 && y > 0)
                {
                    //whiterises
                    

                    c = colors[i];
                    below = colors[i - (xSize + 1)];

                    if (c.r + c.b + c.g < blackThresh)
                    {
                        continue;
                    }

                    if (below.r + below.b + below.g < blackThresh)
                    {
                        continue;
                    }

                    if (c.r  > below.r)
                    {
                        if (y > 0 && hasMoved[i - (xSize + 1)] == false)
                        {
                            SwapColors(i, i - (xSize + 1));
                            continue;
                        }
                    }

                }
                else if (rand < 75 && x < xSize)
                {
                    c = colors[i];
                    below = colors[i + 1];

                    if (c.r + c.b + c.g < blackThresh)
                    {
                        continue;
                    }

                    if (below.r + below.b + below.g < blackThresh)
                    {
                        continue;
                    }

                    if (c.b > below.b)
                    {
                        if (hasMoved[i + 1] == false)
                        {
                           SwapColors(i,i+1);
                            
                        }
                    }

                }
                else if (rand < 100 && x > 0)
                {
                    c = colors[i];
                    below = colors[i - 1];

                    if (c.r + c.b + c.g < blackThresh)
                    {
                        continue;
                    }

                    if (below.r + below.b + below.g < blackThresh)
                    {
                        continue;
                    }

                    if (c.g > below.g)
                    {
                        if (hasMoved[i - 1] == false)
                        {
                            SwapColors(i, i - 1);
                            //break;
                        }
                    }

                }
            }
        }

        mesh.colors = colors;
    }

    public void BlackoutImage()
    {
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                colors[i] = Color.black;
            }
        }
        mesh.colors = colors;
    }

    public void NoiseImage()
    {
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                if (matched[i] == true)
                {
                    continue;
                }
                else
                {
                    colors[i] = Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                }
                
            }
        }
        mesh.colors = colors;
    }

    public void CheckIfCorrect()
    {
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                if (matched[i] == true)
                {
                    continue;
                }
                else if (returnIfColorIsCorrect(colors[i], targetColors[i], matchThresh))
                {
                    //colors[i] = Color.white;
                    matched[i] = true;
                    unmatchedIds.Remove(i);
                }
                
            }
        }

        mesh.colors = colors;
    }

    public bool returnIfColorIsCorrect(Color orig, Color targ, float thresh)
    {
        float r = orig.r - targ.r;
        float g = orig.g - targ.g;
        float b = orig.b - targ.b;

        r = Mathf.Abs(r);
        g = Mathf.Abs(r);
        b = Mathf.Abs(r);

        if (r < thresh && g < thresh && b < thresh)
        {
            //return true;
        }

        if (orig.r > targ.r - thresh && orig.r < targ.r + thresh)
        {
            if (orig.g > targ.g - thresh && orig.g < targ.g + thresh)
            {
                if (orig.b > targ.b - thresh && orig.b < targ.b + thresh)
                {
                    return true;
                }
            }

        }


        return false;
    }

    public void search2()
    {
        int lastOpen = 0;

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                if (matched[i] == true)
                {
                    continue;
                }
                else
                {
                    int targ = listReturnNext(i);
                    //int targ = returnNextId(lastOpen);
                    if (targ == -1000)
                    {
                        //isSearching = false;
                    }
                    else
                    {
                        lastOpen = targ;
                        SwapColors(i, targ);
                    }
                }
            }
        }

        CheckIfCorrect();
        mesh.colors = colors;
    }

    public int listReturnNext(int _i)
    {
        return unmatchedIds[0];
    }

    public int returnNextId(int _i)
    {
        _i++;
        if (_i > matched.Length)
        {
            _i = 0;
        }

        for (int i = _i;i < matched.Length; i++)
        {
            if (matched[i] == true)
            {

            }
            else
            {
                return i;
            }
        }

        for (int i = 0; i < matched.Length; i++)
        {
            if (matched[i] == true)
            {

            }
            else if (i == _i)
            {
                return -1000;
            }
            else
            {
                return i;
            }
        }

        return -1000;
    }

    public int returnIdOfNextOpenCell(int _x, int _y, int _i)
    {
        for (int i = _i, y = _y; y <= ySize; y++)
        {
            for (int x = _x; x <= xSize; x++, i++)
            {
                if (matched[i] == true)
                {
                    continue;
                }
                else
                {
                    return i;
                }
            }
        }

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                if (matched[i] == true)
                {
                    continue;
                }
                else
                {
                    return i;
                }
            }
        }
        isSearching = false;
        return -1000;
    }

    public void searchers()
    {
        for (int i = 0; i < hasMoved.Length; i++)
        {
            hasMoved[i] = false;
        }

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                if (matched[i] == true)
                {
                    continue;
                }else if (hasMoved[i] == true)
                {
                    continue;
                }
                else
                {
                    float r = Random.Range(0, 100);
                    if (r < 2)
                    {
                        colors[i] = Color.Lerp(colors[i], targetColors[i], .01f);
                    }
                    else if (r < 25)
                    {
                        //move right if possible;
                        if (x > 0)
                        {
                            if (hasMoved[i-1] == false && matched[i-1] == false)
                            {
                                SwapColors(i, i - 1);
                            }
                        }
                    }else if (r < 50)
                    {
                        //move right if possible;
                        if (x < xSize-1)
                        {
                            if (hasMoved[i + 1] == false && matched[i + 1] == false)
                            {
                                SwapColors(i, i + 1);
                            }
                        }
                    }
                    else if (r < 75)
                    {
                        //move right if possible;
                        if (y < ySize - 1)
                        {
                            if (hasMoved[i + (ySize + 1)] == false && matched[i + (ySize + 1)] == false)
                            {
                                SwapColors(i, i + (ySize + 1));
                            }
                        }
                    }
                    else if (r < 100)
                    {
                        //move right if possible;
                        if (y > 0)
                        {
                            if (hasMoved[i - (ySize + 1)] == false && matched[i - (ySize + 1)] == false)
                            {
                                SwapColors(i, i - (ySize + 1));
                            }
                        }
                    }

                }

            }
        }

        CheckIfCorrect();
        mesh.colors = colors;
    }

    public bool isPixelOpenForWater(int i)
    {
        float hue = 0;
        float sat = 0;
        float val = 0;
        Color.RGBToHSV(colors[i], out hue, out sat, out val);

        if (val < .2f)
        {
            return true;
        }
        return false;
    }

    public void HueWater()
    {


        for (int i = 0; i < hasMoved.Length; i++)
        {
            hasMoved[i] = false;
        }

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                if (hasMoved[i] == true)
                {
                    continue;
                }

                if (Random.Range(0,10) < 2)
                {
                    continue;
                }

                if ( x == 0 || x == xSize || y == 0 || y == ySize)
                {
                    continue;
                }

                Color c = colors[i];
                float hue = 0;
                float sat = 0;
                float val = 0;
                Color.RGBToHSV(c, out hue, out sat, out val);

                if (hue > lowWaterHueValue && hue < highWaterHueValue)
                {
                    //for blue



                    //below color
                    float th, ts, tv;
                    Color.RGBToHSV(colors[i - (xSize + 1)], out th, out ts, out tv);

                    //if blue is below, darkher blue sink
                    if (th > lowWaterHueValue && th < highWaterHueValue)
                    {
                        if (val < tv)
                        {
                            if (hasMoved[i - (xSize + 1)] == true) { continue; }
                            SwapColors(i, i - (xSize + 1));
                            continue;

                        }
                    }

                    //if below is dark, occupy it
                    if (tv < darkThresh)
                    {
                       if (hasMoved[i - (xSize + 1)] == true) { continue; }
                        SwapColors(i, i - (xSize + 1));
                        continue;
                    }

                    //bool leftOpen = isPixelOpenForWater(i - 1);
                    //bool rightOpen = isPixelOpenForWater(1 + 1);

                    float lh, ls, lv;
                    float rh, rs, rv;
                    Color.RGBToHSV(colors[i-1], out lh, out ls, out lv);
                    Color.RGBToHSV(colors[i+1], out rh, out rs, out rv);

                    bool leftOpen = false;
                    bool rightOpen = false;

                    

                    if (lv < darkThresh)
                    {
                        leftOpen = true;
                    }else if (lh > lowWaterHueValue && lh < highWaterHueValue && val > lv)
                    {
                        leftOpen = true;
                    }

                    if (rv < darkThresh)
                    {
                        rightOpen = true;
                    }
                    else if (rh > lowWaterHueValue && rh < highWaterHueValue && val > rv)
                    {
                        rightOpen = true;
                    }


                    if (leftOpen == true && rightOpen == false)
                    {
                        if (hasMoved[i -1] == true) { continue; }
                        SwapColors(i, i - 1);
                        continue;
                    }
                    else if (leftOpen == false && rightOpen == true)
                    {
                        if (hasMoved[i + 1] == true) { continue; }
                        SwapColors(i, i + 1);
                        continue;
                    }
                    else if (leftOpen == true && rightOpen == true)
                    {

                        
                        if (Random.Range(0,10) >5)
                        {
                            if (hasMoved[i - 1] == true) { continue; }
                            SwapColors(i, i - 1);
                            continue;
                        }
                        else
                        {
                            if (hasMoved[i + 1] == true) { continue; }
                            SwapColors(i, i + 1);
                            continue;
                        }
                    }

                }
            }
        }
                mesh.colors = colors;
    }

    public void KindOfLikeSand()
    {
        for (int i = 0; i < hasMoved.Length; i++)
        {
            hasMoved[i] = false;
        }
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                if (hasMoved[i] == true)
                {
                    continue;
                }

                if (x == 0 || x == xSize || y == 0 || y == ySize)
                {
                    continue;
                }

                Color c = colors[i];
                float hue = 0;
                float sat = 0;
                float val = 0;
                Color.RGBToHSV(c, out hue, out sat, out val);

                float r = Random.Range(0, 20);
                if (r < 2) { continue; }

                if (hue > .3f && hue < .8f)
                {
                    //blue

                    //is below blue?
                    float th, ts, tv;
                    Color.RGBToHSV(colors[i - (xSize + 1)], out th, out ts, out tv);
                    //if blue is below, darkher blue sink
                    if (th > .3f && th < .8f)
                    {
                        if (val < tv)
                        {
                            if (hasMoved[i - (xSize + 1)] == false)
                            {
                                SwapColors(i, i - (xSize + 1));
                                continue;
                            }
                        }
                    }

                    //if below is dark, occupy it
                    if (tv < .2f)
                    {
                        if (hasMoved[i - (xSize + 1)] == false)
                        {
                            SwapColors(i, i - (xSize + 1));
                            continue;
                        }
                    }


                    //if left is dark, occupy it
                    Color.RGBToHSV(colors[i - 1], out th, out ts, out tv);
                    if (tv < .2f)
                    {

                        if (hasMoved[i - 1] == false)
                        {
                            SwapColors(i, i - 1);
                            continue;
                        }

                    }

                    //if right is dark, occupy it
                    Color.RGBToHSV(colors[i + 1], out th, out ts, out tv);
                    if (tv < .2f)
                    {
                        if (hasMoved[i + 1] == false)
                        {
                            SwapColors(i, i + 1);
                            continue;
                        }
                    }

                }


            }
        }


        mesh.colors = colors;
    }

    public void Update()
    {
        //KindOfLikeSand();
        HueWater();
        //HueSpecificTest();
        //HueTest();
        //Water();
        //realWater();
        if (Input.GetKeyDown(KeyCode.B))
        {
            ScreenCapture.CaptureScreenshot("SomeLevel" + Time.frameCount + ".jpg",4);
            //BlackoutImage();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NoiseImage();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CheckIfCorrect();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NewTarget();
        }

        if (isSearching == true)
        {
            if (Input.GetKey(KeyCode.A))
            {
                search2();
            }
            else
            {
                searchers();
            }

            
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (isSearching == true)
            {
                isSearching = false;
            }
            else
            {
                isSearching = true;
            }
        }
    }

    public void Old()
    {

        for (int i = 0; i < hasMoved.Length; i++)
        {
            hasMoved[i] = false;
        }

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                if (hasMoved[i] == true)
                {

                }
                else
                {
                    float rand = Random.Range(0, 10);

                    if (rand < 0)
                    {

                    }else if (rand < 4)
                    {
                        int aboveSpot = i + xSize+1;
                        if (aboveSpot < colors.Length)
                        {
                            if (isTopWarmer(i, aboveSpot))
                            {
                                SwapColors(i, aboveSpot);
                            }

                        }
                    }
                    else if (rand < 7)
                    {
                        int rightSpot = i +1;
                        if (rightSpot < colors.Length)
                        {
                            if (isRightBluer(i, rightSpot))
                            {
                                SwapColors(i, rightSpot);
                            }

                        }
                    }
                    else if (rand < 10)
                    {
                        int leftSpot = i - 1;
                        if (leftSpot > 0)
                        {
                            if (isLeftGreener(i, leftSpot))
                            {
                                SwapColors(i, leftSpot);
                            }

                        }
                    }
                }
            }
        }

        mesh.colors = colors;
    }

    public void SwapColors(int orig, int targ)
    {

            Color _orig = colors[orig];
            Color _targ = colors[targ];
            colors[targ] = _orig;
            colors[orig] = _targ;
            hasMoved[orig] = true;
            hasMoved[targ] = true;
    }

    public bool isTopWarmer(int orig, int targ)
    {
        if (colors[orig].r > colors[targ].r)
        {
            return true;
        }

        return false;
    }

    public bool isRightBluer(int orig, int targ)
    {
        if (colors[orig].b > colors[targ].b)
        {
            return true;
        }

        return false;
    }

    public bool isLeftGreener(int orig, int targ)
    {
        if (colors[orig].g > colors[targ].g)
        {
            return true;
        }

        return false;
    }

    public void makeThatCoolLandscapeThingWithJitters()
    {
        for (int i = 0; i < hasMoved.Length; i++)
        {
            hasMoved[i] = false;
        }

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                if (hasMoved[i] == true)
                {
                    continue;
                }
                else if (y == 0)
                {
                    //colors[i] = Color.black;
                }
                else if (y == ySize)
                {
                    //colors[i] = Color.black;
                }
                else
                {
                    Color c = colors[i];
                    Color below = colors[i - (xSize + 1)];

                    //if (c.b > below.b + .1f)
                    if (c.b > below.b)
                    {
                        if (hasMoved[i - (xSize + 1)] == false)
                            SwapColors(i, i - (xSize + 1));
                        continue;
                    }
                    else
                    {

                        if (x == 0)
                        {
                            continue;
                        }

                        if (hasMoved[i] == true)
                        {
                            continue;
                        }

                        Color leftBelow = colors[i - 1];

                        if (c.b > leftBelow.b)
                        {
                            if (hasMoved[i - 1] == true)
                            {
                                continue;
                            }

                            float r = Random.Range(0, 20);
                            if (r < 0)
                            {
                                //this stops all jitter and makes it actually sort
                                continue;
                            }
                            SwapColors(i, i - 1);
                            continue;
                        }

                        if (x == xSize)
                        {
                            continue;
                        }


                        if (hasMoved[i] == true)
                        {
                            continue;
                        }

                        if (i > colors.Length - 1)
                        {
                            continue;
                        }

                        leftBelow = colors[i + 1];
                        if (c.b > leftBelow.b)
                        {
                            if (hasMoved[i + 1] == true)
                            {

                                continue;

                            }
                            SwapColors(i, i + 1);
                            continue;
                        }



                    }
                }
            }
        }

        mesh.colors = colors;
    }

    public void CentreHue()
    {
        Vector2 centre = new Vector2(xSize / 2, ySize / 2);

        for (int i = 0; i < hasMoved.Length; i++)
        {
            hasMoved[i] = false;
        }

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {

                if (hasMoved[i] == true)
                {
                    //continue;
                }

                if (x == 0 || x == xSize || y < 2 || y > ySize - 2)
                {
                    continue;
                }

                Color c = colors[i];
                float hue = 0;
                float sat = 0;
                float val = 0;
                Color.RGBToHSV(c, out hue, out sat, out val);

                Vector2 target = new Vector2(0, 0);


                if (x >= xSize / 2) { target.x = -1; }
                else if (x < xSize / 2) { target.x = 1; }

                if (y >= ySize / 2) { target.y = -1; }
                else if (y < ySize / 2) { target.y = 1; }



                float r = Random.Range(0, 10);

                if (r < 4)
                {
                    //if (target.x != 0)
                    if (1 == 1)
                    {
                        float th, ts, tv;
                        Color.RGBToHSV(colors[i + (int)target.x], out th, out ts, out tv);

                        if (hue > th)
                        {
                            if (hasMoved[i + (int)target.x] == false)
                            {
                                SwapColors(i, i + (int)target.x);
                            }
                        }
                    }
                }
                else if (r < 8)
                {
                    //if (target.y != 0)
                    if (1 == 1)
                    {
                        int tId = i + (xSize + 1);
                        if (target.y == -1)
                        {
                            tId = i - (xSize + 1);
                        }
                        float th, ts, tv;
                        Color.RGBToHSV(colors[tId], out th, out ts, out tv);

                        if (val > tv)
                        {
                            if (hasMoved[tId] == false)
                            {
                                SwapColors(i, tId);
                            }

                        }
                    }
                }




            }
        }


        mesh.colors = colors;
    }
}