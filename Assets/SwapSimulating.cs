using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// placed on toggle to change state on spacebar
/// </summary>
public class SwapSimulating : MonoBehaviour
{

    Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            toggle.isOn = !toggle.isOn;
        }
    }
}
