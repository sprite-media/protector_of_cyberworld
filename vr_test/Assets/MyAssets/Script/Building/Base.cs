﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : PlayerBuilding
{
	public Base BASE { get { return GameObject.Find("Base").GetComponent<Base>(); } }
	private int numEnemiesLeft = 0;

    //UI text for win
    //UI text for lose
    //UI for hp
    //UI for resource
    //UI for numEnemiesLeft

    /*  Jin, i needeed to make singleton class casue
        i can't make Coroutin function in static function,
        but i can in static class -Hyukin.*/
    #region Singleton
    private static Base instance;
    public static Base Instance
    {
        get { return instance; }
    }
    private void OnDestroy()
    {
        instance = null;
    }
    #endregion

    private void Awake()
	{
        instance = this;

        if (hp != 0)// only 1 base can exist in hierarchy
		{
			Destroy(gameObject);
			return;
		}
		else
		{
			hp = 50;// temp value
			gameObject.name = "Base";
		}
	}
	private void Win()
	{
		//Display Win text
		Debug.Log("Win");
	}
	public override void Death()
	{
        // Display Lose text
        base.Death();
		Debug.Log("Lose");
	}

	// Called when enemy is attacking
	public override void TakeDamage(float amt)
	{
        if (hp <= 0) return;
        base.TakeDamage(amt);
		Debug.Log("Base left hp is: " + hp);
		//Update Health bar
	}

	public void SetTotalNumEnemy(int num)
	{
		numEnemiesLeft = num;
	}
    
    public int GetTotalNumEnemy()
    {
        return numEnemiesLeft; 
    }
    
    public void DereaseTheEnemyNum()
    {
        numEnemiesLeft--;
    }

	// Called when an enemy is dead
	public void ReduceNumEnemy()
	{
		numEnemiesLeft--;
		Debug.Log("Number of Enenmy Left:" + numEnemiesLeft);
		//Update indicator
		if (numEnemiesLeft == 0)
		{
			GameObject.Find("Base").GetComponent<Base>().Win();
		}
        else if(numEnemiesLeft == 1)
        {
            StartCoroutine(SpawnBossToStage());
        }
	}

    IEnumerator SpawnBossToStage() 
    {
        yield return new WaitForSeconds(2.0f);
        Boss.Instance.gameObject.SetActive(true);
    }
}
