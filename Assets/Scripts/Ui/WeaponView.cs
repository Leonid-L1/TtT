using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animation))]
public class WeaponView : MonoBehaviour
{
    [SerializeField] private TMP_Text _price;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _sellButton;

    public UnityAction<Weapon, WeaponView> OnSellButtonClick;

    private float _animationDuration = 0.25f;
    private Weapon _weapon;
    private Animation _animation;

    private void OnEnable()
    {
        _sellButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _sellButton.onClick.RemoveListener(OnButtonClick);
    }

    private void Start()
    {
        _animation = GetComponent<Animation>();
    }

    public void Render(Weapon weapon)
    {
        _weapon = weapon;
        _price.text = weapon.Price.ToString();
        _icon.sprite = weapon.Icon;
    }

    public void Destroy()
    {
        _animation.Play();
        
        Destroy(gameObject,_animationDuration);
    }

    private void OnButtonClick()
    {
        OnSellButtonClick?.Invoke(_weapon, this);
    }
}
