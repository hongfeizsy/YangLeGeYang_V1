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
        cardWidth = CardPrefabs[0].GetComponent<BoxCollider2D>().size[0];
        cardHeight = CardPrefabs[0].GetComponent<BoxCollider2D>().size[1];
        
        row = 7;
        column = 5;
        float cardScalingFactor = CardPrefabs[0].gameObject.transform.localScale[0];
        for (int k = 0; k < layer; k++) 
        {
            for (int j = 0; j < row; j++) 
            {
                for (int i = 0; i < column; i++) 
                {
                    if (j % 2 == k % 2 && i % 2 == k % 2) 
                    {
                        var cardObject = Instantiate(CardPrefabs[k], 
                            new Vector3(((float)i - column / 2) * (cardWidth / 2) * cardScalingFactor, ((float)j - row / 2) * (cardHeight / 2) * cardScalingFactor), 
                            Quaternion.identity, gameObject.transform);
                    }
                }
            }
        }
    }

    void Update()
    {
        
    }
}
