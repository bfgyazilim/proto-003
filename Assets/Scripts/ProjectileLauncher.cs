using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawnLocation;
    public float launchPower;

    public void Launch()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnLocation.position, projectileSpawnLocation.rotation);
        Rigidbody projectileRigidBody = projectile.GetComponent<Rigidbody>();
        projectileRigidBody.AddForce(projectileSpawnLocation.up * launchPower);
    }

}
