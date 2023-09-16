using UnityEngine;

public class IntelligentCard : MonoBehaviour
{
    private CardController cc;

    private void Awake()
    {
        cc = GetComponent<CardController>();
    }
}
