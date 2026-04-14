using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class UI_Face_Camera : MonoBehaviour
{
    public bool alwaysOnTop = true;

    void Awake()
    {
        if (alwaysOnTop) SetAlwaysOnTop();
    }

    void SetAlwaysOnTop()
    {
        foreach (var graphic in GetComponentsInChildren<Graphic>())
        {
            if (graphic is TMP_Text) continue;
            Material mat = new Material(graphic.material);
            mat.SetInt("unity_GUIZTestMode", (int)UnityEngine.Rendering.CompareFunction.Always);
            graphic.material = mat;
        }

        foreach (var tmp in GetComponentsInChildren<TMP_Text>())
        {
            Material mat = new Material(tmp.fontMaterial);
            mat.SetInt("unity_GUIZTestMode", (int)UnityEngine.Rendering.CompareFunction.Always);
            tmp.fontMaterial = mat;
        }
    }

    void LateUpdate()
    {
        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;
    }
}
