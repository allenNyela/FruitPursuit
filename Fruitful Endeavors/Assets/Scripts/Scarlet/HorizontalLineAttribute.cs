using UnityEngine;

public class HorizontalLineAttribute : PropertyAttribute
{
    public float height;
    public float r, g, b;

    public HorizontalLineAttribute(float height = 1f, float r = 0.3f, float g = 0.3f, float b = 0.3f)
    {
        this.height = height;
        this.r = r;
        this.g = g;
        this.b = b;
    }
}
