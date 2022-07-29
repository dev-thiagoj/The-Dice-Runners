using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform levelContainer;
    public List<GameObject> levels;

    [SerializeField] int _index;
    [SerializeField] GameObject _currLevel;

    private void Awake()
    {
        SpawnNextLevel();
    }

    
    [NaughtyAttributes.Button]
    void SpawnNextLevel()
    {
        if(_currLevel != null)
        {
            Destroy(_currLevel);
            _index++;

            if(_index >= levels.Count)
            {
                ResetIndexLevel();
            }
        }

        _currLevel = Instantiate(levels[_index], levelContainer);
        //currLevel.transform.position = Vector3.zero; //serve para instanciar na posição 0.
    }

    void ResetIndexLevel()
    {
        _index = 0;
    }

    
}
