using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorCell
{
    public int x;
    public int y;

    public bool leftWall = true;
    public bool bottomWall = true;

    public bool Vizited = false;
}

public class MazeGenerator
{
    public int width = 5;
    public int height = 5;

    public MazeGeneratorCell[,] GenerateMaze()
    {
        MazeGeneratorCell[,] maze = new MazeGeneratorCell[width, height];

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                maze[x, y] = new MazeGeneratorCell { x = x, y = y };
            }
        }

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            maze[x, height - 1].leftWall = false;
        }
        for (int y = 0; y < maze.GetLength(1); y++)
        {
            maze[width - 1, y].bottomWall = false;
        }

        GenerateMazeWay(maze);

        return maze;
    }

    private void GenerateMazeWay(MazeGeneratorCell[,] maze)
    {
        MazeGeneratorCell current = maze[0, 0];
        current.Vizited = true;

        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();
        do
        {
            List<MazeGeneratorCell> unvizitedNeighbours = new List<MazeGeneratorCell>();

            if (current.x > 0 && !maze[current.x - 1, current.y].Vizited)
            {
                unvizitedNeighbours.Add(maze[current.x - 1, current.y]);
            }
            if (current.y > 0 && !maze[current.x, current.y - 1].Vizited)
            {
                unvizitedNeighbours.Add(maze[current.x, current.y - 1]);
            }
            if (current.x < width - 2 && !maze[current.x + 1, current.y].Vizited)
            {
                unvizitedNeighbours.Add(maze[current.x + 1, current.y]);
            }
            if (current.y < height - 2 && !maze[current.x, current.y + 1].Vizited)
            {
                unvizitedNeighbours.Add(maze[current.x, current.y + 1]);
            }


            if (unvizitedNeighbours.Count > 0)
            {
                MazeGeneratorCell choosen = unvizitedNeighbours[Random.Range(0, unvizitedNeighbours.Count)];
                RemoveWall(current, choosen);
                choosen.Vizited = true;
                current = choosen;
                stack.Push(choosen);
            }
            else
            {
                current = stack.Pop();
            }


        } while (stack.Count > 0);
    }

    private void RemoveWall(MazeGeneratorCell current, MazeGeneratorCell choosen)
    {
        if (current.x == choosen.x)
        {
            if (current.y > choosen.y)
            {
                current.bottomWall = false;
            }
            else
            {
                choosen.bottomWall = false;
            }
        }
        else
        {
            if (current.x > choosen.x)
            {
                current.leftWall = false;
            }
            else
            {
                choosen.leftWall = false;
            }
        }
    }
}