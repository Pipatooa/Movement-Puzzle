using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorScheme", menuName = "Color Scheme")]
public class ColorScheme : ScriptableObject
{
    [System.Serializable]
    public struct NamedColor
    {
        public string name;
        public Material material;
    }

    public List<NamedColor> colors;

    public NamedColor goalColor;

    public Shader shader;
}
