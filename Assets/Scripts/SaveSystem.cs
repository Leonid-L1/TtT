using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private const string CoinsCountKey = "CoinsCount";
    private const string LevelIndexKey = "LevelIndex";
    private const string IsLevelCompleteKey = "IsLevelComplete";
    private const string LevelResulKey = "LevelResult";

    [SerializeField] private LevelHandler _levelhandler;
    [SerializeField] private PlayerCollectibles _player;

    private int _coinsCount;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void Load()
    {
        if(PlayerPrefs.HasKey(CoinsCountKey))   
            _coinsCount = PlayerPrefs.GetInt(CoinsCountKey);
    }

    public void Save()
    {
        _coinsCount = _player.AllCoins;
        PlayerPrefs.SetInt(CoinsCountKey, _coinsCount);
    }
}

public class SaveLevelData
{
    public int LevelIndex { get; set; }
    public bool IsLevelComplete { get; set; }
    public int ResultInStars { get; set; }

    

}


