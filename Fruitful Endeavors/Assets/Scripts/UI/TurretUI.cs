using UnityEngine;

public class TurretUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveTurret()
    {
        this.GetComponentInParent<Turret>().plot.turretFilled = false;
        Destroy(transform.root.gameObject);
    }

    public void UpgradeTurret()
    {

    }
}
