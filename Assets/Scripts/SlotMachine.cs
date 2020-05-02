using System;
using System.Collections.Generic;
using UnityEngine;
using SlotInput;

[Serializable]
public class SlotMachine : MonoBehaviour
{
    public static bool isInGame = false;


    #region Editor
    [Header("Format: n-n-n-n")]
    [SerializeField]
    private List<string> paylines;
    [SerializeField]
    private List<Column> columns;
    [SerializeField]
    private int rowsAmount;
    [SerializeField]
    private int betChangeAmount = 1;
    [SerializeField]
    private int balance = 1000;
    [SerializeField]
    private UIController uiController;
    #endregion

    private int columnsStand = 0;
    private List<int[]> board;
    private List<int[]> paylinesInt = new List<int[]>();
    private int currentBet = 1;
    
     
    void Awake()
    {
        columns.ForEach(column =>
        {
           column.RowsVisible = rowsAmount;
           column.SpinEnd += OnSpinEnd;
        });
    }


    private void Start()
    {
        board = new List<int[]>();
        for(int i = 0; i < columns.Count; i++)
            board.Add(null);

        ReadPaylines();

        uiController.Balance = balance;
        uiController.Bet = currentBet;

        var input = GetComponent<BaseInput>();
        input.BetChanged += ChangeBet;
    }


    void ChangeBet(int multiplier)
    {
        if(currentBet + multiplier * betChangeAmount > 0 && !isInGame)
        {
            currentBet += multiplier * betChangeAmount;
            uiController.Bet = currentBet;
        }
    }


    void OnSpinEnd(Column column, int[] elements)
    {
        columnsStand++;
        var ind = columns.IndexOf(column);
        board[ind] = elements;
        if(columnsStand == columns.Count)
        {
            if(isMatches())            
                balance += currentBet * 100;           
            else
                balance -= currentBet;

            uiController.Balance = balance;

            columnsStand = 0;
            isInGame = false;
        }
    }


    bool isMatches()
    {
        foreach(var payline in paylinesInt)
        {
            bool match = true;
            for(int i = 0; i < board.Count; i++)
            {
                if(board[i][payline[i]] != board[0][payline[0]])
                {
                    match = false;
                    break;
                }
            }            
            if(match)
                return true;
        }
        return false;
    }


    void ReadPaylines()
    {
        foreach(var str in paylines)
        {
            var separeted = str.Split(new char[] { '-' });
            var numbers = new int[columns.Count];
            for(int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = Convert.ToInt32(separeted[i]);
            }
            paylinesInt.Add(numbers);
        }
    }


    private void OnDestroy()
    {
        isInGame = false;
    }
}
