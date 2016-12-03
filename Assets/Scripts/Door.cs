using UnityEngine;

public class Door : MonoBehaviour
{
    public LevelGenerator.ElevatorDirection Direction;
    private Animator _animator;
    public float AnimationTime;
    public float OpeningTimeOffset;
    public float ClosingTimeOffset;
    public AudioClip OpenClip;
    public AudioClip CloseClip;

    private bool _open;
    private bool _opening;
    private bool _closing;
    private float _timeElapsed;
    private AudioSource _audioSource;

    public bool Open
    {
        get { return _open; }
        set
        {
            _open = value;
            if (_open)
            {
                if (OpenClip != null)
                {
                    _audioSource.PlayOneShot(OpenClip);
                }
            }
            else
            {
                if (CloseClip != null)
                {
                    _audioSource.PlayOneShot(CloseClip);
                }
            }
            _animator.SetBool("DoorOpen", _open);
        }
    }

	void Awake ()
	{
	    _animator = GetComponent<Animator>();
	    _audioSource = GetComponent<AudioSource>();
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
