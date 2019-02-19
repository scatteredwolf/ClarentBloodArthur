using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace BlackGardenStudios.HitboxStudioPro
{
    [CustomEditor(typeof(SpritePalette))]
    public class SpritePaletteInspector : Editor
    {
        private SpritePalette m_Palette;
        private SerializedObject m_Object;
        public int MouseOverIndex { get; private set; }

        public void OnEnable()
        {
            m_Palette = (SpritePalette)target;
            m_Object = new SerializedObject(target);

            if (m_Palette.Colors == null || m_Palette.Colors.Length == 0)
            {
                if (m_Palette.Texture != null)
                {
                }
            }
        }

        private bool ColorField(SerializedProperty prop, int index)
        {
            if (index >= prop.arraySize) return false;

            var element = prop.GetArrayElementAtIndex(index);
            var color = element.colorValue;

            //Shouldn't have any alpha values for the palette
            if (color.a < 1f)
            {
                color.a = 1f;
                element.colorValue = color;
            }
#if UNITY_2018_2_OR_NEWER
            element.colorValue = EditorGUILayout.ColorField(new GUIContent(""), element.colorValue, true, false, false, GUILayout.Width(52), GUILayout.Height(32));
#else
            element.colorValue = EditorGUILayout.ColorField(new GUIContent(""), element.colorValue, true, false, false, null, GUILayout.Width(52), GUILayout.Height(32));
#endif

            return GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition);
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty Colors = m_Object.FindProperty("Colors");
            SerializedProperty Name = m_Object.FindProperty("Name");

            EditorGUILayout.PropertyField(Name);

            EditorGUILayout.BeginVertical();

            if (Event.current.type == EventType.MouseMove)
                MouseOverIndex = -1;

            for (int i = 0; i < Colors.arraySize; i += 6)
            {
                EditorGUILayout.BeginHorizontal();
                if (ColorField(Colors, i)) MouseOverIndex = i;
                if (ColorField(Colors, i + 1)) MouseOverIndex = i + 1;
                if (ColorField(Colors, i + 2)) MouseOverIndex = i + 2;
                if (ColorField(Colors, i + 3)) MouseOverIndex = i + 3;
                if (ColorField(Colors, i + 4)) MouseOverIndex = i + 4;
                if (ColorField(Colors, i + 5)) MouseOverIndex = i + 5;
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Import Palette PNG"))
            {
                string path = EditorUtility.OpenFilePanel("Import Palette Png", "", "png");

                if (path.Length != 0)
                {
                    var texture = new Texture2D(16, 3, TextureFormat.ARGB32, false);
                    var fileContent = File.ReadAllBytes(path);
                    texture.LoadImage(fileContent);
                    texture.filterMode = FilterMode.Point;
                    var width = texture.width;
                    var height = texture.height;
                    int i = 0;

                    Colors.ClearArray();

                    for (int y = 0; y < height; y++)
                        for (int x = 0; x < width; x++)
                        {
                            Colors.arraySize++;
                            Colors.GetArrayElementAtIndex(i++).colorValue = texture.GetPixel(x, (height - 1) - y);
                        }
                }
            }

            m_Object.ApplyModifiedProperties();
            EditorUtility.SetDirty(m_Palette);

            for (int i = 0; i < m_Palette.Colors.Length; i++)
            {
                m_Palette.Texture.SetPixel(i, 0, m_Palette.Colors[i]);
            }

            m_Palette.Texture.Apply();
        }
    }
}