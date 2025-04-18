using System;
using System.Collections.Generic;
using System.Linq;
using Kuro.Utilities.DesignPattern;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public enum FlagType { INT, BOOL, STRING, ERR, NOT_FOUND }
    public enum FlagState { ADD, UPDATE, GET, ERR }

    public readonly List<GameManagerData> GameManagerDataList = new();

    protected override void OnInitialize()
    {
        GameManagerDataList.Add(new GameManagerData("Umbrella", FlagType.BOOL, false));
        GameManagerDataList.Add(new GameManagerData("Key", FlagType.BOOL, false));
        GameManagerDataList.Add(new GameManagerData("WinPuzzle", FlagType.BOOL, false));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Application is quitting...");
        }
    }

    public void AddFlag(string id, FlagType flagType, object value)
    {
        if (GameManagerDataList.Any(data => data.ID == id))
        {
            Debug.LogWarning($"Flag with ID {id} already exists. Use UpdateFlag to modify it.");
            return;
        }

        GameManagerDataList.Add(new GameManagerData(id, flagType, value));
    }

    public void UpdateFlag(string id, object value)
    {
        var flag = GameManagerDataList.FirstOrDefault(data => data.ID == id);
        if (flag == null)
        {
            Debug.LogWarning($"Flag with ID {id} does not exist. Use AddFlag to create it.");
            return;
        }

        flag.Value = value;
    }

    public FlagType GetFlag(string id, out object value)
    {
        var flag = GameManagerDataList.FirstOrDefault(data => data.ID == id);
        if (flag == null)
        {
            Debug.LogWarning($"Flag with ID {id} does not exist.");
            value = null;
            return FlagType.NOT_FOUND;
        }

        value = flag.Value;
        return flag.FlagType;
    }
}

[Serializable]
public class GameManagerData
{
    public string ID;
    public GameManager.FlagType FlagType;
    public object Value;

    public GameManagerData(string id, GameManager.FlagType flagType, object value)
    {
        ID = id;
        FlagType = flagType;
        Value = value;
    }
}
