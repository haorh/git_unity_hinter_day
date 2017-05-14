using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcher:MonoBehaviour{

    SceneObjectManager _sceneObjectManager;
    ScenePlacer _scenePlacer;

    int _currentScene = 0;

    void Start()
    {
        _scenePlacer = GameObject.FindObjectOfType<ScenePlacer>();
        _sceneObjectManager = GameObject.FindObjectOfType<SceneObjectManager>();        
    }

    public void TeleportPlayer(GameObject gameObject)
    {
        if (_currentScene == 0)
        {
            _currentScene = 1;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + _scenePlacer.distance, gameObject.transform.position.z);
        }
        else
        {
            _currentScene = 0;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - _scenePlacer.distance, gameObject.transform.position.z);
        }
    }

    public void TeleportPlayer(GameObject gameObject, int index)
    {
        if (_currentScene > index)
        {
            _sceneObjectManager.gameObject.transform.position = new Vector3(_sceneObjectManager.gameObject.transform.position.x, _sceneObjectManager.gameObject.transform.position.y - (_currentScene - index) * _scenePlacer.distance, _sceneObjectManager.gameObject.transform.position.z);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (_currentScene - index) * _scenePlacer.distance, gameObject.transform.position.z);
            _sceneObjectManager.SceneChanged(index);
        }
        else
        {
            _sceneObjectManager.gameObject.transform.position = new Vector3(_sceneObjectManager.gameObject.transform.position.x, _sceneObjectManager.gameObject.transform.position.y + (index - _currentScene) * _scenePlacer.distance, _sceneObjectManager.gameObject.transform.position.z);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (index - _currentScene) * _scenePlacer.distance, gameObject.transform.position.z);
            _sceneObjectManager.SceneChanged(index);
        }

        _currentScene = index;
    }

    public int GetSceneIndex()
    {
        return _currentScene;
    }
}
