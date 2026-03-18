using UnityEngine;

[RequireComponent(typeof(Collider))]
public class JumpPad : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float launchForce = 20f;
    [SerializeField] private bool resetJumpCount = true;

    [Header("Optional Forward Boost")]
    [SerializeField] private bool useForwardBoost = false;
    [SerializeField] private float forwardBoostForce = 5f;
    [SerializeField] private bool usePadForwardDirection = true;

    [Header("Detection")]
    [SerializeField] private string playerTag = "Jammo";

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        PlayerStateMachine player = other.GetComponentInParent<PlayerStateMachine>();
        if (player == null)
            return;

        LaunchPlayer(player, other.transform);
    }

    private void LaunchPlayer(PlayerStateMachine player, Transform playerTransform)
    {
        // Give the player strong upward movement
        player.CurrentMovementY = launchForce;
        player.AppliedMovementY = launchForce;

        // Optional: reset jump count so the player can still double/triple jump after launch
        if (resetJumpCount)
        {
            player.JumpCount = 0;
            player.RequireNewJumpPress = true;
        }

        // Optional forward boost
        if (useForwardBoost)
        {
            Vector3 launchDirection = usePadForwardDirection ? transform.forward : playerTransform.forward;
            launchDirection.y = 0f;
            launchDirection.Normalize();

            player.AppliedMovementX = launchDirection.x * forwardBoostForce;
            player.AppliedMovementZ = launchDirection.z * forwardBoostForce;
        }
    }
}