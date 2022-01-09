using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MatRefinedGrid : MonoBehaviour
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

    public bool runWater;
    public float lowWaterHueValue = .3f;
    public float highWaterHueValue = .8f;

    public bool runSand;
    public float lowSandHue;
    public float highSandHue;

    public bool runFire;
    public float lowFireHue;
    public float highFireHue;

    public bool runPlant;
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

    public void HueBasedSorter()
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

                if (x < 1 || y < 1 || x > xSize - 1 || y > ySize - 1)
                {
                    continue;
                }

                if (val < darkThresh)
                {
                    continue;
                }

                if (hasMoved[i] == true)
                {
                     continue;
                }

                if (GetNewRandomNumber() > 8)
                {
                    continue;
                }

                if (hues[i] > hues[i - 1] + .75f)
                {
                    if (hasMoved[i - 1] == false)
                    {
                        //SwapColors(i, i - 1);
                        //continue;
                    }
                }

                if (hues[i] > hues[i - (xSize + 1)] + .33f)
                {
                    if (hasMoved[i - (xSize + 1)] == false)
                    {
                        SwapColors(i, i - (xSize + 1));
                        continue;
                    }
                }

                if (saturations[i] < saturations[i - (xSize + 1)])
                {
                    if (hasMoved[i - (xSize)] == false)
                    {
                        //SwapColors(i, i - (xSize + 1));
                        //continue;
                    }
                }

                if (values[i] > values[i - (xSize +1)])
                {
                    if (hasMoved[i - (xSize + 1)] == false)
                    {
                        SwapColors(i, i - (xSize +1));
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

                if (x < 4 || y < 4 || x > xSize - 4 || y > ySize - 4)
                {
                    continue;
                }

                if (val < darkThresh)
                {
                    continue;
                }

                if (hasMoved[i] == true)
                {
                   // continue;
                }

                if (hue > lowWaterHueValue && hue < highWaterHueValue)
                {
                    if (runWater == true)
                    {
                        WaterLogic(i, hue, sat, val);
                    }
                }
                else if (hue > lowSandHue && hue < highSandHue)
                {
                    if (runSand == true)
                    {
                        //OldSandLogic(i, hue, sat, val);
                        SandLogic(i, hue, sat, val);
                    }

                }
                else if (hue > lowPlantHue && hue < highPlantHue)
                {
                    if (runPlant == true)
                    {
                        PlantLogic(i, hue, sat, val);
                    }

                }else if (hue > lowFireHue && hue < highFireHue)
                {
                    if (runFire == true)
                    {
                        //firel
                    }
                }

            }
        }
        mesh.colors = colors;

    }

    public void SandLogic(int i, float hue, float sat, float val)
    {
        if (hasMoved[i] == true)
        {
           // return;
        }

        if (GetNewRandomNumber() > 8)
        {
            return;
        }

        // fall through water
        if (hues[i - (xSize + 1)] > lowWaterHueValue && hues[i - (xSize + 1)] < highWaterHueValue)
        {
            // if (hasMoved[i - (xSize + 1)] == true) { return; }
            SwapColors(i, i - (xSize + 1));
            return;
        }

        // fall through water
        if (hues[i - (xSize + 1)] > lowSandHue && hues[i - (xSize + 1)] < highSandHue)
        {
            if (hues[i - (xSize + 1)] < hues[i])
            {
                // if (hasMoved[i - (xSize + 1)] == true) { return; }
                SwapColors(i, i - (xSize + 1));
                return;
            }
        }

        if (values[i - ((xSize + 1) * 3)] < darkThresh)
        {
            // if (hasMoved[i - (xSize + 1)] == true) { return; }
            SwapColors(i, i - ((xSize + 1) * 3));
            return;
        }

        //if below void, fall
        if (values[i - (xSize + 1)] < darkThresh)
        {
           // if (hasMoved[i - (xSize + 1)] == true) { return; }
            SwapColors(i, i - (xSize + 1));
            return;
        }



        if (values[i - ((xSize + 1)*2)] < darkThresh)
        {
            // if (hasMoved[i - (xSize + 1)] == true) { return; }
            SwapColors(i, i - ((xSize + 1)*2));
            return;
        }



        if (values[i-(xSize +2)] < darkThresh)
        {
            //if (hasMoved[i - (xSize-2)] == true) { return; }
            SwapColors(i, i - (xSize +2));
            return;
        }

        if (values[i - (xSize + 3)] < darkThresh)
        {
            if (hasMoved[i - (xSize+3)] == true) { return; }
            SwapColors(i, i - (xSize + 3));
            return;
        }

        if (values[i - (xSize + 4)] < darkThresh)
        {
            if (hasMoved[i - (xSize-4)] == true) { return; }
            SwapColors(i, i - (xSize + 4));
            return;
        }

        if (values[i - (xSize)] < darkThresh)
        {
            if (hasMoved[i - (xSize)] == true) { return; }
            SwapColors(i, i - (xSize));
            //return;
        }


        if (values[i - (xSize-1)] < darkThresh)
        {
            if (hasMoved[i - (xSize-1)] == true) { return; }
            SwapColors(i, i - (xSize-1));
            //return;
        }
    }

    public void OldSandLogic(int i, float hue, float sat, float val)
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
        if (GetNewRandomNumber() > 8)
        {
            return;
        }

        //if below is dark, occupy it
        if (values[i - (xSize + 1)] <= darkThresh)
        {
            if (hasMoved[i - (xSize + 1)] == true) { return; }
            SwapColors(i, i - (xSize + 1));
            return;
        }

        if (hues[i - (xSize + 1)] > lowWaterHueValue && hues[i - (xSize + 1)] < highWaterHueValue)
        {
            if (hues[i - (xSize + 1)] < hues[i])
            {
                SwapColors(i, i - (xSize + 1));
                return;
            }

        }

        if (hues[i - xSize] > lowWaterHueValue && hues[i - xSize] < highWaterHueValue)
        {
            if (hues[i - xSize] < hues[i])
            {
                SwapColors(i, i - xSize);
                return;
            }
        }

        if (hues[(i - xSize) - 2] > lowWaterHueValue && hues[(i - xSize) - 2] < highWaterHueValue)
        {
            if (hues[i - xSize - 2] < hues[i])
            {
                SwapColors(i, (i - xSize) - 2);
                return;
            }
        }

        if (values[i - (xSize)] <= darkThresh)
        {
            if (hasMoved[i - (xSize)] == true) { return; }
            SwapColors(i, i - (xSize));
            return;
        }

        if (Time.frameCount %2 == 1)
        {
            if (values[i - 1] <= darkThresh)
            {
                if (hasMoved[i - 1] == true) { return; }
                SwapColors(i, i - 1);
                return;
            }
        }
        else
        {
            if (values[i + 1] <= darkThresh)
            {
                if (hasMoved[i + 1] == true) { return; }
                SwapColors(i, i + 1);
                return;
            }
        }

        if (values[i - 3] <= darkThresh)
        {
            if (hasMoved[i - 1] == true) { return; }
            SwapColors(i, i - 3);
            return;
        }

        if (values[i + 3] <= darkThresh)
        {
            if (hasMoved[i + 1] == true) { return; }
            SwapColors(i, i + 3);
            return;
        }


    }

    public void OldWaterLogic(int i, float hue, float sat, float val)
    {
        if (GetNewRandomNumber() > 8)
        {
            return;
        }

        //for blue
        //below color
        float belowHue, belowSat, belowValue;
        belowHue = hues[i - (xSize + 1)];
        belowSat = saturations[i - (xSize + 1)];
        belowValue = values[i - (xSize + 1)];
        //Color.RGBToHSV(colors[i - (xSize + 1)], out th, out ts, out tv);

        //if blue is below, darkher blue sink
        if (belowHue > lowWaterHueValue && belowHue < highWaterHueValue && belowValue > darkThresh)
        {
            if (val < belowValue)
            {
                //if (hasMoved[i - (xSize + 1)] == true) { return; }
                //SwapColors(i, i - (xSize + 1));
                //return;

            }
        }

        if (belowValue > darkThresh && values[i + 1] < darkThresh)
        {
            //SwapColors(i, i + 1);
           // return;
        }

        if (belowValue > darkThresh && values[i - 1] < darkThresh)
        {
           // SwapColors(i, i - 1);
           // return;
        }

        //if below is dark, occupy it
        if (belowValue <= darkThresh)
        {
           // if (hasMoved[i - (xSize + 1)] == true) { return; }
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
           // if (hasMoved[i - (xSize)] == true) { return; }
            SwapColors(i, i - (xSize));
            return;
        }

        if (values[i - (xSize + 2)] < darkThresh)
        {
           // if (hasMoved[i - (xSize + 2)] == true) { return; }
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
           // if (hasMoved[i - 1] == true) { return; }
            SwapColors(i, i - 1);
            return;
        }
        else if (leftOpen == true && rightOpen == true)
        {
            if (GetNewRandomNumber() > 5)
            {
                //if (hasMoved[i - 1] == true) { return; }
                SwapColors(i, i - 1);
                return;
            }
            else
            {
                //if (hasMoved[i + 1] == true) { return; }
                SwapColors(i, i + 1);
                return;
            }
        }
        else if (leftOpen == false && rightOpen == true)
        {
            //if (hasMoved[i + 1] == true) { return; }
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
           // if (hasMoved[i - 1] == true) { return; }
            SwapColors(i, i - 1);
            return;
        }
        else if (leftOpen == false && rightOpen == true)
        {
           // if (hasMoved[i + 1] == true) { return; }
            SwapColors(i, i + 1);
            return;
        }
        else if (leftOpen == true && rightOpen == true)
        {
            if (GetNewRandomNumber() > 5)
            {
              //  if (hasMoved[i - 1] == true) { return; }
                SwapColors(i, i - 1);
                return;
            }
            else
            {
               // if (hasMoved[i + 1] == true) { return; }
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

        if (values[i + 1] > darkThresh)
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
        if (values[i + (xSize + 1)] > darkThresh)
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
        if (values[i - (xSize + 1)] > darkThresh)
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

                if (x <= 2 || x == xSize - 1 || y <= 5 || y == ySize - 1)
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
                    if (values[i - (xSize + 2)] > darkThresh && values[i + 2] < darkThresh && values[i + (xSize + 2)] > darkThresh)
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
        //HueBasedSorter();
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
            Texture2D tex = TextureFromColourMap(colors, xSize + 1, ySize + 1);
            SaveTextureAsPNG(tex, "scrn/hello" + Time.frameCount  + ".png");
        }


        if (Input.GetKey(KeyCode.M))
        {
            Texture2D tex = TextureFromColourMap(colors, xSize + 1, ySize + 1);
            SaveTextureAsPNG(tex, "scrn/hello" + Time.frameCount + ".png");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NoiseImage();
        }
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



}