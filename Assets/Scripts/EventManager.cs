using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public event Action<int, int> onClickTileCoord;
    public void ClickTileCoord (int x, int y)
    {
        if (onClickTileCoord != null)
        {
            onClickTileCoord(x, y);
        }
    }

    public event Action<int, int> onSpawnNextEnemy;
    public void SpawnNextEnemy(int x, int y)
    {
        if (onSpawnNextEnemy != null)
        {
            onSpawnNextEnemy(x, y);
        }
    }

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
    }
}