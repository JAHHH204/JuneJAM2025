using UnityEngine;
using UnityEngine.InputSystem;

public class BuilderState : PlayerInterface
{

    private float jumpForce = 12f;
    private float gravity = -9.81f;
    private float verticalVelocity = 0f;
    float airControlMult = 0.4f;
    public void EnterState(PlayerController player)
    {
        player.SetAnims("Create");
    }

    public void UpdateState(PlayerController player)
    {

        //if (player.characterController.isGrounded && verticalVelocity < 1)
        //{
        //    verticalVelocity = 0f;
        //    player.StateTransition(new IdleState());
        //}

        if (player.maxObjects < 1)
        {
            player.isCreating = false;

            // Destroy the previous platform if it exists
            if (player.lastSpawnedPlatform != null)
            {
                GameObject.Destroy(player.lastSpawnedPlatform);
            }

            // Spawn a new platform
            Vector3 spawnPos = player.transform.position + player.transform.forward * 2f+player.transform.right+ player.grabPoint.transform.up;
            GameObject spawnedPlatform = GameObject.Instantiate(player.buildingObject, spawnPos, player.grabPoint.transform.rotation);
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