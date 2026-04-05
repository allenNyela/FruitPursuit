using UnityEngine;
using UnityEngine.InputSystem;

public class TurretSelect : MonoBehaviour
{
    public GameObject Tooltip;
    public bool tooltipPres = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (tooltipPres)
        //{
        //    Tooltip.transform.position = Input.mousePosition;
        //}
        
    }

    private void OnMouseEnter()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Turret"))
        {
            Tooltip.SetActive(true);
            tooltipPres = true;
        }
        
    }

    private void OnMouseExit()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Turret"))
        {
            Tooltip.SetActive(false);
            tooltipPres = false;
        }
    }
}
