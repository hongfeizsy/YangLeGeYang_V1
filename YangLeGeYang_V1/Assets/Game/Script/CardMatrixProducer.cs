using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CardMatrixProducer : MonoBehaviour
{
    [SerializeField] Card[] CardPrefabs;
    [SerializeField] int layer;
    int row, column;
    bool[,,] occupiedIndicator;
    float cardWidth, cardHeight;

    void Start()
    {
        Vector3 coordinate = new Vector3();
        cardWidth = CardPrefabs[0].GetComponent<BoxCollider2D>().size[0];
        cardHeight = CardPrefabs[0].GetComponent<BoxCollider2D>().size[1];
        float cardScalingFactor = CardPrefabs[0].gameObject.transform.localScale[0];

        row = 7;
        column = 5;
        float shiftInZAxis = 0.01f;
        for (int k = 0; k < layer; k++) 
        {
            for (int j = 0; j < row; j++) 
            {
                for (int i = 0; i < column; i++) 
                {
                    if (j % 2 == k % 2 && i % 2 == k % 2) 
                    {
                        var cardObject = Instantiate(CardPrefabs[k], 
                            new Vector3(((float)i - column / 2) * (cardWidth / 2) * cardScalingFactor, ((float)j - row / 2) * (cardHeight / 2) * cardScalingFactor, 
                                -shiftInZAxis * k), Quaternion.identity, gameObject.transform);
                        coordinate = new Vector3(i, j, k);
                        cardObject.GetComponent<Card>().Coordidate = coordinate;
                    }
                }
            }
        }
    }

    void Update()
    {
        
    }
}
