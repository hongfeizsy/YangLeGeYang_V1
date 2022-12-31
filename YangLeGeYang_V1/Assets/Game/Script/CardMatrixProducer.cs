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
    List<int> CardIndex = new List<int>();
    List<Vector3> CoordidateList = new List<Vector3>();

    void Start()
    {
        Vector3 coordinate = new Vector3();
        cardWidth = CardPrefabs[0].GetComponent<BoxCollider2D>().size[0];
        cardHeight = CardPrefabs[0].GetComponent<BoxCollider2D>().size[1];
        float cardScalingFactor = CardPrefabs[0].gameObject.transform.localScale[0];

        row = 2;
        column = 5;
        float shiftInZAxis = 0.01f;
        System.Random rnd = new System.Random(123);
        
        for (int k = 0; k < layer; k++) 
        {
            // if (CoordidateList.Count < (k + 1)) { CoordidateList.Add(new List<Vector3>()); }
            for (int j = 0; j < row; j++) 
            {
                for (int i = 0; i < column; i++) 
                {
                    if (k % 2 == j % 2 && k % 2 == i % 2) 
                    {
                        coordinate = new Vector3((int)(i - column / 2), (int)(j - row / 2), (int)k);
                        var cardObject = Instantiate(CardPrefabs[k],
                            new Vector3(coordinate.x * (cardWidth / 2) * cardScalingFactor, coordinate.y * (cardHeight / 2) * cardScalingFactor,
                            -shiftInZAxis * coordinate.z), Quaternion.identity, gameObject.transform);
                        CoordidateList.Add(coordinate);
                        cardObject.Coordidate = coordinate;
                    }
                    
                    // if (CreateFillingCondition(k, j, i) && rnd.Next(100) <= 70) 
                    // {
                        // var cardObject = Instantiate(CardPrefabs[k], 
                        //     new Vector3(((float)i - column / 2) * (cardWidth / 2) * cardScalingFactor, ((float)j - row / 2) * (cardHeight / 2) * cardScalingFactor, 
                        //         - shiftInZAxis * k), Quaternion.identity, gameObject.transform);
                    //     coordinate = new Vector3(i, j, k);
                    //     cardObject.GetComponent<Card>().Coordidate = coordinate;
                    // }
                }
            }
        }
        CardIndex = Enumerable.Range(0, CoordidateList.Count).ToList<int>();

        IdentifyCardLevels(row, column, layer);
    }

    private bool CreateFillingCondition(int layer, int col, int row) 
    {
        if (layer % 4 == 0 && col % 2 == 0 && row % 2 == 0) { return true; }
        if (layer % 4 == 1 && col % 2 == 0 && row % 2 == 1) { return true; }
        if (layer % 4 == 2 && col % 2 == 1 && row % 2 == 1) { return true; }
        if (layer % 4 == 3 && col % 2 == 1 && row % 2 == 0) { return true; }
        return false;
    }

    private void IdentifyCardLevels(int row, int col, int layer) 
    {
        int _row = 0, _col = 0, _layer = 0;
        List<List<int>> levels = new List<List<int>>(); 
        foreach (Vector3 coordinate in CoordidateList)
        {
            print(coordinate);
            if (coordinate.z == layer - 1) { levels.Add(new List<int>()); }
            else 
            {
                _row = (int)coordinate.x;
                _col = (int)coordinate.y;
                _layer = (int)coordinate.z;

                for (int k = _layer; k < layer; k++) 
                {
                    for (int j = _col - 1; j < _col + 2; j++)
                    {
                        for (int i = _row - 1; i < _row + 2; i++)
                        {
                            
                        }
                    }
                }




            }
        }

    }
}
