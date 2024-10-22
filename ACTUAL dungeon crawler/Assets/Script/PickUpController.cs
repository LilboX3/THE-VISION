using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField, Restrict(typeof(IPickupCallback))]
    private UnityEngine.Object PickupCallbackField;

    private IPickupCallback _enterCallback => PickupCallbackField as IPickupCallback;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerComponent = other.gameObject.GetComponent<PlayerController>();
            if (_enterCallback != null)
            {
                Debug.Log("callingBack");
                _enterCallback.PickupCallback(playerComponent);
                Destroy(gameObject);
            }
        }
    }
}
