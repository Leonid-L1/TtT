
using UnityEngine;
using TMPro;

public class CoinsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _coins;
    [SerializeField] private PlayerCollectibles _playerCoins;

    private void OnEnable()
    {
        _playerCoins.CurrentLevelCoinsChanged += DisplayCurrentLevelCoins;
    }

    private void OnDisable()
    {
        _playerCoins.CurrentLevelCoinsChanged -= DisplayCurrentLevelCoins;
    }

    public void DisplayCurrentLevelCoins()
    {
        _coins.text = _playerCoins.CurrentLevelCoinsCount.ToString();
    }

    //public void DisplayAllCoins()
    //{
    //    _coins.text = _playerCoins.AllCoins.ToString();
    //}
}
