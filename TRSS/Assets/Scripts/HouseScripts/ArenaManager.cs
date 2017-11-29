using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour {

    int grid_x = 10;
    int grid_y = 10;

<<<<<<< HEAD
    int roads = 0;

    GameObject[][] arena;
=======
    public GameObject[][] arena;
>>>>>>> 3c43cafa1495a922327c71e0cb9180d2477f57a5

    System.Random rnd = new System.Random();

    private IEnumerator coroutine;

    private int newX, newY;

    // Use this for initialization
    void Start () {
        arena = new GameObject[grid_x][];
        for (int x = 0; x < grid_x; x++)
        {
            arena[x] = new GameObject[grid_y];
            for(int y = 0; y < grid_y; y++)
            {
                    //GameObject road = Resources.Load("Road") as GameObject;
                    //arena[x][y] = (GameObject)Instantiate(road, new Vector3(x * 10 - 45,.5f, y * 10 - 45), Quaternion.identity);
            }
        }

        float road_tiles = grid_x * grid_y * .5f;

        // 0 = North, 1 = East, 2 = South, 3 = West
        float start_edge = UnityEngine.Random.Range(0, 4);

        int startRoadX, startRoadY;

        if(start_edge < 1f)
        {
            startRoadX = 0;
            startRoadY = (int)UnityEngine.Random.Range(0, grid_y - 1);
        }
        else if (start_edge < 2f)
        {
            startRoadX = (int)UnityEngine.Random.Range(0, grid_x - 1);
            startRoadY = grid_y - 1;
        }
        else if (start_edge < 3f)
        {
            startRoadX = grid_x - 1;
            startRoadY = (int)UnityEngine.Random.Range(0, grid_y - 1);
        }
        else
        {
            startRoadX = (int)UnityEngine.Random.Range(0, grid_x - 1);
            startRoadY = 0;
        }

        roads = (int)road_tiles;
        GameObject road = Resources.Load("Road") as GameObject;
        arena[startRoadX][startRoadY] = (GameObject)Instantiate(road, new Vector3(startRoadX * 10 - 45,.5f, startRoadY * 10 - 45), Quaternion.identity);

        coroutine = Generate_Road(startRoadX, startRoadY);

        StartCoroutine(coroutine);
        //Place_New_Road_Tile(startRoadX, startRoadY, (int)road_tiles);

        //if(startRoadX < .5f)
        //{
        //    //x = 0;
        //}
    }

    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator Generate_Road(int startRoadX, int startRoadY)
    {
        newX = startRoadX;
        newY = startRoadY;
        while(roads >= 0)
        {
            yield return null;
            Place_New_Road_Tile(newX, newY);
        }
        StopCoroutine(coroutine);
    }

    /**
     * Recursive method that attepts to create a new road tile. If the
     * random placement fails then it tries again. If the random placement
     * is successful then it creates a new one with one less tile remaining
     * to be created.
     */
    void Place_New_Road_Tile(int x, int y)
    {
        if(roads == 0)
        {
            StopCoroutine(coroutine);
        }

        int newX, newY;

        // 0 = North, 1 = East, 2 = South, 3 = West
        int direction = 0;

        if (x == 0 && y == 0)
        {
            // North or East
            direction = rnd.Next(0, 2);
        }
        else if (x == grid_x -1 && y == grid_y -1)
        {
            // West or South
            direction = rnd.Next(2, 4);
        }
        else if (x == 0 && y == grid_y -1)
        {
            // South or East
            direction = rnd.Next(1, 3);
        }

        else if(x == grid_x -1 && y == 0)
        {
            // North or West
            direction = rnd.Next(0, 4);

            while (direction == 2 || direction == 1)
            {
                direction = rnd.Next(0, 4);
            }
        }

        else if (x == 0)
        {
            direction = rnd.Next(0, 3);
        }

        else if (x == grid_x - 1)
        {
            direction = rnd.Next(0, 4);
            while (direction == 1)
            {
                direction = rnd.Next(0, 4);
            }   
        }

        else if (y == 0)
        {
            direction = rnd.Next(0, 4);
            while (direction == 2)
            {
                direction = rnd.Next(0, 4);
            }
            
        }

        else if (y == grid_y - 1)
        {
            direction = rnd.Next(1, 4);
        }
        else
        {
            direction = rnd.Next(0, 4);
        }

        // 0 = North, 1 = East, 2 = South, 3 = West
        switch (direction)
        {
            case 0:
                newX = x;
                newY = y + 1;
                break;
            case 1:
                newX = x + 1;
                newY = y;
                break;
            case 2:
                newX = x;
                newY = y - 1;
                break;
            case 3:
                newX = x - 1;
                newY = y;
                break;
            default:
                newX = x;
                newY = y;
                break;
        }

        this.newX = newX;
        this.newY = newY;

        if (arena[newX][newY] == null)
        {
           if(RoadDoesntMakeA2x2(newX, newY))
            {
                GameObject road = Resources.Load("Road") as GameObject;
                arena[newX][newY] = (GameObject)Instantiate(road, new Vector3(newX * 10 - 45, .5f, newY * 10 - 45), Quaternion.identity);
                Debug.Log("Placing road at " + newX + " " + newY);
                roads--;

                
            } else
            {
                //Place_New_Road_Tile(x, y, tiles);
                //yield return Place_New_Road_Tile(newX, newY, tiles);
            }
            
        }
        else
        {
            //Place_New_Road_Tile(newX, newY, tiles);
            //yield return Place_New_Road_Tile(newX, newY, tiles - 1);
        }
    }

    bool RoadDoesntMakeA2x2(int x, int y)
    {
        if(x > 0 && y > 0 && arena[x-1][y] && arena[x-1][y-1] && arena[x][y-1])
        {
            return false;
        }
        else if(x < grid_x -1 && y > 0 && arena[x][y - 1] && arena[x + 1][y] && arena[x + 1][y - 1])
        {
            return false;
        }
        else if(x > 0 && y < grid_y - 1 && arena[x-1][y + 1] && arena[x][y + 1] && arena[x - 1][y + 1])
        {
            return false;
        }
        else if (x < grid_x - 1 && y < grid_y - 1 && arena[x + 1][y + 1] && arena[x][y + 1] && arena[x + 1][y])
        {
            return false;
        }
        else
        {
            return true;
        }

    }

}
