using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private Transform _weaponHolder;
    public int CurrentWeaponDamage => _currentWeapon.Damage;

    public void SetNewWeapon(Weapon newWeapon)
    {
        Destroy(_currentWeapon.gameObject);

        _currentWeapon = Instantiate(newWeapon, _weaponHolder);
    }
}
