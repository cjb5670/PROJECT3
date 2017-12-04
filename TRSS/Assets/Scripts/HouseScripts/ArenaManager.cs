using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour {

    int grid_x = 15;
    int grid_y = 10;

    int roads = 0;

    public GameObject[][] arena;

    public GameObject[] lampPosts;

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
                    GameObject ground = Resources.Load("Grass") as GameObject;
                    ground = (GameObject)Instantiate(ground, new Vector3(x * 10 - 70, .0f, y * 10 - 45), Quaternion.identity);
                //arena[x][y] = (GameObject)Instantiate(road, new Vector3(x * 10 - 45,.5f, y * 10 - 45), Quaternion.identity);
            }
        }

        float road_tiles = grid_x * grid_y * .6f;

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
        lampPosts = new GameObject[roads + 1];
        GameObject mailtruck = Resources.Load("MailTruck") as GameObject;
        GameObject mailTruck_prefab = (GameObject)Instantiate(mailtruck, new Vector3(startRoadX * 10 - 70, .5f, startRoadY * 10 - 45), Quaternion.identity);
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().SetTarget(mailTruck_prefab);
        mailTruck_prefab.GetComponent<Health>().health = GameObject.Find("Foreground").GetComponent<RectTransform>();
        //coroutine = Generate_Road(startRoadX, startRoadY);

        //StartCoroutine(coroutine);
        Generate_Road_nonAsync(startRoadX, startRoadY);

        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().SetLampPostArray(lampPosts);
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().mailtruck = mailTruck_prefab;
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().Initialize();
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

    void Generate_Road_nonAsync(int startRoadX, int startRoadY)
    {
        newX = startRoadX;
        newY = startRoadY;
        while (roads >= 0)
        {
            Place_New_Road_Tile(newX, newY);
        }

        CreateRoads();
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
            //StopCoroutine(coroutine);
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
                int road_type = rnd.Next(0, 3);
                GameObject road;
                switch (road_type)
                {
                    case 0:
                        road = Resources.Load("Road") as GameObject;
                        break;
                    case 1:
                        road = Resources.Load("RoadBenchMailbox") as GameObject;
                        break;
                    case 2:
                        road = Resources.Load("RoadLamp1") as GameObject;
                        break;
                    default:
                        road = Resources.Load("Road") as GameObject;
                        break;
                }

                arena[newX][newY] = new GameObject(); // (GameObject)Instantiate(road, new Vector3(newX * 10 - 70, .5f, newY * 10 - 45), Quaternion.identity);
                //if(road_type == 2)
                //    lampPosts[roads] = arena[newX][newY];
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

    void CreateRoads()
    {
        int lamps_count = 0;

        for(int i = 0; i < arena.Length; i++)
        {
            for (int j = 0; j < arena[i].Length; j++)
            {
                if(arena[i][j] != null)
                {
                    String directions = GetDirections(i, j);
                    GameObject road = null;
                    switch (directions)
                    {
                        case "N":
                            road = Resources.Load("RoadS") as GameObject;
                            
                            
                            break;
                        case "E":
                            road = Resources.Load("RoadE") as GameObject;
                            
                            break;
                        case "S":
                            road = Resources.Load("RoadN") as GameObject;
                            break;
                        case "W":
                            road = Resources.Load("RoadW") as GameObject;
                            break;
                        case "NE":
                            road = Resources.Load("RoadNE") as GameObject;
                            break;
                        case "NS":
                            int lamp = rnd.Next(0, 2);
                            if(lamp == 0)
                            {
                                road = Resources.Load("RoadNS") as GameObject;
                            } else
                            {
                                road = Resources.Load("Road2NS") as GameObject;
                                arena[i][j] = (GameObject)Instantiate(road, new Vector3(i * 10 - 70, 0, j * 10 - 45), road.transform.rotation);
                                lampPosts[lamps_count] = arena[i][j];
                                road = null;
                                lamps_count++;
                            }
                            
                            break;
                        case "NW":
                            road = Resources.Load("RoadNW") as GameObject;
                            break;
                        case "ES":
                            road = Resources.Load("RoadES") as GameObject;
                            break;
                        case "EW":
                            lamp = rnd.Next(0, 3);
                            if (lamp == 0)
                            {
                                road = Resources.Load("RoadEW") as GameObject;
                            }
                            else
                            {
                                road = Resources.Load("Road2EW") as GameObject;
                                arena[i][j] = (GameObject)Instantiate(road, new Vector3(i * 10 - 70, 0, j * 10 - 45), road.transform.rotation);
                                lampPosts[lamps_count] = arena[i][j];
                                road = null;
                                lamps_count++;
                            }
                            break;
                        case "SW":
                            road = Resources.Load("RoadSW") as GameObject;
                            break;
                        case "ESW":
                            road = Resources.Load("RoadESW") as GameObject;
                            break;
                        case "NES":
                            road = Resources.Load("RoadNES") as GameObject;
                            break;
                        case "NEW":
                            road = Resources.Load("RoadNEW") as GameObject;
                            break;
                        case "NSW":
                            road = Resources.Load("RoadNSW") as GameObject;
                            break;
                        case "NESW":
                            road = Resources.Load("RoadNESW") as GameObject;
                            break;
                        default:
                            break;
                    }
                    if(road != null)
                        arena[i][j] = (GameObject)Instantiate(road, new Vector3(i * 10 - 70, 0, j * 10 - 45), road.transform.rotation);
                }
            }
        }

        //int road_type = rnd.Next(0, 3);
        //GameObject road;
        //switch (road_type)
        //{
        //    case 0:
        //        road = Resources.Load("Road") as GameObject;
        //        break;
        //    case 1:
        //        road = Resources.Load("RoadBenchMailbox") as GameObject;
        //        break;
        //    case 2:
        //        road = Resources.Load("RoadLamp1") as GameObject;
        //        break;
        //    default:
        //        road = Resources.Load("Road") as GameObject;
        //        break;
        //}

        //arena[newX][newY] = new GameObject(); // (GameObject)Instantiate(road, new Vector3(newX * 10 - 70, .5f, newY * 10 - 45), Quaternion.identity);
        //if (road_type == 2)
        //    lampPosts[roads] = arena[newX][newY];
        //Debug.Log("Placing road at " + newX + " " + newY);
       
    }

    String GetDirections(int x, int y)
    {
        String directions = "";
        if( y < grid_y-1 && arena[x][y+1] != null) 
        {
            directions += "N";
        }
        if( x < grid_x-1 && arena[x+1][y] != null)
        {
            directions += "E";
        }
        if(y > 0 && arena[x][y-1] != null)
        {
            directions += "S";
        }
        if(x > 0 && arena[x-1][y] != null)
        {
            directions += "W";
        }

        return directions;
    }

}
