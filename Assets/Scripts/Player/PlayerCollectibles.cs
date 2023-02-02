using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(PlayerWeapon))]

public class PlayerCollectibles : MonoBehaviour
{
    public event UnityAction<int> ManaCountChanged;    
    public event UnityAction CurrentLevelCoinsChanged;

    private int _mana;
    private int _currentLevelCoinsCount = 0;
    private int _allCoins;
    private int _maxMana = 100;

    private PlayerWeapon _weaponHandler;
    public int MaxMana => _maxMana;
    public int CurrentLevelCoinsCount => _currentLevelCoinsCount;
    public int AllCoins => _allCoins;

    private void Start()
    {
        _mana = 0;
        _weaponHandler = GetComponent<PlayerWeapon>();
    }

    public void Load(int coinsCount)
    {
        _allCoins = coinsCount;
    }

    public void IncreaseCoinsCount()
    {
        _currentLevelCoinsCount++;
        CurrentLevelCoinsChanged?.Invoke();
    }

    public void IncreaseMana(int mana)
    {
        if (_maxMana < (_mana + mana))
        {
            _mana = _maxMana;
        }
        else
        {
            _mana += mana;
        }
        ManaCountChanged?.Invoke(_mana);
    }

    public void ResetMana() 
    {
        _mana = 0;
        ManaCountChanged?.Invoke(_mana);
    }

    public void ResetCoins()
    {
        _currentLevelCoinsCount = 0;
        CurrentLevelCoinsChanged?.Invoke();
    }

    public void AddCompleteLevelCoins()
    {
        _allCoins += _currentLevelCoinsCount;
    }

    public bool TryGetMana(int manaCost)
    {
        if(manaCost <= _mana)
        {
            _mana -= manaCost;
            ManaCountChanged?.Invoke(_mana);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryBuyWeapon(Weapon weapon)
    {
        if(weapon.Price <= _allCoins)
        {
            _allCoins -= weapon.Price;
            _weaponHandler.SetNewWeapon(weapon);
            return true;
        }
        else
        {
            return false;
        }
    }
}
