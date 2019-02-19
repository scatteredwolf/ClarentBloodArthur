using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BlackGardenStudios.HitboxStudioPro
{
    public class SpriteIndexer : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            if (assetPath.EndsWith("palette.png")) return;

            var path = assetPath.Substring(0, assetPath.LastIndexOf('/') + 1) + "palette.png";

            if (File.Exists(path))
            {
                TextureImporter textureImporter = (TextureImporter)assetImporter;
                textureImporter.isReadable = true;
                textureImporter.filterMode = FilterMode.Point;
                textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
                textureImporter.maxTextureSize = 8192;
                textureImporter.wrapMode = TextureWrapMode.Clamp;
                textureImporter.mipmapEnabled = false;
                textureImporter.mipMapBias = -1;
            }
        }

        void OnPostprocessTexture(Texture2D texture)
        {
            if (assetPath.EndsWith("palette.png")) return;

            var path = assetPath.Substring(0, assetPath.LastIndexOf('/') + 1) + "palette.png";

            if (File.Exists(path))
            {
                var palette = new Texture2D(16, 3, TextureFormat.ARGB32, false);
                var fileContent = File.ReadAllBytes(path);
                palette.LoadImage(fileContent);
                var colors = palette.GetPixels32();
                var ordered = new Color32[palette.width * palette.height];
                int i = 0;

                for (int y = 0; y < palette.height; y++)
                    for (int x = 0; x < palette.width; x++)
                        ordered[i++] = colors[x + ((palette.height - 1 - y) * palette.width)];

                Convert(texture, ordered);
            }
        }

        static public void Convert(Texture2D texture, Color32[] palettePixels)
        {
            var outputPixels = texture.GetPixels32();
            var paletteMap = new Dictionary<int, byte>(256);

            for (int i = 0; i < palettePixels.Length; i++)
            {
                int color = (palettePixels[i].r << 16) + (palettePixels[i].g << 8) + palettePixels[i].b;
                if(!paletteMap.ContainsKey(color))
                    paletteMap.Add(color, (byte)i);
            }

            for (int i = 0; i < outputPixels.Length; i++)
            {
                Color32 color = outputPixels[i];
                int key = (color.r << 16) + (color.g << 8) + color.b;
                byte index = 0;

                paletteMap.TryGetValue(key, out index);
                outputPixels[i] = new Color32(index, 0, 0, color.a);
            }

            texture.SetPixels32(outputPixels);
            texture.Apply();
        }
    }
}