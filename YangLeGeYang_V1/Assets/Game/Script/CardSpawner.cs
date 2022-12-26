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
            SpawnCard(cardIndex, shiftInYAxis * i);
        }
    }

    private void SpawnCard(int cardIndex, float shiftInYAxis) {
        // Instantiate(CardPrefabs[cardIndex], transform);
        Card cardObject = Instantiate(CardPrefabs[cardIndex], transform.position - new Vector3(0, shiftInYAxis), transform.rotation);
        cardObject.gameObject.transform.parent = this.transform;
    }
}
