using UnityEngine;

public class testAnimationPery : MonoBehaviour
{
    [SerializeField] private GameObject root, model;

    // Update is called once per frame
    void Update()
    {
        model.transform.position = root.transform.position;
        model.transform.rotation = root.transform.rotation;
    }
}
