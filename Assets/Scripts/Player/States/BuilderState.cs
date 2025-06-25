using UnityEngine;
using UnityEngine.InputSystem;

public class BuilderState : PlayerInterface
{


    public void EnterState(PlayerController player)
    {
        player.SetAnims("Create");
    }

    public void UpdateState(PlayerController player)
    {
        if (player.maxObjects < 3)
        {
            player.isCreating = false;
            Vector3 spawnPos = player.transform.position + player.transform.right * 2f;
            GameObject spawnedPlatform = GameObject.Instantiate(player.buildingObject, spawnPos, player.transform.rotation);
            Debug.Log("Building at " + spawnPos);
            PlatformTimer timer = spawnedPlatform.GetComponent<PlatformTimer>();
            if (timer != null)
            {
                timer.SetOwner(player); 
            }
            player.maxObjects++;
            player.StateTransition(new IdleState());
        }
        else
        {
            Debug.Log("TooManyObjects...GetRekt");
            player.StateTransition(new IdleState());
        }
    }


    public void ExitState(PlayerController player)
    {
        player.SetAnims("Idle");
    
    }
}