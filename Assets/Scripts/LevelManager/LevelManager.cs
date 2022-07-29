using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform levelContainer;
    public List<GameObject> levels;

    [Header("Pieces")]
    public List<GameObject> levelPieces;
    public int numberOfPieces;

    [SerializeField] int _index;
    [SerializeField] GameObject _currLevel;
    [SerializeField] List<GameObject> _spawnedPieces;

    private void Awake()
    {
        //SpawnNextLevel();
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

    void CreatePieces()
    {
        var piece = levelPieces[Random.Range(0, levelPieces.Count)];
        var spawnedPiece = Instantiate(piece, levelContainer);

        if(_spawnedPieces.Count > 0)
        {
            var lastPiece = _spawnedPieces[_spawnedPieces.Count - 1];
        }

        _spawnedPieces.Add(spawnedPiece);
    }

    void CreateLevel()
    {
        _spawnedPieces = new List<GameObject>();

        for (int i = 0; i < numberOfPieces; i++)
        {
            CreatePieces();
        }
    }
}
