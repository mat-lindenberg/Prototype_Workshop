using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MatImageGrid : MonoBehaviour
{

    public int xSize, ySize;

    public Mesh mesh;
    public Vector3[] vertices;
    public Color[] colors;

    public Texture2D tex;
    public Texture2D targetTex;

    public float[] randomArray;
    public int placeInRandomArray;

    public Texture2D[] textures;
    public int placeInTextureArray;
    public bool[] hasMoved;

    public float[] hues;
    public float[] saturations;
    public float[] values;

    bool leftOpen = false;
    bool rightOpen = false;

    public float matchThresh;

    public Camera mainCamera;

    public float darkThresh = .2f;
    public float lowWaterHueValue = .3f;
    public float highWaterHueValue = .8f;

    public float lowSandHue;
    public float highSandHue;

    public float lowFireHue;
    public float highFireHue;

    public float lowPlantHue;
    public float highPlantHue;

    public void Awake()
    {
        Generate();
    }

    public void Generate()
    {
        randomArray = new float[10];
        SetNewRandomArray();

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        colors = new Color[(xSize + 1) * (ySize + 1)];

        hasMoved = new bool[(xSize + 1) * (ySize + 1)];

        saturations = new float[(xSize + 1) * (ySize + 1)];
        hues = new float[(xSize + 1) * (ySize + 1)];
        values = new float[(xSize + 1) * (ySize + 1)];

        float h, s, v;

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

                Color.RGBToHSV(colors[i], out h, out s, out v);
                saturations[i] = s;
                hues[i] = h;
                values[i] = v;


            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;
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
        //mesh.RecalculateNormals();

        mainCamera.transform.position = new Vector3((float)xSize / 2f, (float)ySize / 2f, -10);
        mainCamera.orthographicSize = ((float)xSize / 2f) + 2;
    }

    public void NewTarget()
    {
        targetTex = textures[placeInTextureArray];
        placeInTextureArray++;
        if (placeInTextureArray > textures.Length - 1)
        {
            placeInTextureArray = 0;
        }
    }

    public void SetNewRandomArray()
    {
        for (int i = 0; i < randomArray.Length; i++)
        {
            randomArray[i] = Random.Range(0, 10);
        }
    }

    public float GetNewRandomNumber()
    {
        placeInRandomArray++;
        if (placeInRandomArray > randomArray.Length - 1)
        {
            placeInRandomArray = 0;
        }

        return randomArray[placeInRandomArray];
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
                        if (hasMoved[i - 1] == false)
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

                        if (hue > th)
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
                            // continue;
                        }

                        Color leftBelow = colors[i - 1];

                        if (c.b > leftBelow.b)
                        {
                            if (hasMoved[i - 1] == true)
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

                    if (c.r > below.r)
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
                            SwapColors(i, i + 1);

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

                colors[i] = Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));


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

    public void WeirdEqualibirum()
    {
        SetNewRandomArray();

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

                if (x == 0 || x == xSize || y == 0 || y == ySize)
                {
                    continue;
                }

                // Color c = colors[i];
                float hue = hues[i];
                float sat = saturations[i];
                float val = values[i];
                //Color.RGBToHSV(c, out hue, out sat, out val);

                if (hue > lowWaterHueValue && hue < highWaterHueValue)
                {
                    //for blue
                    //below color
                    float th, ts, tv;
                    th = hues[i - (xSize + 1)];
                    ts = saturations[i - (xSize + 1)];
                    tv = values[i - (xSize + 1)];
                    //Color.RGBToHSV(colors[i - (xSize + 1)], out th, out ts, out tv);

                    //if blue is below, darkher blue sink
                    if (th > lowWaterHueValue && th < highWaterHueValue && tv > darkThresh)
                    {
                        if (val < tv)
                        {
                            if (hasMoved[i - (xSize + 1)] == true) { continue; }
                            SwapColors(i, i - (xSize + 1));
                            continue;

                        }
                    }

                    if (tv > darkThresh && values[i + 1] < darkThresh)
                    {
                        SwapColors(i, i + 1);
                        continue;
                    }

                    if (tv > darkThresh && values[i - 1] < darkThresh)
                    {
                        SwapColors(i, i - 1);
                        continue;
                    }

                    //if below is dark, occupy it
                    if (tv <= darkThresh)
                    {
                        if (hasMoved[i - (xSize + 1)] == true) { continue; }
                        SwapColors(i, i - (xSize + 1));
                        continue;
                    }

                    //check if two next are dark, and spread out
                    if (x < xSize - 2 && values[i + 2] < darkThresh && values[i + 1] < darkThresh)
                    {
                        SwapColors(i, i + 1);
                        continue;
                    }

                    if (x > 1 && values[i - 2] < darkThresh && values[i - 1] < darkThresh)
                    {
                        SwapColors(i, i - 1);
                        continue;
                    }

                    if (x > 1 && y > 1 && values[i - (xSize)] < darkThresh)
                    {
                        if (hasMoved[i - (xSize)] == true) { continue; }
                        SwapColors(i, i - (xSize));
                        continue;
                    }

                    if (x > 1 && y > 1 && values[i - (xSize + 2)] < darkThresh)
                    {
                        if (hasMoved[i - (xSize + 2)] == true) { continue; }
                        SwapColors(i, i - (xSize + 2));
                        continue;
                    }




                    //bool leftOpen = isPixelOpenForWater(i - 1);
                    //bool rightOpen = isPixelOpenForWater(1 + 1);

                    float lh, ls, lv;
                    float rh, rs, rv;

                    lh = hues[i - 1];
                    ls = saturations[i - 1];
                    lv = values[i - 1];

                    rh = hues[i + 1];
                    rs = saturations[i + 1];
                    rv = values[i + 1];
                    //Color.RGBToHSV(colors[i - 1], out lh, out ls, out lv);
                    //Color.RGBToHSV(colors[i + 1], out rh, out rs, out rv);

                    //priority order - fill dark to the left and right first
                    leftOpen = false;
                    rightOpen = false;

                    if (lv < darkThresh)
                    {
                        leftOpen = true;
                    }
                    if (rv < darkThresh)
                    {
                        rightOpen = true;
                    }

                    if (leftOpen == true && rightOpen == false)
                    {
                        if (hasMoved[i - 1] == true) { continue; }
                        SwapColors(i, i - 1);
                        continue;
                    }
                    else if (leftOpen == true && rightOpen == true)
                    {
                        if (GetNewRandomNumber() > 5)
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
                    else if (leftOpen == false && rightOpen == true)
                    {
                        if (hasMoved[i + 1] == true) { continue; }
                        SwapColors(i, i + 1);
                        continue;
                    }

                    leftOpen = false;
                    rightOpen = false;

                    if (lh > lowWaterHueValue && lh < highWaterHueValue && val > lv)
                    {
                        leftOpen = true;
                    }
                    if (rh > lowWaterHueValue && rh < highWaterHueValue && val > rv)
                    {
                        rightOpen = true;
                    }


                    if (leftOpen == true && rightOpen == false)
                    {
                        if (hasMoved[i - 1] == true) { continue; }
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
                        if (GetNewRandomNumber() > 5)
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

    public void FireTest()
    {
        SetNewRandomArray();

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

                if (x <= 2 || x == xSize - 1 || y <= 5 || y == ySize - 1)
                {
                    continue;
                }
                // Color c = colors[i];
                float hue = hues[i];
                float sat = saturations[i];
                float val = values[i];

                if (hue > lowPlantHue && hue < highPlantHue && val > darkThresh)
                {

                    if (values[i + ((xSize + 1) * 1)] < darkThresh)
                    {
                        if (hasMoved[i + (xSize + 1)] == true) { continue; }
                        SwapColors(i, i + ((xSize + 1) * 1));
                        continue;
                    }

                }
            }
        }

        mesh.colors = colors;
    }

    public void AllTest()
    {
        SetNewRandomArray();


        for (int i = 0; i < hasMoved.Length; i++)
        {
            hasMoved[i] = false;
        }

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                float hue = hues[i];
                float sat = saturations[i];
                float val = values[i];

                if ( x < 4 || y < 4 || x > xSize-4 || y > ySize - 4)
                {
                    continue;
                }

                if (val < darkThresh)
                {
                    continue;
                }

                if (hue > lowWaterHueValue && hue < highWaterHueValue)
                {
                    WaterLogic(i, hue, sat, val);
                }else if ( hue > lowSandHue && hue < highSandHue)
                {
                    SandLogic(i, hue, sat, val);
                }else if (hue > lowPlantHue && hue < highPlantHue)
                {
                    PlantLogic(i, hue, sat, val);
                }

            }
        }
                mesh.colors = colors;

    }

    public void SandLogic(int i, float hue, float sat, float val)
    {
        if (values[i - (xSize + 1)] < darkThresh && values[i - (xSize + 1)] > lowWaterHueValue && values[i - (xSize + 1)] < highWaterHueValue)
        {
            //if (hasMoved[i - (xSize + 1)] == true) { continue; }
            SwapColors(i, i - (xSize + 1));
            return;
        }


        //move down three
        if (values[i - (xSize + 1)] < darkThresh && values[i - ((xSize + 1) * 4)] < darkThresh)
        {
            //if (hasMoved[i - (xSize + 1)] == true) { continue; }
            SwapColors(i, i - ((xSize + 1) * 4));
            return;
        }

        //move down three
        if (values[i - (xSize + 1)] < darkThresh && values[i - ((xSize + 1) * 3)] < darkThresh)
        {
            //if (hasMoved[i - (xSize + 1)] == true) { continue; }
            SwapColors(i, i - ((xSize + 1) * 3));
            return;
        }

        //if below void, fall
        if (values[i - (xSize + 1)] < darkThresh)
        {
            //if (hasMoved[i - (xSize + 1)] == true) { continue; }
            SwapColors(i, i - (xSize + 1));
            return;
        }

        //move down two
        if (values[i - (xSize + 1)] > darkThresh && values[i - ((xSize + 1) * 2)] < darkThresh)
        {
            //if (hasMoved[i - (xSize + 1)] == true) { continue; }
            SwapColors(i, i - ((xSize + 1) * 2));
            return;
        }


        //move right
        if (values[i - (xSize + 1)] > darkThresh && values[i + 1] < darkThresh && values[i + (xSize + 1)] > darkThresh)
        {
            if (hasMoved[i + 1] == true) { return; }
            SwapColors(i, i + 1);
            return;
        }

        //move right if top & bottom full and two right is clear
        if (values[i - (xSize + 2)] > darkThresh && values[i + 2] < darkThresh && values[i + (xSize + 2)] > darkThresh)
        {
            if (hasMoved[i + 2] == true) { return; }
            SwapColors(i, i + 2);
            return;
        }


        if (values[i - (xSize + 1)] > darkThresh && values[i - 1] < darkThresh && values[i + (xSize + 1)] > darkThresh)
        {
            if (hasMoved[i - 1] == true) { return; }
            SwapColors(i, i - 1);
            return;
        }

        if (values[i - (xSize + 2)] > darkThresh && values[i - 2] < darkThresh && values[i + (xSize + 2)] > darkThresh)
        {
            if (hasMoved[i - 2] == true) { return; }
            SwapColors(i, i - 2);
            return;
        }
    }

    public void WaterLogic(int i, float hue, float sat, float val)
    {
        //for blue
        //below color
        float th, ts, tv;
        th = hues[i - (xSize + 1)];
        ts = saturations[i - (xSize + 1)];
        tv = values[i - (xSize + 1)];
        //Color.RGBToHSV(colors[i - (xSize + 1)], out th, out ts, out tv);

        //if blue is below, darkher blue sink
        if (th > lowWaterHueValue && th < highWaterHueValue && tv > darkThresh)
        {
            if (val < tv)
            {
                if (hasMoved[i - (xSize + 1)] == true) { return; }
                SwapColors(i, i - (xSize + 1));
                return;

            }
        }

        if (tv > darkThresh && values[i + 1] < darkThresh)
        {
            SwapColors(i, i + 1);
            return;
        }

        if (tv > darkThresh && values[i - 1] < darkThresh)
        {
            SwapColors(i, i - 1);
            return;
        }

        //if below is dark, occupy it
        if (tv <= darkThresh)
        {
            if (hasMoved[i - (xSize + 1)] == true) { return; }
            SwapColors(i, i - (xSize + 1));
            return;
        }

        //check if two next are dark, and spread out
        if (values[i + 2] < darkThresh && values[i + 1] < darkThresh)
        {
            SwapColors(i, i + 1);
            return;
        }

        if (values[i - 2] < darkThresh && values[i - 1] < darkThresh)
        {
            SwapColors(i, i - 1);
            return;
        }

        //make sand

        if (values[i - (xSize)] < darkThresh)
        {
            if (hasMoved[i - (xSize)] == true) { return; }
            SwapColors(i, i - (xSize));
            return;
        }

        if (values[i - (xSize + 2)] < darkThresh)
        {
            if (hasMoved[i - (xSize + 2)] == true) { return; }
            SwapColors(i, i - (xSize + 2));
            return;
        }




        //bool leftOpen = isPixelOpenForWater(i - 1);
        //bool rightOpen = isPixelOpenForWater(1 + 1);

        float lh, ls, lv;
        float rh, rs, rv;

        lh = hues[i - 1];
        ls = saturations[i - 1];
        lv = values[i - 1];

        rh = hues[i + 1];
        rs = saturations[i + 1];
        rv = values[i + 1];
        //Color.RGBToHSV(colors[i - 1], out lh, out ls, out lv);
        //Color.RGBToHSV(colors[i + 1], out rh, out rs, out rv);

        //priority order - fill dark to the left and right first
        leftOpen = false;
        rightOpen = false;

        if (lv < darkThresh)
        {
            leftOpen = true;
        }
        if (rv < darkThresh)
        {
            rightOpen = true;
        }

        if (leftOpen == true && rightOpen == false)
        {
            if (hasMoved[i - 1] == true) { return; }
            SwapColors(i, i - 1);
            return;
        }
        else if (leftOpen == true && rightOpen == true)
        {
            if (GetNewRandomNumber() > 5)
            {
                if (hasMoved[i - 1] == true) { return; }
                SwapColors(i, i - 1);
                return;
            }
            else
            {
                if (hasMoved[i + 1] == true) { return; }
                SwapColors(i, i + 1);
                return;
            }
        }
        else if (leftOpen == false && rightOpen == true)
        {
            if (hasMoved[i + 1] == true) { return; }
            SwapColors(i, i + 1);
            return;
        }

        leftOpen = false;
        rightOpen = false;

        if (lh > lowWaterHueValue && lh < highWaterHueValue && val > lv)
        {
            leftOpen = true;
        }
        if (rh > lowWaterHueValue && rh < highWaterHueValue && val > rv)
        {
            rightOpen = true;
        }


        if (leftOpen == true && rightOpen == false)
        {
            if (hasMoved[i - 1] == true) { return; }
            SwapColors(i, i - 1);
            return;
        }
        else if (leftOpen == false && rightOpen == true)
        {
            if (hasMoved[i + 1] == true) { return; }
            SwapColors(i, i + 1);
            return;
        }
        else if (leftOpen == true && rightOpen == true)
        {
            if (GetNewRandomNumber() > 5)
            {
                if (hasMoved[i - 1] == true) { return; }
                SwapColors(i, i - 1);
                return;
            }
            else
            {
                if (hasMoved[i + 1] == true) { return; }
                SwapColors(i, i + 1);
                return;
            }
        }
    }

    public void PlantLogic(int i, float hue, float sat, float val)
    {
        float r = GetNewRandomNumber();

        if (returnPlantCellCount(i) == 2)
            {
                //SetColor(i, Color.green);
                //continue;
            }

            if (returnPlantCellCount(i) == 3)
            {
                return;
            }

            if (returnPlantCellCount(i) == 4)
            {
                SetColor(i, Color.black);
            }

            if (returnPlantCellCount(i) == 5)
            {
                SetColor(i, Color.black);
            }

            if (returnPlantCellCount(i) == 6)
            {
                SetColor(i, Color.black);
            }

            if (returnPlantCellCount(i) == 7)
            {
                SetColor(i, Color.green);
            }

            if (returnPlantCellCount(i) == 8)
            {
                SetColor(i, Color.black);
            }

            if (r == 1)
            {
                if (values[i + (xSize)] < darkThresh)
                {
                    PlantAddColor(i, i + (xSize), colors[i]);
                }
            }
            else if (r == 2)
            {
                if (values[i + (xSize + 1)] < darkThresh)
                {
                    PlantAddColor(i, i + (xSize + 1), colors[i]);
                }
            }
            else if (r == 3)
            {
                if (values[i + (xSize + 2)] < darkThresh)
                {
                    PlantAddColor(i, i + (xSize + 2), colors[i]);
                }
            }
            else if (r == 4)
            {
                if (values[i + 1] < darkThresh)
                {
                    PlantAddColor(i, i + 1, colors[i]);
                }
            }
            else if (r == 5)
            {
                if (values[i - (xSize + 2)] < darkThresh)
                {
                    PlantAddColor(i, i - (xSize + 2), colors[i]);
                }
            }
            else if (r == 6)
            {
                if (values[i - (xSize + 1)] < darkThresh)
                {
                    PlantAddColor(i, i - (xSize + 1), colors[i]);
                }
            }
            else if (r == 7)
            {
                if (values[i - (xSize)] < darkThresh)
                {
                    PlantAddColor(i, i - (xSize), colors[i]);
                }
            }
            else if (r == 8)
            {
                if (values[i - 1] < darkThresh)
                {
                    PlantAddColor(i, i - 1, colors[i]);
                }
            }
        }



        public int returnPlantCellCount(int i)
    {
        int toReturn = 0;

        if (values[i+1] > darkThresh)
        {
            toReturn++;
        }
        if (values[i - 1] > darkThresh)
        {
            toReturn++;
        }
        if (values[i + (xSize)] > darkThresh)
        {
            toReturn++;
        }
        if (values[i + (xSize+ 1)] > darkThresh)
        {
            toReturn++;
        }
        if (values[i + (xSize + 2)] > darkThresh)
        {
            toReturn++;
        }
        if (values[i - (xSize)] > darkThresh)
        {
            toReturn++;
        }
        if (values[i - (xSize+ 1)] > darkThresh)
        {
            toReturn++;
        }
        if (values[i - (xSize + 2)] > darkThresh)
        {
            toReturn++;
        }

        return toReturn;
    }

    public void SandTest()
    {
        SetNewRandomArray();

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
                    //if (hasMoved[i - (xSize + 1)] == true) { continue; }
                    SwapColors(i, i - ((xSize + 1) * 5));
                    continue;
                }

                if (x <= 2 || x == xSize-1 || y <= 5 || y == ySize - 1)
                {
                    continue;
                }
                // Color c = colors[i];
                float hue = hues[i];
                float sat = saturations[i];
                float val = values[i];

                if (hue > lowSandHue && hue < highSandHue && val > darkThresh)
                {

                    //move down three
                    if (values[i - ((xSize + 1) * 5)] < darkThresh)
                    {
                        //if (hasMoved[i - (xSize + 1)] == true) { continue; }
                        SwapColors(i, i - ((xSize + 1) * 5));
                        continue;
                    }

                    //move down three
                    if (values[i - (xSize + 1)] < darkThresh && values[i - ((xSize + 1) * 4)] < darkThresh)
                    {
                        //if (hasMoved[i - (xSize + 1)] == true) { continue; }
                        SwapColors(i, i - ((xSize + 1) * 4));
                        continue;
                    }

                    //move down three
                    if (values[i - (xSize + 1)] < darkThresh && values[i - ((xSize + 1) * 3)] < darkThresh)
                    {
                        //if (hasMoved[i - (xSize + 1)] == true) { continue; }
                        SwapColors(i, i - ((xSize + 1) * 3));
                        continue;
                    }

                    //if below void, fall
                    if (values[i - (xSize + 1)] < darkThresh)
                    {
                        //if (hasMoved[i - (xSize + 1)] == true) { continue; }
                        SwapColors(i, i - (xSize + 1));
                        continue;
                    }

                    //move down two
                    if (values[i - (xSize + 1)] > darkThresh && values[i - ((xSize + 1) * 2)] < darkThresh)
                    {
                        //if (hasMoved[i - (xSize + 1)] == true) { continue; }
                        SwapColors(i, i - ((xSize + 1) * 2));
                        continue;
                    }


                    //move right
                    if (values[i - (xSize + 1)] > darkThresh && values[i + 1] < darkThresh && values[i + (xSize + 1)] > darkThresh)
                    {
                        if (hasMoved[i + 1] == true) { continue; }
                        SwapColors(i, i + 1);
                        continue;
                    }

                    //move right if top & bottom full and two right is clear
                    if (values[i - (xSize + 2)] > darkThresh && values[i +2] < darkThresh && values[i + (xSize + 2)] > darkThresh)
                    {
                        if (hasMoved[i + 2] == true) { continue; }
                        SwapColors(i, i + 2);
                        continue;
                    }


                    if (values[i - (xSize + 1)] > darkThresh && values[i - 1] < darkThresh && values[i + (xSize + 1)] > darkThresh)
                    {
                       if (hasMoved[i - 1] == true) { continue; }
                       SwapColors(i, i - 1);
                       continue;
                    }

                    if (values[i - (xSize + 2)] > darkThresh && values[i - 2] < darkThresh && values[i + (xSize + 2)] > darkThresh)
                    {
                        if (hasMoved[i - 2] == true) { continue; }
                        SwapColors(i, i - 2);
                        continue;
                    }

                }



            }
        }

        mesh.colors = colors;
    }

    public void WeirdSandMountainThing()
    {
        SetNewRandomArray();

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

                if (x == 0 || x == xSize || y == 0 || y == ySize)
                {
                    continue;
                }
                // Color c = colors[i];
                float hue = hues[i];
                float sat = saturations[i];
                float val = values[i];

                if (hue > lowSandHue && hue < highSandHue && val > darkThresh)
                {


                    if (values[i - (xSize + 1)] > darkThresh && values[i + 1] < darkThresh && values[i + (xSize + 1)] > darkThresh)
                    {
                        if (hasMoved[i + 1] == true) { continue; }
                        SwapColors(i, i + 1);
                        continue;
                    }

                    if (values[i - (xSize + 1)] > darkThresh && values[i - 1] < darkThresh && values[i + (xSize + 1)] > darkThresh)
                    {
                        if (hasMoved[i - 1] == true) { continue; }
                        SwapColors(i, i - 1);
                        continue;
                    }

                    if (x > 1 && y > 1 && values[i - (xSize)] < darkThresh)
                    {
                        // if (hasMoved[i - (xSize)] == true) { continue; }
                        // SwapColors(i, i - (xSize));
                        // continue;
                    }

                    //move right diagonal
                    if (x > 1 && y > 1 && values[i - (xSize + 2)] < darkThresh)
                    {
                        // if (hasMoved[i - (xSize + 2)] == true) { continue; }
                        // SwapColors(i, i - (xSize + 2));
                        // continue;
                    }

                    //if below void, fall
                    if (values[i - (xSize + 1)] < darkThresh)
                    {
                        if (hasMoved[i - (xSize + 1)] == true) { continue; }
                        SwapColors(i, i - (xSize + 1));
                        continue;
                    }

                    //move left diagonal


                }



            }
        }

        mesh.colors = colors;
    }


    public void HueWater()
    {
        SetNewRandomArray();

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

                if (x == 0 || x == xSize || y == 0 || y == ySize)
                {
                    continue;
                }

                // Color c = colors[i];
                float hue = hues[i];
                float sat = saturations[i];
                float val = values[i];
                //Color.RGBToHSV(c, out hue, out sat, out val);

                if (hue > lowWaterHueValue && hue < highWaterHueValue)
                {
                    //for blue
                    //below color
                    float th, ts, tv;
                    th = hues[i - (xSize + 1)];
                    ts = saturations[i - (xSize + 1)];
                    tv = values[i - (xSize + 1)];
                    //Color.RGBToHSV(colors[i - (xSize + 1)], out th, out ts, out tv);

                    //if blue is below, darkher blue sink
                    if (th > lowWaterHueValue && th < highWaterHueValue && tv > darkThresh)
                    {
                        if (val < tv)
                        {
                           if (hasMoved[i - (xSize + 1)] == true) { continue; }
                           SwapColors(i, i - (xSize + 1));
                           continue;

                        }
                    }

                    if (tv > darkThresh && values[i + 1] < darkThresh)
                    {
                        SwapColors(i, i + 1);
                        continue;
                    }

                    if (tv > darkThresh && values[i-1] < darkThresh)
                    {
                        SwapColors(i, i - 1);
                        continue;
                    }

                    //if below is dark, occupy it
                    if (tv <= darkThresh)
                    {
                        if (hasMoved[i - (xSize + 1)] == true) { continue; }
                        SwapColors(i, i - (xSize + 1));
                        continue;
                    }

                    //check if two next are dark, and spread out
                    if (x < xSize -2 && values[i + 2] < darkThresh && values[i+1] < darkThresh)
                    {
                        SwapColors(i, i + 1);
                        continue;
                    }

                    if (x > 1 && values[i - 2] < darkThresh && values[i - 1] < darkThresh)
                    {
                        SwapColors(i, i - 1);
                        continue;
                    }

                    //make sand

                    if (x > 1 && y > 1 && values[i - (xSize)] < darkThresh)
                    {
                        if (hasMoved[i - (xSize)] == true) { continue; }
                        SwapColors(i, i - (xSize));
                        continue;
                    }

                    if (x > 1 && y > 1 && values[i - (xSize+2)] < darkThresh)
                    {
                        if (hasMoved[i - (xSize + 2)] == true) { continue; }
                        SwapColors(i, i - (xSize+2));
                        continue;
                    }




                    //bool leftOpen = isPixelOpenForWater(i - 1);
                    //bool rightOpen = isPixelOpenForWater(1 + 1);

                    float lh, ls, lv;
                    float rh, rs, rv;

                    lh = hues[i - 1];
                    ls = saturations[i - 1];
                    lv = values[i - 1];

                    rh = hues[i + 1];
                    rs = saturations[i + 1];
                    rv = values[i + 1];
                    //Color.RGBToHSV(colors[i - 1], out lh, out ls, out lv);
                    //Color.RGBToHSV(colors[i + 1], out rh, out rs, out rv);

                    //priority order - fill dark to the left and right first
                    leftOpen = false;
                    rightOpen = false;

                    if (lv < darkThresh){
                        leftOpen = true;
                    }
                    if (rv < darkThresh){
                       rightOpen = true;
                    }

                    if (leftOpen == true && rightOpen == false){
                        if (hasMoved[i - 1] == true) { continue; }
                        SwapColors(i, i - 1);
                        continue;
                    }
                    else if (leftOpen == true && rightOpen == true){
                        if (GetNewRandomNumber() > 5)
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
                    else if (leftOpen == false && rightOpen == true){
                        if (hasMoved[i + 1] == true) { continue; }
                        SwapColors(i, i + 1);
                        continue;
                    }

                    leftOpen = false;
                    rightOpen = false;

                    if (lh > lowWaterHueValue && lh < highWaterHueValue && val > lv)
                    {
                        leftOpen = true;
                    }
                    if (rh > lowWaterHueValue && rh < highWaterHueValue && val > rv)
                    {
                      rightOpen = true;
                    }


                    if (leftOpen == true && rightOpen == false)
                    {
                        if (hasMoved[i - 1] == true) { continue; }
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
                        if (GetNewRandomNumber() > 5)
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



    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    public static void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
    {
        byte[] _bytes = _texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(_fullPath, _bytes);
        Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + _fullPath);
    }

    public void Update()
    {
        //FireTest();
        AllTest();
        //SandTest();
        //HueWater();
        //HueSpecificTest();
        //HueTest();
        //Water();
        //realWater();

        if (Input.GetKeyDown(KeyCode.B))
        {
            //ScreenCapture.CaptureScreenshot("SomeLevel" + Time.frameCount + ".jpg", 10);
            //BlackoutImage();
            Texture2D tex = TextureFromColourMap(colors, xSize+1, ySize+1);
            SaveTextureAsPNG(tex, "scrn/hello" + Time.frameCount);
        }


        if (Input.GetKey(KeyCode.M))
        {
            //ScreenCapture.CaptureScreenshot("SomeLevel" + Time.frameCount + ".jpg", 10);
            //BlackoutImage();
            Texture2D tex = TextureFromColourMap(colors, xSize + 1, ySize + 1);
            SaveTextureAsPNG(tex, "scrn/hello" + Time.frameCount + ".png");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NoiseImage();
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

                    }
                    else if (rand < 4)
                    {
                        int aboveSpot = i + xSize + 1;
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
                        int rightSpot = i + 1;
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

    public void OldSwap(int orig, int targ)
    {

        Color _orig = colors[orig];
        Color _targ = colors[targ];
        colors[targ] = _orig;
        colors[orig] = _targ;
        hasMoved[orig] = true;
        hasMoved[targ] = true;
    }

    public void SetColor(int targ, Color c)
    {
        hasMoved[targ] = true;

        float hueStore, satStore, valStore;
        Color.RGBToHSV(c, out hueStore, out satStore, out valStore);

        hues[targ] = hueStore;
        saturations[targ] = satStore;
        values[targ] = valStore;

        colors[targ] = Color.HSVToRGB(hueStore, satStore, valStore);
    }

    public void PlantAddColor(int orig, int targ, Color c)
    {

        hasMoved[orig] = true;
        hasMoved[targ] = true;



        float hueStore, satStore, valStore;
        Color.RGBToHSV(c, out hueStore, out satStore, out valStore);


        valStore -= .0075f;
        
        

        hues[targ] = hueStore;
        saturations[targ] = satStore;
        values[targ] = valStore;

        colors[targ] = Color.HSVToRGB(hueStore, satStore, valStore);
    }

    public void SwapColors(int orig, int targ)
    {


        Color _orig = colors[orig];
        Color _targ = colors[targ];
        colors[targ] = _orig;
        colors[orig] = _targ;

        float hueStore, satStore, valStore;

        hueStore = hues[targ];
        satStore = saturations[targ];
        valStore = values[targ];

        hues[targ] = hues[orig];
        saturations[targ] = saturations[orig];
        values[targ] = values[orig];

        hues[orig] = hueStore;
        saturations[orig] = satStore;
        values[orig] = valStore;

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