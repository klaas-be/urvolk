using UnityEngine;
using System.Collections;
public class MoveSnake : MonoBehaviour {
    public Transform startPosition;
    private Transform lastTransform;
    public GameObject pathFinder, target;
    private Grid grid;
    public float snakeMoveSpeed = 0.5f;
    // Use this for initialization
    void Start() {
        gameObject.transform.position = startPosition.position;
        if (pathFinder != null) {
            if (pathFinder.GetComponent<Grid>() != null) {
                grid = pathFinder.GetComponent<Grid>();
            }
        }
        GameObject helper = new GameObject();
        helper.transform.position = startPosition.position;
        lastTransform = helper.transform;
        lastTransform.position = startPosition.position;
    }

    // Update is called once per frame
    void Update() {

    }
    public void StartMoving() {
        StartCoroutine(Example());
    }
    public void MoveToNext() {
        if (grid.path == null || grid.path.Count == 0) {
            return;
        }
        //if (gameObject.GetComponent<BoxCollider2D> ()) {}
        for (int i = 0; i < grid.path.Count; i++) {
            gameObject.transform.position = grid.path[i].worldPosition;


            lastTransform.position = gameObject.transform.position;

        }
    }
    IEnumerator Example() {

        if (grid.path == null || grid.path.Count == 0) {
            yield return null;
        }
        //if (gameObject.GetComponent<BoxCollider2D> ()) {}
        for (int i = 0; i < grid.path.Count; i++) {
            gameObject.transform.position = grid.path[i].worldPosition;


            lastTransform.position = gameObject.transform.position;

            yield return new WaitForSeconds(snakeMoveSpeed);
        }
    }
    void OnTriggerEnter(Collider other) {
    }
}