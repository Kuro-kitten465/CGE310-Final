using System;
using System.Collections.Generic;
using System.Linq;
using Kuro.Utilities.DesignPattern;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public enum FlagType { INT, BOOL, STRING, ERR, NOT_FOUND }
    public enum FlagState { ADD, UPDATE, GET, ERR }

    public readonly List<GameManagerData> GameManagerDataList = new();

    #if UNITY_EDITOR
    [Header("Editor Only")]
    [property: SerializeField] bool HasUmbrella { get {
        var flag = GameManagerDataList.FirstOrDefault(data => data.ID == "Umbrella");
        if (flag != null && flag.FlagType == FlagType.BOOL)
        {
            return (bool)flag.Value;
        }
        return false;
    }}
    #endif

    protected override void OnInitialize()
    {
        GameManagerDataList.Add(new GameManagerData("Umbrella", FlagType.BOOL, false));
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
