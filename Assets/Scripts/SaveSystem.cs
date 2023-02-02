using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static string SavePath = "/Saves/" + "/Save.json";

    [SerializeField] private LevelHandler _levelhandler;
    [SerializeField] private PlayerCollectibles _player;
    [SerializeField] private Shop _shop;

    private SaveFile _saveFile;

    private void Start()
    {
        Load();
    }

    public void Load()
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
        };
        
        _saveFile = JsonConvert.DeserializeObject<SaveFile>(File.ReadAllText(Application.streamingAssetsPath + SavePath), settings);

        if( _saveFile.Coins > 0)
            _player.Load(_saveFile.Coins);   
        
        if(_saveFile.LevelsResult.Count > 0)
            _levelhandler.LoadSavingData(_saveFile.LevelsResult);

        if (_saveFile.SoldWeaponNames!= null && _saveFile.SoldWeaponNames.Count > 0)
            _shop.Load(_saveFile.SoldWeaponNames);
    }

    public void Save()
    {
        _saveFile = new SaveFile();
        _saveFile.SetToSave(_shop.SoldWeaponNames, _levelhandler.GetLevelsResultToSave(), _player.AllCoins);

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None
        };

        File.WriteAllText(Application.streamingAssetsPath + SavePath, JsonConvert.SerializeObject(_saveFile, settings));    
    }
}
public class SaveFile
{
    public List<string> SoldWeaponNames;
    public List<int> LevelsResult;
    public int Coins;

    public void SetToSave(List<string> soldWeaponNames, List<int> levelsResult, int coins)
    {
        LevelsResult = levelsResult;
        Coins = coins;

        if (soldWeaponNames.Count > 0)
            SoldWeaponNames = soldWeaponNames;
    }  
}



