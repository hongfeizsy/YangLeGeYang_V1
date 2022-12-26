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
        float shiftInYAxis = 0.1f;
        for (int i = 0; i < LayerNumber; i++) {
            cardIndex = Random.Range(0, numberOfCardTypes);
            SpawnCard(cardIndex, i, shiftInYAxis * i);
        }
    }

    private void SpawnCard(int cardIndex, int layer, float shiftInYAxis) {
        // Instantiate(CardPrefabs[cardIndex], transform);
        Card cardObject = Instantiate(CardPrefabs[cardIndex], transform.position - new Vector3(0, shiftInYAxis), transform.rotation);
        cardObject.gameObject.transform.parent = this.transform;
        if (layer == 0) {cardObject.IsTouchable = true; }
        else { cardObject.IsTouchable = false; }
    }
}
