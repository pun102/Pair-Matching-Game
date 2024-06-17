using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private readonly Dictionary<EPuzzleCategories, string> _tilesDirectory = new Dictionary<EPuzzleCategories, string>();
    private int _settings;
    private const int SettingsNumber = 2;
    public enum EPairNumber
    {
        NotSet = 0,
        E10Pairs = 10,
        E15Pairs = 15,
        E20Pairs = 20,
    }
    public enum EPuzzleCategories
    {
        NotSet,
        Tiles1,
        Tiles2,
    }
    public struct Settings
    {
        public EPairNumber PairsNumber;
        public EPuzzleCategories PuzzleCategory;
    };

    private Settings _gameSettings;

    public static GameSettings Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(target:this);
            Instance = this;
        }
        else
        {
            Destroy(obj: this);
        }
    }
    private void Start()
    {
        SetTilesDirectory();
        _gameSettings = new Settings();
        ResetGameSettings();
    }

    private void SetTilesDirectory()
    {
        _tilesDirectory.Add(EPuzzleCategories.Tiles1, "Tiles1");
        _tilesDirectory.Add(EPuzzleCategories.Tiles2, "Tiles2");
    }

    public void SetPairNumber(EPairNumber Number)
    {
        if(_gameSettings.PairsNumber == EPairNumber.NotSet)
            _settings++;

        _gameSettings.PairsNumber = Number;
    }

    public void SetPuzzleCategories(EPuzzleCategories cat)
    {
        if(_gameSettings.PuzzleCategory == EPuzzleCategories.NotSet)
            _settings++;

        _gameSettings.PuzzleCategory = cat;
    }

    public EPairNumber GetPairNumber()
    {
        return _gameSettings.PairsNumber;
    }

    public EPuzzleCategories GetPuzzleCategory()
    {
        return _gameSettings.PuzzleCategory;
    }

    public void ResetGameSettings()
    {
        _settings = 0;
        _gameSettings.PuzzleCategory = EPuzzleCategories.NotSet;
        _gameSettings.PairsNumber = EPairNumber.NotSet;
    }

    public bool AllSettingsReady()
    {
        return _settings == SettingsNumber;
    }
    public string GetMaterialDirectoryName()
    {
        return "Materials/";
    }
    public string GetTilesCategoryTextureDirectoryName()
    {
        if(_tilesDirectory.ContainsKey(_gameSettings.PuzzleCategory))
        {
            return "Sprites/Tiles/" + _tilesDirectory[_gameSettings.PuzzleCategory] + "/";
        }
        else
        {
            Debug.LogError("ERROR: CANNOT GET DIRECTORY NAME");
            return "";
        }
    }
}
