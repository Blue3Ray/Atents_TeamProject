using System;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    /// <summary>
    /// 연결되어 있는 방과 해당방의 방향 bool은 연결 여부
    /// </summary>
    public List<(Room, ExitDirection)> connectedRooms;
    public List<Room> alreadyConnectPassWayRooms;

    public bool isMainRoom = false;
    public bool isAccessibleMainRoom = false;

    public Vector2Int roomCoord;
    public Vector2Int gridCoord;

    // 방 생성시 사용할 변수들
    // 타일 맵---------------

    /// <summary>
    /// 모든 출구 정보들(위치, 방향, 연결 여부)
    /// </summary>
    public List<PassWay> exitPos;

    /// <summary>
    /// 레이어별로 타일 맵 위치
    /// </summary>
    public List<List<Vector3Int>> tilesPos;

    /// <summary>
    /// 그려질 맵의 최저점
    /// </summary>
    public Vector2Int origine;

    // 그리드 -----------------------------
    /// <summary>
    /// 축소 그리드 맵 상 위치
    /// </summary>
    public Vector2Int gridPos;


    public Room() { }

    public Room(Vector2Int coord)
    {
        roomCoord = coord;
        connectedRooms = new();
        alreadyConnectPassWayRooms = new();
    }

    public void SetAccessibleMainRoom()
    {
        isAccessibleMainRoom = true;            // 해당 방을 true로 설정

        for (int i = 0; i < connectedRooms.Count; i++)       // 연결되어 있는 방들에 대해서 검사
        {
            Room room = connectedRooms[i].Item1;
            if (!room.isAccessibleMainRoom)             // 연결되어 있는 방들 중 ture가 아닌 방이 있으면
            {
                room.SetAccessibleMainRoom();           // 해당 방과 연결되어 있는 방들도 메인룸과 연결 가능으로 바꿈
            }
        }
    }

    /// <summary>
    /// 이방이 target과 직접 연결(바로 옆) 되어 있는지 확인하는 함수
    /// </summary>
    /// <param name="targetRoom">확인할 방</param>
    /// <returns>참이면 연결 되어 있음, 거짓이면 연결 안되어 있음</returns>
    public bool IsConnected(Room targetRoom)
    {
        foreach ((Room, ExitDirection) room in connectedRooms)
        {
            if (room.Equals(targetRoom))
            {
                return true;
            }
        }
        return false;
    }

    public static void ConnectRooms(Room room1, Room room2)
    {
        // 연결할 때 둘 중 하나가 메인 룸과 연결이 되어 있으면 둘다 연결 설정
        if (room1.isAccessibleMainRoom)
        {
            room2.SetAccessibleMainRoom();
        }
        else if (room2.isAccessibleMainRoom)
        {
            room1.SetAccessibleMainRoom();
        }

        room1.SetExitDirAndAddConnectList(room2);
        room2.SetExitDirAndAddConnectList(room1);
    }

    void SetExitDirAndAddConnectList(Room targetRoom)
    {
        ExitDirection result = ExitDirection.None;

        // target이 현재 룸보다 위에 있을 때
        if (targetRoom.roomCoord.y > roomCoord.y)
        {
            // target이 현재 룸 기준으로 X축보다 Y축 쪽으로 올라가 있을 때
            if (targetRoom.roomCoord.y - roomCoord.y > Mathf.Abs(targetRoom.roomCoord.x - roomCoord.x))
            {
                result = ExitDirection.Up;
            }
            else
            {
                // target이 오른쪽에 있을 때
                if (targetRoom.roomCoord.x > roomCoord.x)
                {
                    result = ExitDirection.Right;
                }
                else
                {
                    result = ExitDirection.Left;
                }
            }
        }
        else
        {
            // target이 현재 룸 기준으로 X축보다 Y축 쪽으로 내려가 있을 때
            if (roomCoord.y - targetRoom.roomCoord.y > Mathf.Abs(targetRoom.roomCoord.x - roomCoord.x))
            {
                result = ExitDirection.Down;
            }
            else
            {
                // target이 오른쪽에 있을 때
                if (targetRoom.roomCoord.x > roomCoord.x)
                {
                    result = ExitDirection.Right;
                }
                else
                {
                    result = ExitDirection.Left;
                }
            }
        }

        if (result != ExitDirection.None)
        {
            connectedRooms.Add((targetRoom, result));
        }
        else
        {
            Debug.LogWarning("방 연결이 안됩니다!");
        }
    }

    // 방만들때 사용할 거

    public bool IsConnectedBuildRoom(Room targetRoom)
    {
        return alreadyConnectPassWayRooms.Contains(targetRoom);
    }
}
