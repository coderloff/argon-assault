using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public int wallPercentage;
    public int smoothingIterations;
    public int roomCount;
    public int minRoomSize;
    public int maxRoomSize;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject corridorPrefab;
    public GameObject obstaclePrefab;
    public GameObject lootPrefab;

    private int[,] map;
    private Room[] rooms;
    private Corridor[] corridors;

    private struct Room
    {
        public int x;
        public int y;
        public int width;
        public int height;
    }

    private struct Corridor
    {
        public int startX;
        public int startY;
        public int endX;
        public int endY;
    }

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        map = new int[width, height];
        rooms = new Room[roomCount];
        corridors = new Corridor[roomCount - 1];

        CreateRooms();
        CreateCorridors();
        SmoothMap();

        InstantiateMap();
    }

    void CreateRooms()
    {
        for (int i = 0; i < roomCount; i++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);
            int posX = Random.Range(1, width - roomWidth - 1);
            int posY = Random.Range(1, height - roomHeight - 1);

            Room newRoom = new Room();
            newRoom.x = posX;
            newRoom.y = posY;
            newRoom.width = roomWidth;
            newRoom.height = roomHeight;

            bool isOverlap = false;
            for (int j = 0; j < i; j++)
            {
                if (CheckOverlap(newRoom, rooms[j]))
                {
                    isOverlap = true;
                    break;
                }
            }

            if (!isOverlap)
                rooms[i] = newRoom;
        }
    }

    bool CheckOverlap(Room room1, Room room2)
    {
        if (room1.x <= room2.x + room2.width && room1.x + room1.width >= room2.x &&
            room1.y <= room2.y + room2.height && room1.y + room1.height >= room2.y)
            return true;

        return false;
    }

    void CreateCorridors()
    {
        for (int i = 0; i < roomCount - 1; i++)
        {
            int startX = rooms[i].x + rooms[i].width / 2;
            int startY = rooms[i].y + rooms[i].height / 2;
            int endX = rooms[i + 1].x + rooms[i + 1].width / 2;
            int endY = rooms[i + 1].y + rooms[i + 1].height / 2;

            Corridor newCorridor = new Corridor();
            newCorridor.startX = startX;
            newCorridor.startY = startY;
            newCorridor.endX = endX;
            newCorridor.endY = endY;

            corridors[i] = newCorridor;
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighborWallCount = GetSurroundingWallCount(x, y);

                if (neighborWallCount > 4)
                    map[x, y] = 1;
                else if (neighborWallCount < 4)
                    map[x, y] = 0;
            }
        }
    }

    int GetSurroundingWallCount(int x, int y)
    {
        int wallCount = 0;
        for (int neighborX = x - 1; neighborX <= x + 1; neighborX++)
        {
            for (int neighborY = y - 1; neighborY <= y + 1; neighborY++)
            {
                if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                {
                    if (neighborX != x || neighborY != y)
                        wallCount += map[neighborX, neighborY];
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    void InstantiateMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity);
                }
                else
                {
                    Instantiate(floorPrefab, new Vector3(x, y, 0), Quaternion.identity);

                    // Place obstacles or loot randomly in rooms
                    foreach (Room room in rooms)
                    {
                        if (x > room.x && x < room.x + room.width && y > room.y && y < room.y + room.height)
                        {
                            if (Random.Range(0, 100) < 10) // Adjust the percentage as desired
                            {
                                Instantiate(obstaclePrefab, new Vector3(x, y, 0), Quaternion.identity);
                            }
                            else if (Random.Range(0, 100) < 5) // Adjust the percentage as desired
                            {
                                Instantiate(lootPrefab, new Vector3(x, y, 0), Quaternion.identity);
                            }
                        }
                    }
                }
            }
        }

        // Instantiate corridors
        foreach (Corridor corridor in corridors)
        {
            Vector3 start = new Vector3(corridor.startX, corridor.startY, 0);
            Vector3 end = new Vector3(corridor.endX, corridor.endY, 0);
            Instantiate(corridorPrefab, start, Quaternion.identity);
            Instantiate(corridorPrefab, end, Quaternion.identity);
        }
    }
}
 