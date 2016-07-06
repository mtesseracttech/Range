using UnityEngine;
using System.Collections;

public class AmmoScript : MonoBehaviour {
    public static int amountOfBullets = 6;
	// Use this for initialization
    public int GetAmountOfBullets()
    {
        return amountOfBullets;
    }
}
