using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftShoulderMove : MonoBehaviour
{
    public float x = 0.0f;
    public float movementVolicity = 5.0f;
    public GameObject elbow;
    public LeftElbowMove leftElbowMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        elbow = GameObject.Find("Left Elbow Joint");
        leftElbowMovement = elbow.GetComponent<LeftElbowMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow)) {
            x += Time.deltaTime * 35;
            if (x < 190.0f) {
                transform.rotation = Quaternion.Euler(x, 0, 0);
                leftElbowMovement.parentCallback(x);
            } else {
                x = 190.0f;
            }
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            x -= Time.deltaTime * 35;
            if (x > -40.0f) {
                transform.rotation = Quaternion.Euler(x, 0, 0);
                leftElbowMovement.parentCallback(x);
            } else {
                x = -40.0f;
            }
        }
    }
}
