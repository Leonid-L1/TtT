using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : Panel
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private PlayerCollectibles _playerMoney;
    [SerializeField] private WeaponView _template;
    [SerializeField] private GameObject _itemContainer;
    [SerializeField] private TMP_Text _coinsCount;
    [SerializeField] private SaveSystem _saver;
    [SerializeField] private PlayerWeapon _playerWeapon;

    public List<string> SoldWeaponNames { get; private set; } = new List<string>();

    private void OnEnable()
    {
        foreach (var weapon in _weapons)
            AddItem(weapon);
    }

    public override void MoveToScreen()
    {
        _coinsCount.text = _playerMoney.AllCoins.ToString();
        base.MoveToScreen();
    }

    public void Load(List<string>SoldWeaponNames)
    {
        Weapon lastSoldWeapon = _weapons.Find(weapon => weapon.Name == SoldWeaponNames[SoldWeaponNames.Count - 1]);
        
        _playerWeapon.SetNewWeapon(lastSoldWeapon);

        for (int i = 0; i < SoldWeaponNames.Count; i++)
        {
            var soldWeapon = _weapons.Find(weapon => weapon.Name == SoldWeaponNames[i]);
            _weapons.Remove(soldWeapon);
        }
    }

    private void AddItem(Weapon weapon)
    {
        var view = Instantiate(_template, _itemContainer.transform);

        view.OnSellButtonClick += OnSellButtonCLick;
        view.Render(weapon);
    }

    private void OnSellButtonCLick(Weapon weapon, WeaponView view)
    {
        if (_playerMoney.TryBuyWeapon(weapon))
        {
            view.OnSellButtonClick-=OnSellButtonCLick;
            view.Destroy();
            _coinsCount.text = _playerMoney.AllCoins.ToString();

            SoldWeaponNames.Add(weapon.Name);
            _weapons.Remove(weapon);
            _saver.Save();
        }
    }
}   
