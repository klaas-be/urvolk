using UnityEngine;
using System.Collections;

public class Farm : MonoBehaviour {
  public float foodIncome = 1;

  public void Increase(int val) {
      foodIncome += val;
      return;
  }
}
