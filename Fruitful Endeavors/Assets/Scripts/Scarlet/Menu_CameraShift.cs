using UnityEngine;
using UnityEngine.UI;

public class Menu_CameraShift : MonoBehaviour
{
    public Camera mainCamera;
    public Camera menuCamera;
    public Button menuButton;
    [Range(0.1f, 5f)] public float shiftDuration = 0.5f;

    private Vector3 originPos, menuPos;
    private Quaternion originRot, menuRot;
    private float elapsed = float.MaxValue;
    private Vector3 fromPos;

    private void Start()
    {
        originPos = mainCamera.transform.position;
        originRot = mainCamera.transform.rotation;
        menuPos = menuCamera.transform.position;
        menuRot = menuCamera.transform.rotation;
        menuButton.onClick.AddListener(OnMenuButtonClicked);
    }

    private void OnMenuButtonClicked()
    {
        fromPos = mainCamera.transform.position;
        elapsed = 0f;
    }

    private void LateUpdate()
    {
        if (elapsed >= shiftDuration) return;
        elapsed += Time.deltaTime;
        float s = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / shiftDuration));
        if (fromPos == originPos) OpenMenuUpdate(s);
        else CloseMenuUpdate(s);
    }

    private void OpenMenuUpdate(float s)
    {
        mainCamera.transform.SetPositionAndRotation(
            Vector3.Lerp(originPos, menuPos, s),
            Quaternion.Slerp(originRot, menuRot, s));
    }

    private void CloseMenuUpdate(float s)
    {
        mainCamera.transform.SetPositionAndRotation(
            Vector3.Lerp(menuPos, originPos, s),
            Quaternion.Slerp(menuRot, originRot, s));
    }
}
