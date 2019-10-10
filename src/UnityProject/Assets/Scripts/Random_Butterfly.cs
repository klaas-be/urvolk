using UnityEngine;
using System.Collections;

public class Random_Butterfly : MonoBehaviour {

    bool skipframe = false;
    bool flipped = false;
    Vector3 lastpos;
    int waitframe = 0;
    void Start() {
        lastpos = gameObject.transform.position;
    }

    void Update() {
        if (!skipframe) {
            Vector3 vec = new Vector3(gameObject.transform.position.x + Random.Range(-1f, 1f), gameObject.transform.position.y + Random.Range(-1f, 1f), 0.5f);
            while ((vec - gameObject.transform.position).magnitude > 0.5f) {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, vec, 0.1f);
            }
            skipframe = true;
        } else {
            if (waitframe > 1) {
                skipframe = false;
                waitframe = 0;
            } else {
                waitframe++;
            }
        }/*
        if ((lastpos - gameObject.transform.position).magnitude < 0 && !flipped) {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            flipped = true;
        } else if ((lastpos - gameObject.transform.position).magnitude > 0 && flipped) {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            flipped = false;
        }
        lastpos = gameObject.transform.position;
          */
    }
}
