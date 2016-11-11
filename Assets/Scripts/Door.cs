using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator;

    private bool _open;

    public bool Open
    {
        get { return _open; }
        set
        {
            _open = value;
            _animator.SetBool("DoorOpen", _open);
        }
    }

	void Awake ()
	{
	    _animator = GetComponent<Animator>();
	}

    void Start()
    {
        Open = true;
    }

    public void ToggleAnimation()
    {
        Open = !Open;
    }
}
