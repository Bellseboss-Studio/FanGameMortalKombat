using UnityEngine;

public interface IFatality
{
    GameObject GetEnemyToKillWithFatality();
    bool ReadInput(out INPUTS input);
    void StartToReadInputs(bool b);
}