using UnityEngine;

public class Plot : MonoBehaviour
{
    //[Header("References")]
    //[SerializeField] private SpriteRenderer sr;
    //[SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;

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
        //sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        //sr.color = startColor;
    }

    private void OnMouseDown()
    {
       // if (tower != null) return;


        Turret tempTower = BuildManager.main.GetSelectedTower();

        // if (tempTower.cost > LevelManager.main.currency)
        ////{
        //return;
        // }

        //LevelManager.main.SpendCurrency(tempTower.cost);
        Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);

        tower = (GameObject)Instantiate(tempTower.turretPrefab, spawnLocation, Quaternion.identity);
    }
}
