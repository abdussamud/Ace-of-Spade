using UnityEngine;


public class TouchManager : MonoBehaviour
{
    [SerializeField] private GameObject sellectedCard;
    [SerializeField] private GameManager gameManager;
    public Card oldSelectedCard;
    public LayerMask cardLayer;
    public LayerMask cellLayer;
    private RaycastHit2D rayHit;
    private RaycastHit2D hitCell;
    private bool isCardPlaced;
    [SerializeField] private GameObject placeCardButton;
    private Color blueColor;


    private void Start()
    {
        blueColor = new Color32(0x00, 0x72, 0xFF, 0xFF);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && sellectedCard == null)
        {
            rayHit = Cast2DRay;
            if (rayHit.collider && rayHit.collider.CompareTag("Card"))
            {
                sellectedCard = rayHit.collider.gameObject;
                oldSelectedCard = sellectedCard.GetComponent<Card>();
                sellectedCard.GetComponent<SpriteRenderer>().color = blueColor;
                if (!isCardPlaced)
                {
                    placeCardButton.SetActive(true);
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && sellectedCard != null)
        {
            rayHit = Cast2DRay;
            hitCell = Cast2DRayForCell;
            if (rayHit.collider && rayHit.collider.CompareTag("Card"))
            {
                if (sellectedCard == rayHit.collider.gameObject)
                {
                    sellectedCard.GetComponent<SpriteRenderer>().color = Color.white;
                    oldSelectedCard = null;
                    sellectedCard = null;
                    placeCardButton.SetActive(false);
                }
                else if (sellectedCard != rayHit.collider.gameObject)
                {
                    sellectedCard.GetComponent<SpriteRenderer>().color = Color.white;
                    sellectedCard = rayHit.collider.gameObject;
                    oldSelectedCard = sellectedCard.GetComponent<Card>();
                    sellectedCard.GetComponent<SpriteRenderer>().color = blueColor;
                }
            }
        }
    }

    public void PlaceCard()
    {
        foreach (Cell cell in gameManager.cellPositions)
        {
            if (!cell.isOccupide)
            {
                sellectedCard.transform.position = cell.cellTransform.position;
                sellectedCard.GetComponent<SpriteRenderer>().color = Color.white;
                sellectedCard.tag = "Untagged";
                sellectedCard.layer = default;
                oldSelectedCard = null;
                sellectedCard = null;
                placeCardButton.SetActive(false);
                isCardPlaced = true;
                cell.isOccupide = true;
                return;
            }
        }
        Debug.Log("No Space");
    }

    private RaycastHit2D Cast2DRay => Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 90f, cardLayer);

    private RaycastHit2D Cast2DRayForCell => Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 90f, cellLayer);

}
