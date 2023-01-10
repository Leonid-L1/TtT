using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerCollectibles))]

public class SpellCast : MonoBehaviour
{
    private const string CastTrigger = "Cast";

    [SerializeField] private Button _castButton;
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private float _pauseBeforeThrow = 0.6f;

    private int _manaCost = 30;
    public event UnityAction<float> SpellCasting;
    private Transform _target;


    private Animator _animator;
    private PlayerCollectibles _player;

    private void OnEnable()
    {
        _castButton.onClick.AddListener(Cast);
    }

    private void OnDisable()
    {
        _castButton.onClick.RemoveListener(Cast);
    }

    private void Start()
    {
        _player = GetComponent<PlayerCollectibles>();
        _animator = GetComponent<Animator>();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
    private void Cast()
    {   
        if(_player.TryGetMana(_manaCost))
            StartCoroutine(CastSpell());
    }

    private IEnumerator CastSpell()
    {
        _animator.SetTrigger(CastTrigger);
        _castButton.interactable = false;
        GameObject fireball = Instantiate(_fireballPrefab, _leftHand);
        SpellCasting?.Invoke(_pauseBeforeThrow);

        yield return new WaitForSeconds(_pauseBeforeThrow);
        
        fireball.GetComponent<Fireball>().Init(_target);
        _castButton.interactable = true;

        yield break;
    }
}
