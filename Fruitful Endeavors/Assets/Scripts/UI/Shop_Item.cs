using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Shop_Item : MonoBehaviour
{

    public int cost;
    public Button button;
    public Image display;

    [Header("Events")]
    public static UnityEvent onCurrencyChange = new UnityEvent();

    private void Awake()
    {
        onCurrencyChange.AddListener(CurrencyChanged);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //display = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CurrencyChanged()
    {
        if (LevelManager.main.currency < cost)
        {
            button.interactable = false;
            Color tempColor = display.color;
            tempColor.a = .5f;
            display.color = tempColor;
        }
        else
        {
            button.interactable = true;
            Color tempColor = display.color;
            tempColor.a = 1f;
            display.color = tempColor;
        }
    }
}
