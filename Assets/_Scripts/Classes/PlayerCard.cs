using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Image cardImage;
    public GameObject discardButtonGO;

    private void Start()
    {
        cardImage.Fade(0.5f);
    }

    public void SetSprite(Sprite sprite)
    {
        cardImage.sprite = sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardImage.Fade(1f);
        discardButtonGO.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardImage.Fade(0.5f);
        discardButtonGO.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //cardImage.color = Color.blue;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //cardImage.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //discardButtonGO.SetActive(!discardButtonGO.activeSelf);
    }

    public void OnDiscardButtonClick()
    {

    }
}
