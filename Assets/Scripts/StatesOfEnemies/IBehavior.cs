﻿using UnityEngine;

namespace StatesOfEnemies
{
    public interface IBehavior
    {
        void SetNextState(int nextStateFromState);
        void WalkToPoint(Vector3 toPoint);
        bool IsPlayerInRedZone();
        bool IsEnemyArrived(Vector3 concurrentPoint);
        int GetNextState();
        int GetRandom(int start, int end);
        Vector3 GetTarget();
        bool IsPlayerInYellowZone();
        void LookPlayer(Vector3 target);
        bool IsPlayerInGreenZone();
    }
}