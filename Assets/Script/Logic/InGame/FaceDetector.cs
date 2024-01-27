using OpenCvSharp.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDetector : WebCamera
{
    public TextAsset faces;
    public TextAsset eyes;
    public TextAsset shapes;

    private FaceProcessorLive<WebCamTexture> processor;

    /// <summary>
    /// Default initializer for MonoBehavior sub-classes
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        base.forceFrontalCamera = true; // we work with frontal cams here, let's force it for macOS s MacBook doesn't state frontal cam correctly

        byte[] shapeDat = shapes.bytes;
        processor = new FaceProcessorLive<WebCamTexture>();
        processor.Initialize(faces.text, eyes.text, shapes.bytes);

        // data stabilizer - affects face rects, face landmarks etc.
        processor.DataStabilizer.Enabled = true;        // enable stabilizer
        processor.DataStabilizer.Threshold = 2.0;       // threshold value in pixels
        processor.DataStabilizer.SamplesCount = 2;      // how many samples do we need to compute stable data

        // performance data - some tricks to make it work faster
        processor.Performance.Downscale = 256;          // processed image is pre-scaled down to N px by long side
        processor.Performance.SkipRate = 0;             // we actually process only each Nth frame (and every frame for skipRate = 0)
    }

    /// <summary>
    /// Per-frame video capture processor
    /// </summary>
    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        // detect everything we're interested in
        processor.ProcessTexture(input, TextureParameters);

        // mark detected objects
        processor.MarkDetected();

        processor.Faces.ForEach(face =>
        {
            GenerateTexture(face);
        });

        return true;
    }

    void GenerateTexture(DetectedFace face)
    {
        int minMarkX = 1000;
        int maxMarkX = 0;
        int minMarkY = 1000;
        int maxMarkY = 0;
        if (face.Marks.Length == 0) return;
        for (int i = 0; i < face.Marks.Length; i++)
        {
            if (face.Marks[i].X < minMarkX) minMarkX = face.Marks[i].X;
            if (face.Marks[i].X > maxMarkX) maxMarkX = face.Marks[i].X;
            if (face.Marks[i].Y < minMarkY) minMarkY = face.Marks[i].Y;
            if (face.Marks[i].Y > maxMarkY) maxMarkY = face.Marks[i].Y;
        }
        int width = maxMarkX - minMarkX;
        int height = maxMarkY - minMarkY;
        Texture2D texture = new Texture2D(128, 128);
        for (int x = 0; x < 128; x++)
        {
            for (int y = 0; y < 128; y++)
            {
                texture.SetPixel(x, y, new UnityEngine.Color(1.0f, 1.0f, 1.0f, 0.0f)); // Optional: Set a default color for the entire texture
            }
        }

        // Set pixels at specific points
        for (int i = 0; i < face.Marks.Length; i++)
        {
            int x = (int)(((float)(face.Marks[i].X - minMarkX) / width) * 100) + 14;
            int y = 114 - (int)(((float)(face.Marks[i].Y - minMarkY) / height) * 100);
            SetPoints(texture, x, y);
        }

        texture.Apply();
        if (FaceMgr.In.OnFaceDetected != null)
            FaceMgr.In.OnFaceDetected.Invoke(texture);
    }

    void SetPoints(Texture2D texture, int x, int y)
    {
        int pointSize = 4;
        for (int i = -pointSize / 2; i <= pointSize / 2; i++)
        {
            for (int j = -pointSize / 2; j <= pointSize / 2; j++)
            {
                int pixelX = x + i;
                int pixelY = y + j;

                if (pixelX >= 0 && pixelX < texture.width && pixelY >= 0 && pixelY < texture.height)
                {
                    texture.SetPixel(pixelX, pixelY, UnityEngine.Color.green);
                }
            }
        }
    }
}
