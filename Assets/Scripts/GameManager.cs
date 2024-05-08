using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject token1, token2, token3;
    private int[,] GameMatrix; //0 not chosen, 1 player, 2 enemy
    private int[] startPos = new int[2];
    private int[] objectivePos = new int[2];

    private int done = 0;

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
        // 1
        currentToken = new Token(startPos, objectivePos, 0);
        openList.Add(currentToken);

    }

    private void Update() {
        // 2
        if (openList.Count > 0) {CreatePath();}
        else if (done == 0) { Debug.Log("JODEEER"); done ++; }
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
    
    private void CreatePath() 
    {
            Debug.Log(currentToken.position[0] + " " + currentToken.position[1]);
            // 3, 4
            currentToken = FindLowestToken(openList);
            // 5
            if (FoundPath(currentToken)) return;
            // 6
            List<Token> succesorTokens = GetSuccesorTokens(currentToken);
            // 7
            foreach (Token token in succesorTokens) 
            {
                // 8
                float currentSuccesorCost = currentToken.G() + Calculator.distance;
                // 9
                if (openList.Find(t => t.position == token.position) != null)
                {
                    // 10
                    if (token.G() <= currentSuccesorCost) { ; continue; }
                }
                // 11
                else if (closedList.Find(t => t.position == token.position) != null) 
                {
                    // 12
                    if (token.G() <= currentSuccesorCost) { ; continue; }
                    // 13
                    FromOpenToClose(token);
                }
                // 14
                else 
                {
                    // 15
                    openList.Add(token);
                    // 16
                }
                // 18
                // 19
                token.parent = currentToken;
            }
            // 20, 21
            closedList.Add(currentToken);

    }

    private Token FindLowestToken(List<Token> list) 
    {
        Token lowestToken = list[0];
        foreach (Token token in list) 
        {
            if (token.F() <= lowestToken.F()) {lowestToken = token;} 
        }
        return lowestToken; 
    }

    private List<Token> GetSuccesorTokens(Token token)
    {
        List<Token> succesorTokens = new List<Token>();
        // HAY QUE MEJORAR
        succesorTokens.Add(new Token(new int[] {token.position[0] + 1, token.position[1]}, objectivePos, token.G() + Calculator.distance));
        succesorTokens.Add(new Token(new int[] {token.position[0] - 1, token.position[1]}, objectivePos, token.G() + Calculator.distance));
        succesorTokens.Add(new Token(new int[] {token.position[0], token.position[1] + 1}, objectivePos, token.G() + Calculator.distance));
        succesorTokens.Add(new Token(new int[] {token.position[0], token.position[1] - 1}, objectivePos, token.G() + Calculator.distance));

        return succesorTokens;
    }

    private bool FoundPath(Token token) 
    { 
        bool isWin = token.position[0] == objectivePos[0] && token.position[1] == objectivePos[1];
        Debug.Log(isWin ? "THE WAY: " + token.DebugParents() : $"Me están jodiendo {token.position[0]} {token.position[1]} {objectivePos[0]} {objectivePos[1]}\n {token.H()}");
        if (isWin) 
        {
            List<Token> pathList = new List<Token>();
            token.PaintPath(pathList);
            foreach(Token t in pathList) InstantiateToken(token3, t.position);
        }
        return isWin; 
    }

    private void FromCloseToOpen(Token token)
    {
        closedList.RemoveAt(closedList.IndexOf(token));
        openList.Add(token);
    }

    private void FromOpenToClose(Token token)
    {
        openList.RemoveAt(closedList.IndexOf(token));
        closedList.Add(token);

    }
    
    private void ShowList(List<Token> list) {
        string debugString = "";
        foreach (Token token in list) 
        {
            debugString += $"[{token.position[0]}, {token.position[1]}]: E = {token.H()}";
        }
        Debug.Log(debugString);
    }
}

class Token {
    public int[] position;
    private int[] objectivePos;
    private float cost;

    public Token parent;

    public float H() { return Calculator.CheckDistanceToObj(position, objectivePos); }

    public float G() { return cost; }

    public float F() { return H() + G(); }

    public string DebugParents() {
        string pos = $"{position[0]}, {position[1]} ";
        if (parent != null) 
        {
            pos += parent.DebugParents();
            return pos;
        }
        return pos;
    }

    public void PaintPath(List<Token> pathList) 
    {
        if (parent != null) {
            parent.PaintPath(pathList);
        }
        pathList.Add(this);
    }

    public Token(int[] position, int[] objectivePos, float cost)
    {
        this.position = position;
        this.objectivePos = objectivePos;
        this.cost = cost;
    }

}