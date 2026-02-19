using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;

    private bool isMenuOpen = false;


    private void OnGUI()
    {
        currencyUI.text = LevelManager.main.currency.ToString();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleMenu()
    {
        if (isMenuOpen)
        {
            Vector2 currPos = GetComponent<RectTransform>().anchoredPosition;
            currPos.x = -1189;
            GetComponent<RectTransform>().anchoredPosition = currPos;
            isMenuOpen = false;
        }
        else
        {
            Vector2 currPos = GetComponent<RectTransform>().anchoredPosition;
            currPos.x = -736;
            GetComponent<RectTransform>().anchoredPosition = currPos;
            isMenuOpen = true;
        }

    }
}
