using UnityEngine;

public class DestinationRangeGizmo : MonoBehaviour
{
    private GameObject rangeIndicator;
    private SphereCollider sphereCollider;
    [SerializeField] private Material rangeIndicatorMaterial;

    private void Awake()
    {
        sphereCollider = GetComponentInParent<SphereCollider>();
    }

    // private void Start()
    // {
    //     rangeIndicator = CreateRangeIndicator(sphereCollider.radius);
    // }

    // private void FixedUpdate()
    // {
    //     rangeIndicator.transform.position = new Vector3(transform.position.x, 0.21f, transform.position.z);
    // }

    private void OnDisable()
    {
        rangeIndicator.SetActive(false);
    }

    private void OnEnable()
    {
        if(rangeIndicator == null)
        {
            rangeIndicator = CreateRangeIndicator(sphereCollider.radius);
        }
        
        rangeIndicator.SetActive(true);
    }

    private GameObject CreateRangeIndicator(float range)
    {
        GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        // indicator.transform.SetParent(transform);
        Destroy(indicator.GetComponent<Collider>());
        indicator.transform.localScale = new Vector3(range * 2, 0.01f, range * 2);
        indicator.GetComponent<Renderer>().material = rangeIndicatorMaterial;
        indicator.transform.position = new Vector3(transform.position.x, 0.21f, transform.position.z); 
        return indicator;
    }
}
