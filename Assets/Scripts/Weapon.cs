using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _damage;
    [SerializeField] private int _price;
    [SerializeField] private bool _isBuyed;

    public string Name => _name;
    public int Damage => _damage;
    public int Price => _price;
    public Sprite Icon => _icon;    
    public bool IsBuyed => _isBuyed;
}
