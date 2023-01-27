using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Transform _weaponHolder;
    private Weapon _currentWeapon;

    public int CurrentWeaponDamage => _currentWeapon.Damage;

    private void Start()
    {
        _currentWeapon = _weapon.GetComponent<Weapon>();
    }

    public void SetNewWeapon(Weapon newWeapon)
    {
        Destroy(_currentWeapon.gameObject);

        var weapon = Instantiate(newWeapon, _weaponHolder);
        _currentWeapon = weapon;        
    }
}
