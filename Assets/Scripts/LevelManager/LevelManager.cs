using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform levelContainer;
    //public List<GameObject> levels;

    [Header("Pieces")]
    public LevelPieceBase emptyPiece;
    public List<LevelPieceBase> startLevelPieces;
    public List<LevelPieceBase> levelPieces;
    public List<LevelPieceBase> endLevelPieces;

    public int numberOfStartPieces = 3;
    public int numberOfPieces;
    public int numberOfEndPieces = 1;
    [Space]
    public float timeBetweenSpawns = .3f;

    [SerializeField] int _index;
    GameObject _currLevel;
    List<LevelPieceBase> _spawnedPieces;

    private void Awake()
    {
        //SpawnNextLevel();
        CreateLevel();
    }


    #region === Spawn ready track ===
    //tester SpawnNextLevel com a pista montada
    /*void SpawnNextLevel()
    {
        if (_currLevel != null)
        {
            Destroy(_currLevel);
            _index++;

            if (_index >= levels.Count)
            {
                ResetIndexLevel();
            }
        }

        _currLevel = Instantiate(levels[_index], levelContainer);
        //currLevel.transform.position = Vector3.zero; //serve para instanciar na posi��o 0.
    }

    void ResetIndexLevel()
    {
        _index = 0;
    }*/
    #endregion


    #region === Spawn Track With Random Pieces ===

    //Para montar a pista com pe�as randomizadas
    void CreatePieces(List<LevelPieceBase> list)
    {
        var piece = list[Random.Range(0, list.Count)];
        var spawnedPiece = Instantiate(piece, levelContainer);

        if (_spawnedPieces.Count > 0)
        {
            var lastPiece = _spawnedPieces[_spawnedPieces.Count - 1];

            spawnedPiece.transform.position = lastPiece.endPiece.position;
        }

        _spawnedPieces.Add(spawnedPiece);
    }

    void CreateLevel()
    {
        BalanceWithEmptyPieces();
        StartCoroutine(CreateLevelPiecesCoroutine());

        /*_spawnedPieces = new List<LevelPieceBase>();

        for (int i = 0; i < numberOfPieces; i++)
        {
            CreatePieces();
        }*/
    }

    IEnumerator CreateLevelPiecesCoroutine()
    {
        _spawnedPieces = new List<LevelPieceBase>();

        for (int i = 0; i < numberOfStartPieces; i++)
        {
            CreatePieces(startLevelPieces);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        for (int i = 0; i < numberOfPieces; i++)
        {
            CreatePieces(levelPieces);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        for (int i = 0; i < numberOfEndPieces; i++)
        {
            CreatePieces(endLevelPieces);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        Actions.findFemaleAnim();
    }

    void BalanceWithEmptyPieces()
    {
        var numberOfEmptys = levelPieces.Count / 3;

        for (int i = 0; i < numberOfEmptys; i++)
        {
            levelPieces.Add(emptyPiece);
        }

    }
    #endregion
}
