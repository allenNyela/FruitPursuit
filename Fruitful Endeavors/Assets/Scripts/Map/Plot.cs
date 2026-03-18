using UnityEngine;

public class Plot : MonoBehaviour
{
    //[Header("References")]
    //[SerializeField] private SpriteRenderer sr;
    //[SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;
    public bool turretFilled;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // startColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse hover!");
        if (!turretFilled)
        {
            LevelManager.main.showPreview(this);
        }
        
    }

    private void OnMouseExit()
    {
        LevelManager.main.hidePreview(this);
    }

    private void OnMouseDown()
    {
       if (tower != null) return;

        
        Tower tempTower = BuildManager.main.GetSelectedTower();

        if (tempTower.cost > LevelManager.main.currency)
        { 
            return;
        }
        LevelManager.main.hidePreview(this);
        LevelManager.main.SpendCurrency(tempTower.cost);
        Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);

        tower = Instantiate(tempTower.prefab, spawnLocation, Quaternion.identity);
        turretFilled = true;
    }
}
