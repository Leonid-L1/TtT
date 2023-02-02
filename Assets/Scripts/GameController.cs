using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraController _camera;
    [SerializeField] private ForestSpawnController _forestspawner;
    [SerializeField] private InteractableSpawner _interactableSpawner;
    [SerializeField] private MenuItems _menuItems;
    [SerializeField] private UiController _uiController;
    [SerializeField] private Player _player;
    [SerializeField] private ArenaSecondTrigger _trigger;
    [SerializeField] private BackgroundMusicHandler _musicHandler;
    [SerializeField] private SaveSystem _saver;

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
        _musicHandler.SetToRunnerPhaze();
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
        _trigger.ResetCondition();        
        _trigger.enabled = true;
        _musicHandler.SetToRunnerPhaze();
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
        _menuItems.gameObject.SetActive(true);
        _musicHandler.SetToMainMenu();
    }

    public void SetActionPhaze()
    {   
        List<Enemy> enemyList = _levelHandler.GetEnemies();
        _interactableSpawner.DisableAllInteractables();
        _forestspawner.ChangeMoveCondition(false);

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetComponent<Enemy>().SetTarget(_player);
        }
        _camera.SetActionState(enemyList[0].transform);
        _player.SetToActionPhaze(enemyList);
        _trigger.enabled = false;
        _menuItems.gameObject.SetActive(false);
    }

    private void OnLevelComplete(int newResult)
    {
        _uiController.OnLevelComplete(newResult);
        _player.GetComponent<PlayerCollectibles>().AddCompleteLevelCoins();
        _musicHandler.StopMusic();
        _saver.Save();
    }

    private void DeathOnRunnerPhaze()
    {
        _interactableSpawner.SetSpawnCondition(false);
        _interactableSpawner.ChangeMoveCondition(false);
        _forestspawner.ChangeMoveCondition(false);
        _uiController.OnPlayerDeath();
        _musicHandler.StopMusic();
    }

    private void DeathOnActionPhaze()
    {
        _uiController.OnPlayerDeath();
        _musicHandler.StopMusic();
    }
}
