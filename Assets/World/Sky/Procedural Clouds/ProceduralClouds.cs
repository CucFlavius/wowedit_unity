using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.World.Sky.Procedural_Clouds
{
    public class ProceduralClouds : MonoBehaviour
    {
        public int pixWidth;
        public int pixHeight;
        public float xOrg;
        public float yOrg;
        public float scale = 1.0F;
        private Texture2D noiseTex;
        private Color[] pix;
        private Renderer rend;
        private float zCoord = 0f;

        private ProceduralNoiseProject.PerlinNoise noise;

        private float nextActionTime = 0.0f;
        public float updatePeriod = 2.0f;

        //REFERENCES
        Texture2D mask;

        void Start()
        {
            noise = new ProceduralNoiseProject.PerlinNoise(0, 1, 1);
            rend = GetComponent<Renderer>();
            noiseTex = new Texture2D(pixWidth, pixHeight);
            pix = new Color[noiseTex.width * noiseTex.height];
            rend.material.mainTexture = noiseTex;
        }
        IEnumerator CalcNoise()
        {
            float y = 0.0F;
            while (y < noiseTex.height)
            {
                float x = 0.0F;
                while (x < noiseTex.width)
                {
                    float xCoord = xOrg + x / noiseTex.width * scale + Time.time / 50;
                    float yCoord = yOrg + y / noiseTex.height * scale;
                    float sample = noise.Sample3D(xCoord, yCoord, zCoord);

                    // distance fade - mask //
                    Vector2 maskResolution = new Vector2(noiseTex.height, noiseTex.width);
                    Vector2 maskCenter = new Vector2(maskResolution.x * 0.5f, maskResolution.y * 0.5f);
                    float distFromCenter = Vector2.Distance(maskCenter, new Vector2(x, y));
                    float maskPixel = ((distFromCenter / maskResolution.x) - 0.5f) * 3;

                    pix[(int)(y * noiseTex.width + x)] = new Color(255, 255, 255, sample * maskPixel);
                    x++;
                }
                y++;
                yield return null;
            }
            noiseTex.SetPixels(pix);
            noiseTex.Apply();
        }
        void Update()
        {
            zCoord = Time.time / 20;
            if (Time.time > nextActionTime)
            {
                nextActionTime += updatePeriod;
                StartCoroutine(CalcNoise());
            }
        }

    }
}
