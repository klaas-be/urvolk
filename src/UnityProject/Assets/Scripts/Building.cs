using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

    public int IncreaseMultiplyer;

    public void increaseIncome(int val) {
         if (GetComponent<Farm>() != null) {
            Farm temp = GetComponent<Farm>();
            temp.Increase(val*IncreaseMultiplyer);
        } else if (GetComponent<Lumberjack>() != null) {
            Lumberjack temp = GetComponent<Lumberjack>();
            temp.Increase(val * IncreaseMultiplyer);
        } else if (GetComponent<Temple>() != null) {
            Temple temp = GetComponent<Temple>();
            temp.Increase(val * IncreaseMultiplyer);
         }
    }

}
