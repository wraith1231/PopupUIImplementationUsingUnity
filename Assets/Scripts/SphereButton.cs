using UnityEngine;

public class SphereButton : MonoBehaviour
{
    [SerializeField]
    private ShopSphere _shopSphere;

    private Transform _camera;
    public GameObject _text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _text.SetActive(false);
        _camera = Camera.main.transform;    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(transform.position + _camera.rotation * Vector3.forward, _camera.rotation * Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        _text.SetActive(true);

        other.gameObject.GetComponent<Player>().SetButton(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        _text.SetActive(false);

        other.gameObject.GetComponent<Player>().SetButton(null);
    }

    public void OpenPopup()
    {
        if (_shopSphere != null)
        {
            _shopSphere.OpenUI();
        }
    }
}
