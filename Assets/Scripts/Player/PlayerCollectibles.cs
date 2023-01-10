using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollectibles : MonoBehaviour
{
    public event UnityAction<int> ManaCountChanged;
    //public event UnityAction<int> CoinsCountChanged;

    private int _mana;
    private int _coinsCount;
    private int _maxMana = 100;
    public int MaxMana => _maxMana;
 
    private void Start()
    {
        _mana = 0;
    }

    public void IncreaseCoinsCount()
    {
        _coinsCount++;
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
        _coinsCount = 0;
        //CoinsCountChanged?.Invoke(_mana);
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
}
