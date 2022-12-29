using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] Card[] CardPrefabs;
    [SerializeField] int LayerNumber;
    int numberOfCardTypes;

    void Start()
    {
        numberOfCardTypes = CardPrefabs.Length;
        int cardIndex = 0;
        float shiftInYAxis = 0.05f;
        float shiftInZAxis = 0.01f;
        for (int i = 0; i < LayerNumber; i++) {
            cardIndex = Random.Range(0, numberOfCardTypes);
            SpawnCard(cardIndex, i, shiftInYAxis * i, shiftInZAxis * i);
        }
    }

    private void SpawnCard(int cardIndex, int layer, float shiftInYAxis, float shiftInZAxis) {
        // Instantiate(CardPrefabs[cardIndex], transform);
        Card cardObject = Instantiate(CardPrefabs[cardIndex], transform.position - new Vector3(0, shiftInYAxis, shiftInZAxis), transform.rotation);
        cardObject.gameObject.transform.parent = this.transform;
        if (layer == LayerNumber - 1) {cardObject.IsTouchable = true; }
        else { cardObject.IsTouchable = false; }
    }

    // public void EnableCardInQueue() {
    //     if (transform.childCount >= 1) {
    //         transform.GetChild(transform.childCount - 1).GetComponent<Card>().IsTouchable = true;
    //     }
    // }
}
