using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HorizontalLineAttribute))]
public class HorizontalLineDrawer : DecoratorDrawer
{
    public override float GetHeight()
    {
        var attr = (HorizontalLineAttribute)attribute;
        return EditorGUIUtility.singleLineHeight + attr.height + 6f;
    }

    public override void OnGUI(Rect position)
    {
        var attr = (HorizontalLineAttribute)attribute;
        Rect lineRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 0.5f,
                                 position.width, attr.height);
        EditorGUI.DrawRect(lineRect, new Color(attr.r, attr.g, attr.b));
    }
}
