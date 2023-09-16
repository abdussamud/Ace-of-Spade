using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public bool isPlayer;
    public int playerNumber;
    public string playerName;
    public List<int> handIntList;
    private IntelligentCard ic;
    private GameplayUI Gui => GameplayUI.gUI;

    private void Awake()
    {
        ic = GetComponent<IntelligentCard>();
    }

    public void ActivatePlayerCards()
    {
        if (isPlayer) { _ = StartCoroutine(ActivateCardsRoutine()); }
    }

    private IEnumerator ActivateCardsRoutine()
    {
        handIntList.Sort();
        int count = handIntList.Count;
        for (int i = 0; i < count; i++)
        {
            Gui.SetPlayerCard(handIntList[i], i);
            yield return Helper.GetWait(0.1f);
        }
    }
}
