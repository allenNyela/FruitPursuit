using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class Shop_Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int cost;
    public Button button;
    public Image display;
    public GameObject tooltip;

    [Header("Events")]
    public static UnityEvent onCurrencyChange = new UnityEvent();

    private void Awake()
    {
        onCurrencyChange.AddListener(CurrencyChanged);
        if (tooltip) tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip) tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip) tooltip.SetActive(false);
    }

    void CurrencyChanged()
    {
        bool canAfford = LevelManager.main.currency >= cost;
        button.interactable = canAfford;
        Color c = display.color;
        c.a = canAfford ? 1f : .5f;
        display.color = c;
    }
}
