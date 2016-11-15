using UnityEngine;

public class Door : MonoBehaviour
{
    public LevelGenerator.ElevatorDirection Direction;
    private Animator _animator;
    public float AnimationTime;
    public float OpeningTimeOffset;
    public float ClosingTimeOffset;

    private bool _open;
    private bool _opening;
    private bool _closing;
    private float _timeElapsed;

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

    }

    void Update()
    {
        if (!_opening && !_closing)
            return;
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed > ClosingTimeOffset && _closing)
        {
            _closing = false;
            Open = false;
        }
        if (_timeElapsed > OpeningTimeOffset && _opening)
        {
            _opening = false;
            Open = true;
        }
    }

    public void ToggleAnimation()
    {
        Open = !Open;
    }

    public void ScheduleOpen(bool skipClose)
    {
        _opening = true;
        _timeElapsed = skipClose ? AnimationTime : 0;
    }

    public void ScheduleClose()
    {
        _closing = true;
        _timeElapsed = 0;
    }
}
