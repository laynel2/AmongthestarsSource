using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReEditNo : MonoBehaviour
{
    bool canEdit = true;
    public InputField inf;
    // Start is called before the first frame update
    public void noEdit()
    {
        canEdit = false;
    }

    private void Start()
    {
        canEdit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canEdit)
        {
            inf.readOnly = true;
        }
    }
}
