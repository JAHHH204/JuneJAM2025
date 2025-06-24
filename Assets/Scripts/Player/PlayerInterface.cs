using UnityEngine;

public interface PlayerInterface
{
    void EnterState(PlayerController player);
    void UpdateState(PlayerController player);
    void ExitState(PlayerController player);

}
