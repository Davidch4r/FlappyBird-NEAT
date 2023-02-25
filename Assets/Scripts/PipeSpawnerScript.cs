using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnerScript : MonoBehaviour
{
    public float maxTime = 1;
    float set;
    float timer = 0;
    public GameObject Pipe;
    public float minHeight;
    public float maxHeight;
    public bool start;
    void Start() {
        set = maxTime;
        start = false;
    }
    public void Reset() {
        maxTime = set;
        foreach(var pipe in GameObject.FindGameObjectsWithTag("Pipe")) {
            Destroy(pipe);
        }
    }

    public void Increase(float increase) {
        maxTime += increase;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (start) {
            if (timer > maxTime) {
                GameObject pipe = Instantiate(Pipe);
                pipe.transform.position = transform.position + new Vector3(0, Random.Range(minHeight, maxHeight), 0);
                Destroy(pipe, 15);
                timer = 0;
            }

            timer += Time.deltaTime;
        }
    }
}
