using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform[] path;
    public Transform startpoint;
    public GameObject[] towers;

    public int currency;

    private void Awake()
    {
        main = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //currency = 20;
    }

    // Update is called once per frame
    void Update()
    {

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
        switch (BuildManager.main.GetSelectedTower().name)
        {
            case "Ghost":
                towers[0].transform.position = new Vector3(plot.transform.position.x, plot.transform.position.y + .5f, plot.transform.position.z);
                towers[0].SetActive(true);
                break;
            case "Another Time":
                towers[1].transform.position = new Vector3(plot.transform.position.x, plot.transform.position.y + .5f, plot.transform.position.z);
                towers[1].SetActive(true);
                break;
            case "Chat Bubble":
                towers[2].transform.position = new Vector3(plot.transform.position.x, plot.transform.position.y + .5f, plot.transform.position.z);
                towers[2].SetActive(true);
                break;
            case "Gift":
                towers[3].transform.position = new Vector3(plot.transform.position.x, plot.transform.position.y + .5f, plot.transform.position.z);
                towers[3].SetActive(true);
                break;
            case "Punch":
                towers[4].transform.position = new Vector3(plot.transform.position.x, plot.transform.position.y + .5f, plot.transform.position.z);
                towers[4].SetActive(true);
                break;
        }
    }

    public void hidePreview(Plot plot)
    {
        switch (BuildManager.main.GetSelectedTower().name)
        {
            case "Ghost":
                //towers[0].transform.position = new Vector3(plot.transform.position.x, plot.transform.position.y + .5f, plot.transform.position.z);
                towers[0].SetActive(false);
                break;
            case "Another Time":
                //towers[1].transform.position = new Vector3(plot.transform.position.x, plot.transform.position.y + .5f, plot.transform.position.z);
                towers[1].SetActive(false);
                break;
            case "Chat Bubble":
                //towers[2].transform.position = new Vector3(plot.transform.position.x, plot.transform.position.y + .5f, plot.transform.position.z);
                towers[2].SetActive(false);
                break;
            case "Gift":
                //towers[3].transform.position = new Vector3(plot.transform.position.x, plot.transform.position.y + .5f, plot.transform.position.z);
                towers[3].SetActive(false);
                break;
            case "Punch":
                //towers[3].transform.position = new Vector3(plot.transform.position.x, plot.transform.position.y + .5f, plot.transform.position.z);
                towers[4].SetActive(false);
                break;
        }
    }
}
