using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public bool isPlayer;
    public int playerNumber;
    public string playerName;
    public List<int> handIntList;
    private DiscardCard discardCard;
    private IntelligentCard ic;
    private GameplayUI Gui => GameplayUI.gUI;

    private void Awake()
    {
        ic = !isPlayer ? GetComponent<IntelligentCard>() : null;
    }

    private void Start()
    {
        discardCard = Gui.discardCards[playerNumber];
    }

    public void ActivatePlayerCards()
    {
        Gui.SetPlayerProfile(playerNumber, playerName);
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

    public bool HaveAceOfSpade => handIntList.Any(x => x == 39);

    public IEnumerator TurnRoutine(float time)
    {
        while (time > 0)
        {
            discardCard.profileBG.color = time % 2 == 0 ? Color.red : Color.green;
            discardCard.turnText.text = time.ToString();
            yield return Helper.GetWait(1f);
            time--;
        }
        discardCard.turnText.text = "";
        discardCard.profileBG.color = Color.white;
    }
}
