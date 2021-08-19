using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField]GameObject paimonTest;
    int moveSpeed = 4;
    private void FixedUpdate()
    {
        /*
        if (Input.GetKey(KeyCode.D))
        {
            paimonTest.transform.Translate(new Vector3(1, 0, 0) * moveSpeed *Time.deltaTime);
        }else if (Input.GetKey(KeyCode.A))
        {
            paimonTest.transform.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime);
        }else if (Input.GetKey(KeyCode.W))
        {
            var paimonRig = paimonTest.GetComponent<Rigidbody2D>();
            paimonRig.GetComponent<Rigidbody2D>().velocity = new Vector3(paimonRig.velocity.x, 350 * Time.deltaTime);
        }
        */
    }
}
