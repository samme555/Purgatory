using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] GameObject powerUpSelectionUI;

    [SerializeField] GameObject powerUpPrefab;

    [SerializeField] Transform powerUpPositionOne;

    [SerializeField] Transform powerUpPositionTwo;

    [SerializeField] Transform powerUpPositionThree;

    [SerializeField] List<PowerUpSO> powerUpList;

    [SerializeField] GameObject powerUpOne, powerUpTwo, powerUpThree;

    List<PowerUpSO> alreadySelectedPowerUp = new List<PowerUpSO>();

    private void Start()
    {
        RandomizeNewPowerUps();
    }

    void RandomizeNewPowerUps() 
    {
        if (powerUpOne != null) Destroy(powerUpOne);
        if (powerUpTwo != null) Destroy(powerUpTwo);
        if (powerUpThree != null) Destroy(powerUpThree);
        
        List<PowerUpSO> randomizedPowerUps = new List<PowerUpSO>();

        List<PowerUpSO> availablePowerUps = new List<PowerUpSO>(powerUpList);
        
        while (randomizedPowerUps.Count < 3) 
        { 
            PowerUpSO randomPowerUp = availablePowerUps[Random.Range(0, availablePowerUps.Count)];
            if (!alreadySelectedPowerUp.Contains(randomPowerUp))
            { 
                randomizedPowerUps.Add(randomPowerUp);  
            }
        }

        powerUpOne = InstantiatePowerUp(randomizedPowerUps[0], powerUpPositionOne);
        powerUpTwo = InstantiatePowerUp(randomizedPowerUps[1], powerUpPositionTwo);
        powerUpThree = InstantiatePowerUp(randomizedPowerUps[2], powerUpPositionThree);

        GameObject InstantiatePowerUp(PowerUpSO powerUpSO, Transform position) 
        { 
            GameObject powerUpGO = Instantiate(powerUpPrefab, position.position, Quaternion.identity, position);
            PowerUp powerUp = powerUpGO.GetComponent<PowerUp>();
            powerUp.Setup(powerUpSO);
            return powerUpGO;
        }
    }

}
