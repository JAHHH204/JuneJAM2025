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
        if (player.maxObjects < 1)
        {
            player.isCreating = false;

            // Destroy the previous platform if it exists
            if (player.lastSpawnedPlatform != null)
            {
                GameObject.Destroy(player.lastSpawnedPlatform);
            }

            // Spawn a new platform
            Vector3 spawnPos = player.transform.position + player.transform.forward * 2f;
            GameObject spawnedPlatform = GameObject.Instantiate(player.buildingObject, spawnPos, player.transform.rotation);
            Debug.Log("Building at " + spawnPos);

            // Store reference to the new platform
            player.lastSpawnedPlatform = spawnedPlatform;

            // Optional timer component
            // PlatformTimer timer = spawnedPlatform.GetComponent<PlatformTimer>();
            // if (timer != null)
            // {
            //     timer.SetOwner(player); 
            // }

            // player.maxObjects++;  // You can use this logic if you limit to 1 at a time

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