using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isPlayer;
    public int playerNumber;
    public string playerName;
    public List<int> handIntList;

    private void Start()
    {

    }

    public void ActivatePlayerCards()
    {
        if (isPlayer) { _ = StartCoroutine(ActivateCardsRoutine()); }
    }

    private IEnumerator ActivateCardsRoutine()
    {
        int j = 0;
        int count = handIntList.Count;
        float yOffset = 0;
        float zOffset = 0;
        for (int i = 0; i < count; i++)
        {
            CardManager.cm.cards[handIntList[i]].gameObject.transform.position =
                new(CardManager.cm.cardPositions[j].position.x,
                CardManager.cm.cardPositions[j].position.y + yOffset,
                CardManager.cm.cardPositions[j].position.z + zOffset);
            if (j < 4) { j++; }
            else
            {
                j = 0;
                yOffset -= 0.7f;
                zOffset -= 0.1f;
            }
            CardManager.cm.cards[handIntList[i]].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
