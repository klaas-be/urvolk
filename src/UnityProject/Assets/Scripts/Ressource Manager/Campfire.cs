using UnityEngine;
using System.Collections;

public class Campfire : MonoBehaviour {

    public GameObject villagerPrefab;
    public Vector3 spawnPoint;

	public void createVillager()
    {
        Instantiate(villagerPrefab, spawnPoint, Quaternion.Euler(0, 0, 0));
        //FMODUnity.RuntimeManager.PlayOneShot("event:/SpawnSounds");
        
    }
}
