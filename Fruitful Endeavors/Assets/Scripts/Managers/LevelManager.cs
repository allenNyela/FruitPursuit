using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform[] path;
    public Transform startpoint;
    public float rotation = 0;

    public int currency;

    private void Awake()
    {
        main = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0) 
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                rotateTurretLeft();
            } else
            {
                rotateTurretRight();
            }
        }
    }

    public void rotateTurretRight()
    {
        if (rotation == 270)
        {
            rotation = 0;
            BuildManager.main.GetSelectedTower().towerPreview.transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        } else
        {
            rotation += 90;
            BuildManager.main.GetSelectedTower().towerPreview.transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }
    }

    public void rotateTurretLeft()
    {
        if (rotation == -270)
        {
            rotation = 0;
            BuildManager.main.GetSelectedTower().towerPreview.transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }
        else
        {
            rotation -= 90;
            BuildManager.main.GetSelectedTower().towerPreview.transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        Shop_Item.onCurrencyChange.Invoke();
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            Shop_Item.onCurrencyChange.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void showPreview(Plot plot)
    {
        BuildManager.main.GetSelectedTower().towerPreview.transform.position = new Vector3(plot.transform.position.x, plot.transform.position.y + .5f, plot.transform.position.z);
        BuildManager.main.GetSelectedTower().towerPreview.transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        BuildManager.main.GetSelectedTower().towerPreview.SetActive(true);
        BuildManager.main.GetSelectedTower().towerPreview.GetComponent<Turret>()?.ShowRange(true);
    }

    public void hidePreview(Plot plot)
    {
        BuildManager.main.GetSelectedTower().towerPreview.GetComponent<Turret>()?.ShowRange(false);
        BuildManager.main.GetSelectedTower().towerPreview.SetActive(false);
    }
}
