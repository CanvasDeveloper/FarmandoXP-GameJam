using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFeedback : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private Color OnMouseEnterColor = Color.gray;
    [SerializeField] private Color OnMouseExitColor = Color.white;
   
    public void OnSelect(BaseEventData eventData)
    {
        textMeshProUGUI.color = OnMouseEnterColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        textMeshProUGUI.color = OnMouseExitColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textMeshProUGUI.color = OnMouseEnterColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textMeshProUGUI.color = OnMouseExitColor;
    }
}