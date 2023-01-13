using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class CardMatrixProducer : MonoBehaviour
{
    [SerializeField] Card[] CardPrefabs;
    int maxlayer;         // It is only the up-boundary, and will never be reached.
    int row, column;
    bool[,,] occupiedIndicator;
    float cardWidth, cardHeight;
    List<int> cardIndex = new List<int>();
    List<Vector3> CoordidateList = new List<Vector3>();
    List<List<Vector3>> cardRelation = new List<List<Vector3>>();
    List<int> randCardArrangement = new List<int>();

    void Start()
    {
        Vector3 coordinate = new Vector3();
        cardWidth = CardPrefabs[0].GetComponent<BoxCollider2D>().size[0];
        cardHeight = CardPrefabs[0].GetComponent<BoxCollider2D>().size[1];
        float cardScalingFactor = CardPrefabs[0].gameObject.transform.localScale[0];
        
        maxlayer = 100;
        row = 5;
        column = 3;
        float shiftInZAxis = 0.01f;
        System.Random rnd = new System.Random(123);
        ProduceRandCardArrangement();
        int idx = 0;
        int cardTypeCount = 0;
        int cardTypeIndex = 0;
        int myLayer = 0;
        for (int k = 0; k < maxlayer; k++) 
        {
            for (int j = 0; j < row; j++) 
            {
                for (int i = 0; i < column; i++) 
                {
                    if (CreateFillingCondition(k, j, i) && rnd.Next(100) <= 70) 
                    {
                        coordinate = new Vector3((int)(i - column / 2), (int)(j - row / 2), (int)k);
                        cardTypeIndex = randCardArrangement[cardTypeCount];
                        var cardObject = Instantiate(CardPrefabs[cardTypeIndex], 
                            new Vector3(coordinate.x * (cardWidth / 2) * cardScalingFactor, coordinate.y * (cardHeight / 2) * cardScalingFactor, 
                                - shiftInZAxis * coordinate.z), Quaternion.identity, gameObject.transform);
                        CoordidateList.Add(coordinate);
                        cardObject.Coordidate = coordinate;
                        cardObject.CardIndex = idx;
                        idx++;
                        cardObject.IsTouchable = false;
                        cardTypeCount++;
                        if (cardTypeCount >= randCardArrangement.Count) { goto EndStart; }
                    }
                }
            }
            myLayer++;
        }

        EndStart: 
        {
            cardIndex = Enumerable.Range(0, CoordidateList.Count).ToList<int>();
            cardRelation = IdentifyCardRelation(maxlayer);
            SetCardTouchability();
            // print("How many layers finally? " + (myLayer + 1));
            // print("How many cards in total? " + randCardArrangement.Count);
        };
    }

    private void ProduceRandCardArrangement()
    {
        int numberOfType = CardPrefabs.Count();
        List<int> numberOfPairs = new List<int>();    // 3N cards for each type.
        List<int> cardArrangement = new List<int>();
        System.Random rnd = new System.Random(100);
        for (int i = 0; i < numberOfType; i++)
        {
            numberOfPairs.Add(rnd.Next(2, 4));
            cardArrangement.AddRange(Enumerable.Repeat(i, 3 * numberOfPairs[i]).ToList());
        }
        
        randCardArrangement = cardArrangement.OrderBy(x => rnd.Next()).ToList();    // Shuffle the cards.
        // print(string.Format("The random card list: ({0})", string.Join(", ", randCardArrangement)));
    }

    private bool CreateFillingCondition(int layer, int col, int row) 
    {
        if (layer % 4 == 0 && col % 2 == 0 && row % 2 == 0) { return true; }
        if (layer % 4 == 1 && col % 2 == 0 && row % 2 == 1) { return true; }
        if (layer % 4 == 2 && col % 2 == 1 && row % 2 == 1) { return true; }
        if (layer % 4 == 3 && col % 2 == 1 && row % 2 == 0) { return true; }
        return false;
    }

    private List<List<Vector3>> IdentifyCardRelation(int layer) 
    {
        int x = 0, y = 0, z = 0;
        int _x = 0, _y = 0, _z = 0;
        List<List<Vector3>> cardRelation = new List<List<Vector3>>();
        int cardCounter = 0; 
        foreach (Vector3 coordinate in CoordidateList)
        {
            cardRelation.Add(new List<Vector3>());
            if (coordinate.z == layer - 1) { continue; }
            else 
            {
                foreach (Vector3 _coordinate in CoordidateList) 
                {
                    x = (int)coordinate.x;
                    y = (int)coordinate.y;
                    z = (int)coordinate.z;
                    _x = (int)_coordinate.x;
                    _y = (int)_coordinate.y;
                    _z = (int)_coordinate.z;
                    
                    if ((z < _z) && (x <= _x + 1 && x >= _x - 1) && (y <= _y + 1 && y >= _y - 1)) {
                        cardRelation[cardCounter].Add(new Vector3(_x, _y, _z));
                    }
                }
                cardCounter++;
            }
        }
        return cardRelation;
    }

    public void RemoveItemsFromLists(int cardIdx, Vector3 cardCoordinate)
    {
        int idx = cardIndex.IndexOf(cardIdx);
        cardIndex.RemoveAt(idx);
        cardRelation.RemoveAt(idx);
        foreach (List<Vector3> coordinateList in cardRelation)
        {
            if (coordinateList.Contains(cardCoordinate))
            {
                coordinateList.Remove(cardCoordinate);
            }
        }
    }

    public void SetCardTouchability() 
    {
        // List of CardIndex should always have the same length as List of CardRelation.
        int idx = 0;

        Card[] cards = FindObjectsOfType<Card>();
        foreach (Card card in cards) 
        {
            if (cardIndex.Contains(card.CardIndex)) 
            {
                idx = cardIndex.IndexOf(card.CardIndex);
                if (cardRelation[idx].Count == 0) {
                    card.IsTouchable = true;
                }
                else {
                    card.IsTouchable = false;
                }
            }
        }
    }
}
