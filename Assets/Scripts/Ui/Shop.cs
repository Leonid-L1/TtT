using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : Panel
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private PlayerCollectibles _player;
    [SerializeField] private WeaponView _template;
    [SerializeField] private GameObject _itemContainer;
    [SerializeField] private TMP_Text _coinsCount;   

    private void Start()
    {
        foreach (var weapon in _weapons)
            AddItem(weapon);
    }

    public override void MoveToScreen()
    {
        _coinsCount.text = _player.AllCoins.ToString();
        base.MoveToScreen();
    }

    private void AddItem(Weapon weapon)
    {
        var view = Instantiate(_template, _itemContainer.transform);

        view.OnSellButtonClick += OnSellButtonCLick;
        view.Render(weapon);
    }

    private void OnSellButtonCLick(Weapon weapon, WeaponView view)
    {
        if (_player.TryBuyWeapon(weapon))
        {
            view.OnSellButtonClick-=OnSellButtonCLick;
            view.Destroy();
            _coinsCount.text = _player.AllCoins.ToString();
        }
    }
}   
