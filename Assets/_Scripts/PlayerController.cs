using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerNumber;
    public List<Card> hand;
    public List<int> handIntList;

    private void Start()
    {
        GameController.gc.players.Add(this);
        playerNumber = GameController.gc.players.Count - 1;
    }

    public void ActivatePlayerCards()
    {
        _ = StartCoroutine(ActivateCardsRoutine());
    }

    private IEnumerator ActivateCardsRoutine()
    {
        Vector3 posY = Vector3.zero;
        Vector3 posZ = Vector3.zero;
        int activeCards = 0;
        while (activeCards < handIntList.Count)
        {
            hand[handIntList[activeCards]].gameObject.SetActive(true);
            posY += Vector3.down;
            posZ += -Vector3.forward;
            hand[handIntList[activeCards]].gameObject.transform.position += posZ;
            hand[handIntList[activeCards]].gameObject.transform.position += 3 * playerNumber * Vector3.left;
            hand[handIntList[activeCards]].gameObject.transform.position += posY;
            activeCards++;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
