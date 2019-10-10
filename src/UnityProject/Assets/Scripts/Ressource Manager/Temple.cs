using UnityEngine;
using System.Collections;

public class Temple : MonoBehaviour {

    public float faithIncome = 1;

    public void Increase(int val) {
        faithIncome += val;
        return;
    }
	}

