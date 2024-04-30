using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject token1, token2, token3;
    private int[,] GameMatrix; //0 not chosen, 1 player, 2 enemy
    private int[] startPos = new int[2];
    private int[] objectivePos = new int[2];

    // 
    private List<Token> openList = new List<Token>();
    private List<Token> closedList = new List<Token>();
    private Token currentToken;
    private void Awake()
    {
        GameMatrix = new int[Calculator.length, Calculator.length];

        for (int i = 0; i < Calculator.length; i++) //fila
            for (int j = 0; j < Calculator.length; j++) //columna
                GameMatrix[i, j] = 0;

        //randomitzar pos final i inicial;
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        startPos[0] = rand1;
        startPos[1] = rand2;
        SetObjectivePoint(startPos);

        GameMatrix[startPos[0], startPos[1]] = 1;
        GameMatrix[objectivePos[0], objectivePos[1]] = 2;

        InstantiateToken(token1, startPos);
        InstantiateToken(token2, objectivePos);
        ShowMatrix();

        // 
        currentToken = new Token(startPos, 0);
        openList.Add(currentToken);
    }
    private void InstantiateToken(GameObject token, int[] position)
    {
        Instantiate(token, Calculator.GetPositionFromMatrix(position),
            Quaternion.identity);
    }
    private void SetObjectivePoint(int[] startPos) 
    {
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        if (rand1 != startPos[0] || rand2 != startPos[1])
        {
            objectivePos[0] = rand1;
            objectivePos[1] = rand2;
        }
    }

    private void ShowMatrix() //fa un debug log de la matriu
    {
        string matrix = "";
        for (int i = 0; i < Calculator.length; i++)
        {
            for (int j = 0; j < Calculator.length; j++)
            {
                matrix += GameMatrix[i, j] + " ";
            }
            matrix += "\n";
        }
        Debug.Log(matrix);
    }
    //EL VOSTRE EXERCICI COMENÇA AQUI
    private void Update()
    {

        if(!EvaluateWin())
        {
            // int[] currentToken = GetLowestInOpen();

            // if (EvaluateWin()) break;

            // Token[] possibleTokens = GetAllPossibleTokens();

            // foreach (Token token in possibleTokens)
            // {

            //     int currentCost = currentToken.cost + CheckDistanceToObj(currentToken.position, objectivePos);

            //     if (isInOpenList(token))
            //     {
            //         if (CheckDistanceToObj(token.position, objetivePos) <= currentCost) closedList.Add(currentToken);
            //         else if (isInClosedList(token)) 
            //         {
            //             if (CheckDistanceToObj(token.position, objetivePos) <= currentCost) closedList.Add(currentToken);
            //             closedList.Remove(token);
            //             openList.Add(token);
            //         }
            //         else
            //         {
            //             openList.Add(token);
            //             // 
            //         }
            //         // 
            //         // 
            //     }
            //     // 
            // }
            // // 
        }
    }
    private bool EvaluateWin()
    {
        return currentToken.position == objectivePos;
    }
}

class Token {
    public int[] position;
    public float Distance(Token from) { return Calculator.CheckDistanceToObj(from.position, position)}

    public float Heuristic(int[] objectivePos) { return Calculator.CheckDistanceToObj(position, objectivePos);}

    public float Grade(int[] startPos) { return Calculator.CheckDistanceToObj(position, startPos);}

    public float Fuck(int[] startPos, int[] objectivePos) { return Heuristic(objectivePos) + Grade(startPos); }

    public Token(int[] position, int cost)
    {
        this.position = position;
        this.cost = cost;
    }
}