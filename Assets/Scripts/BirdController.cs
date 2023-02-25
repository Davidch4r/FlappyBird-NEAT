using System.Collections;
using System.Collections.Generic;
using SharpNeat.Phenomes;
using UnityEngine;
using UnitySharpNEAT;

public class BirdController : UnitController
{
    float fitness;
    public bool death;
    bool jump;
    float velocity;
    float[] inputs = new float[2];
    Rigidbody2D rb;

    private Vector3 _initialPosition = default;
    private Quaternion _initialRotation = default;

    public override float GetFitness()
    {
        return fitness;
    }

    protected override void HandleIsActiveChanged(bool newIsActive)
    {
        if (!newIsActive)
            SetObj();

    }

    protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray)
    {
        if (!death) {
            FeedForwardVals();

            inputSignalArray[0] = inputs[0];
            inputSignalArray[1] = inputs[1];
        }
    }

    protected override void UseBlackBoxOutpts(ISignalArray outputSignalArray)
    {
        if (!death && outputSignalArray[0] >= 0.5f)
            jump = true;
    }

    
    void Start()
    {
        velocity = 1.4f;
        rb =  GetComponent<Rigidbody2D>();
        _initialPosition = new Vector2(0, 0);
        _initialRotation = transform.rotation;
    }

    void SetObj() {
        fitness = 0;
        death = false;
        jump = false;
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;

    }

    void FeedForwardVals() {
        Vector2 currentPipe = new Vector2(0, 0);
        bool found = false;
        foreach (var pipe in GameObject.FindGameObjectsWithTag("Clear")) {
            if (pipe.transform.position.x > -0.15f && pipe.transform.position.x < 1.5f) {
                found = true;
                currentPipe = pipe.transform.position;
                goto a;
            }
        }
        a: {}
        if (!found) {
            try {
                currentPipe = GameObject.FindGameObjectWithTag("Clear").transform.position;
            } catch (System.Exception)
            {
                currentPipe = new Vector2(0, 0);
            }
        }

        inputs[0] = (transform.position.x - currentPipe.x);
        inputs[1] = (transform.position.y - currentPipe.y);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        death = true;
    }

    protected override void Step()
    {
        if (!death) {
            fitness += Time.deltaTime;
            if (jump) {
                jump = false;
                rb.velocity = Vector2.up * velocity;
            }
            transform.position = new Vector2(transform.position.x, Mathf.Min(transform.position.y, 1.2f));
        } 
    }
}
