using UnityEngine;
using System.Collections;

public class Lumberjack : MonoBehaviour {

	public float woodIncome = 1;

    public void Increase(int val) {
        woodIncome += val;
        return;
    }
}
