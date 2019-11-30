using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Board board;

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }

        if (instance != this)
        {
            Destroy(gameObject);            
        }

        DontDestroyOnLoad(gameObject);

        board = GetComponent<Board>();
        board.MakeBoard();
    }
}
