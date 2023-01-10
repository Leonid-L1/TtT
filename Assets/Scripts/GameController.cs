using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraController _camera;
    [SerializeField] private ForestSpawnController _forestspawner;
    [SerializeField] private InteractableSpawner _interactableSpawner;
    [SerializeField] private MenuItems _menuItems;
    [SerializeField] private UiController _uiController;
    [SerializeField] private Player _player;
    [SerializeField] private ArenaSecondTrigger _trigger;

    private LevelHandler _levelHandler;
    private void Awake()
    {
        _levelHandler = GetComponent<LevelHandler>();
    }

    private void OnEnable()
    {
        _camera.MovedToRunPosition += StartLevel;
        _player.DeadOnRunnerPhaze += DeathOnRunnerPhaze;
        _player.DeadOnActionPhaze += DeathOnActionPhaze;
        _levelHandler.LevelComplete += OnLevelComplete;
    }

    private void OnDisable()
    {
        _camera.MovedToRunPosition -= StartLevel;
        _player.DeadOnRunnerPhaze -= DeathOnRunnerPhaze;
        _player.DeadOnActionPhaze -= DeathOnActionPhaze;
        _levelHandler.LevelComplete += OnLevelComplete;
    } 

    private void StartLevel()
    {   
        _forestspawner.ChangeMoveCondition(true);
        _interactableSpawner.gameObject.SetActive(true);
        _interactableSpawner.SetSpawnCondition(true);
        _interactableSpawner.ChangeMoveCondition(true);
        _player.StartRun();
    }

    public void RestartLevel()
    {
        _camera.Restart();
        _player.Restart();
        _forestspawner.Restart();
        _levelHandler.Restart();
        _uiController.Restart();
        _interactableSpawner.DisableAllInteractables();
        _interactableSpawner.ChangeMoveCondition(true);
        _interactableSpawner.SetSpawnCondition(true);
        _trigger.enabled = true;
        _trigger.ResetCondition();
        //SceneManager.LoadScene(0);
        
    }

    public void SetMainMenuFromGame()
    {
        _interactableSpawner.DisableAllInteractables();;
        _interactableSpawner.gameObject.SetActive(false);
        _forestspawner.Restart();
        _forestspawner.ChangeMoveCondition(false);
        _camera.Restart();
        _camera.MoveToMenuPosition();
        _player.SetToMainMenu();
        _levelHandler.SetToMainMenu();
        _trigger.enabled = true;
        _trigger.ResetCondition();
        _uiController.SetMainMenu();
    }

    public void SetActionPhaze()
    {   
        List<GameObject> enemyList = _levelHandler.GetEnemies();

        _interactableSpawner.DisableAllInteractables();
        _camera.SetActionState(enemyList[0].transform);
        _forestspawner.ChangeMoveCondition(false);
        _player.SetToActionPhaze(enemyList);
        _trigger.enabled = false;

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetComponent<Enemy>().SetTarget(_player);
        }
    }

    private void OnLevelComplete()
    {
        _uiController.OnLevelComplete();
    }

    private void DeathOnRunnerPhaze()
    {
        _interactableSpawner.SetSpawnCondition(false);
        _interactableSpawner.ChangeMoveCondition(false);
        _forestspawner.ChangeMoveCondition(false);
        _uiController.OnPlayerDeath();
    }

    private void DeathOnActionPhaze()
    {
        _uiController.OnPlayerDeath();
    }
}
