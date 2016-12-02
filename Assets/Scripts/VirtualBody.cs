using UnityEngine;
using Zenject;

public class VirtualBody : MonoBehaviour
{
    private Quaternion _originalRotation;
    [Inject]
    private ScoreManager _scoreManage;

    void Awake()
    {
        _originalRotation = transform.rotation;
    }

	void LateUpdate ()
	{
	    transform.rotation = _originalRotation;
	}

    void OnTriggerEnter(Collider triggerCollider)
    {
        var projectile = triggerCollider.gameObject.GetComponent<ProjectileFromShootbackTarget>();
        if (projectile != null)
        {
            _scoreManage.TimeLeft -= 10;
            Destroy(projectile.gameObject);
        }
    }
}
