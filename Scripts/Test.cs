using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Time.time);
        Game2D.Interface.GetSystem<ITimeSystem>()
            .AddDelayTask(3, () =>
            {
                  Debug.Log(Time.time);
            });
    }
}
