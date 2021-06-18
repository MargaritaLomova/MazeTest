using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Variables To Level Control"), SerializeField]
    private float distanceFromMazeEnd = 8;

    [Header("Preafbs"), SerializeField]
    private CellInfo cellPrefab;
    [SerializeField]
    private MazeInfo mazePrefab;

    [Header("Objects From Scene"), SerializeField]
    private Transform player;
    [SerializeField]
    private CameraController localCamera;

    [Header("UI"), SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private GameObject deathInterface;
    [SerializeField]
    private TMP_Text finalScore;
    [SerializeField]
    private GameObject menuInterface;
    [SerializeField]
    private GameObject shopInterface;

    [HideInInspector]
    public bool isPlay;

    private List<MazeInfo> spawnedMazes = new List<MazeInfo>();
    private float timeFromStart;
    private Vector3 playerStartPosition;

    private void Start()
    {
        playerStartPosition = player.position;
        OnMenuClicked();
    }

    private void FixedUpdate()
    {
        if (isPlay)
        {
            GenerateEndlessMaze();

            if (!IsPlayerInCameraView())
            {
                Death();
            }

            UpdateScore();
        }
    }

    #region Custom Methods

    private void Death()
    {
        isPlay = false;
        deathInterface.SetActive(true);
        finalScore.text = $"You lasted {(int)timeFromStart} seconds!";
    }

    private bool IsPlayerInCameraView()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(player.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    private void UpdateScore()
    {
        timeFromStart += Time.fixedDeltaTime;
        scoreText.text = $"Your score: {(int)timeFromStart}";
    }

    #region Clicked

    public void OnPlayClicked()
    {
        player.position = playerStartPosition;
        localCamera.FastFocusOnPlayer();

        timeFromStart = 0;

        ClearAllMazes();

        isPlay = true;

        menuInterface.SetActive(false);
        deathInterface.SetActive(false);
        shopInterface.SetActive(false);

        var startMaze = GenerateNewMazePart();
        startMaze.name = $"maze_{spawnedMazes.Count}";
        spawnedMazes.Add(startMaze);
    }

    public void OnShopClicked()
    {
        menuInterface.SetActive(false);
        deathInterface.SetActive(false);
        shopInterface.SetActive(true);
    }

    public void OnRestratClicked()
    {
        OnPlayClicked();
    }

    public void OnMenuClicked()
    {
        menuInterface.SetActive(true);
        deathInterface.SetActive(false);
        shopInterface.SetActive(false);

        isPlay = false;
    }

    #endregion

    #region Maze

    private void GenerateEndlessMaze()
    {
        var nearestMazePart = GetNearestMazePart();

        var playerNearUp = player.position.x < nearestMazePart.up.position.x + distanceFromMazeEnd;
        var playerNearDown = player.position.x > nearestMazePart.down.position.x - distanceFromMazeEnd;
        var playerNearLeft = player.position.z < nearestMazePart.left.position.z + distanceFromMazeEnd;
        var playerNearRight = player.position.z > nearestMazePart.right.position.z - distanceFromMazeEnd;
        var isNeedToGenerateNewMazePart = playerNearUp || playerNearDown || playerNearLeft || playerNearRight;

        if (isNeedToGenerateNewMazePart)
        {
            MazeGenerator generator = new MazeGenerator();

            var mazeWithUpCordinates = new Vector3(nearestMazePart.up.position.x - generator.width * 2, 0, nearestMazePart.transform.position.z);
            var mazeWithDownCoordinates = new Vector3(nearestMazePart.down.position.x + generator.width * 2, 0, nearestMazePart.transform.position.z);
            var mazeWithLeftCoordinates = new Vector3(nearestMazePart.transform.position.x, 0, nearestMazePart.left.position.z - generator.height * 2);
            var mazeWithRightCoordinates = new Vector3(nearestMazePart.transform.position.x, 0, nearestMazePart.right.position.z + generator.height * 2);

            if (playerNearUp && spawnedMazes.Find(x => x.transform.position == mazeWithUpCordinates) == null)
            {
                AddMazePart(mazeWithUpCordinates);
                RemoveUselessMazes();
                if (spawnedMazes.Count == 1)
                {
                    RemoveNearestToPlayerWall();
                }
                return;
            }
            else if (playerNearDown && spawnedMazes.Find(x => x.transform.position == mazeWithDownCoordinates) == null)
            {
                AddMazePart(mazeWithDownCoordinates);
                RemoveUselessMazes();
                if (spawnedMazes.Count == 1)
                {
                    RemoveNearestToPlayerWall();
                }
                return;
            }
            else if (playerNearLeft && spawnedMazes.Find(x => x.transform.position == mazeWithLeftCoordinates) == null)
            {
                AddMazePart(mazeWithLeftCoordinates);
                RemoveUselessMazes();
                if (spawnedMazes.Count == 1)
                {
                    RemoveNearestToPlayerWall();
                }
                return;
            }
            else if (playerNearRight && spawnedMazes.Find(x => x.transform.position == mazeWithRightCoordinates) == null)
            {
                AddMazePart(mazeWithRightCoordinates);
                RemoveUselessMazes();
                if (spawnedMazes.Count == 1)
                {
                    RemoveNearestToPlayerWall();
                }
                return;
            }
        }
    }

    private void AddMazePart(Vector3 position)
    {
        var newMaze = GenerateNewMazePart();
        newMaze.name = $"maze_{spawnedMazes.Count}";

        newMaze.transform.position = position;
        spawnedMazes.Add(newMaze);
    }

    private void RemoveUselessMazes()
    {
        if (spawnedMazes.Count >= 8)
        {
            var farestMazePart = GetFarestMazePart();
            Destroy(farestMazePart.gameObject);
            spawnedMazes.Remove(farestMazePart);
        }
    }

    public void RemoveNearestToPlayerWall()
    {

        float minDist = Mathf.Infinity;
        Vector3 currentPlayerPos = player.position;
        List<GameObject> walls = new List<GameObject>();
        foreach (var mazePart in spawnedMazes)
        {
            foreach (var cell in mazePart.GetComponentsInChildren<CellInfo>())
            {
                walls.Add(cell.WallLeft);
                walls.Add(cell.WallBottom);
            }
        }

        foreach (var wall in walls)
        {
            float dist = Vector3.Distance(wall.transform.position, currentPlayerPos);
            if (dist < minDist)
            {
                wall.SetActive(false);
                minDist = dist;
            }
        }
    }

    private MazeInfo GetNearestMazePart()
    {
        MazeInfo nearestMazePart = new MazeInfo();
        float minDist = Mathf.Infinity;
        Vector3 currentPlayerPos = player.position;
        foreach (var mazePart in spawnedMazes)
        {
            float dist = Vector3.Distance(mazePart.transform.position, currentPlayerPos);
            if (dist < minDist)
            {
                nearestMazePart = mazePart;
                minDist = dist;
            }
        }
        return nearestMazePart;
    }

    private MazeInfo GetFarestMazePart()
    {
        MazeInfo nearestMazePart = new MazeInfo();
        float maxDist = 20;
        Vector3 currentPlayerPos = player.position;
        foreach (var mazePart in spawnedMazes)
        {
            float dist = Vector3.Distance(mazePart.transform.position, currentPlayerPos);
            if (dist > maxDist)
            {
                nearestMazePart = mazePart;
                maxDist = dist;
            }
        }
        return nearestMazePart;
    }

    private MazeInfo GenerateNewMazePart()
    {
        MazeGenerator generator = new MazeGenerator();
        MazeGeneratorCell[,] maze = generator.GenerateMaze();

        var newMaze = Instantiate(mazePrefab);
        newMaze.transform.localScale = new Vector3(generator.width * 4, 1, generator.height * 4);
        newMaze.transform.position = new Vector3(generator.width + (generator.width * 0.2f), 0, generator.height * 2);

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                var newCell = Instantiate(cellPrefab, new Vector3(x * 4, 0, y * 4), Quaternion.identity);
                newCell.WallLeft.SetActive(maze[x, y].leftWall);
                newCell.WallBottom.SetActive(maze[x, y].bottomWall);

                newCell.transform.SetParent(newMaze.transform);
            }
        }

        return newMaze;
    }

    private void ClearAllMazes()
    {
        foreach(var maze in spawnedMazes)
        {
            Destroy(maze.gameObject);
        }
        spawnedMazes.Clear();
    }

    #endregion

    #endregion
}