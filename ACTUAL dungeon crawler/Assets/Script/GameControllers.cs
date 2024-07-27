using System.Collections;
using UnityEngine;

public class GameControllers : MonoBehaviour
{
    public static bool connected = false;

    IEnumerator CheckForControllers()
    {
        //endless loop checking for controller i guess (eh gut?)
        while (true)
        {
            var controllers = Input.GetJoystickNames();
            bool hasValidController = false;

            foreach (var controller in controllers)
            {
                if (!string.IsNullOrEmpty(controller))
                {
                    hasValidController = true;
                }
            }

            if (!connected && hasValidController)
            {
                connected = true;
                Debug.Log("Connected");

            }
            else if (connected && !hasValidController)
            {
                connected = false;
                Debug.Log("Disconnected");
            }

            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Awake()
    {
        StartCoroutine(CheckForControllers());
    }
}
