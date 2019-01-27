using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using GameFramework;
using GameFramework.Controls;


public class DrunkPlayer : Player
{
    #region Private Declarations

    [SerializeField][Tooltip("Animator for the player")]
    private Animator _animator;

    private const string SPEED_ANIM = "Speed";

    private float _speed = 0f;
    private float _turn = 0f;
    private float _drunkennessSpeed = 0f;
    private float _drunkennessTurn = 0f;

    #endregion

    #region Protected Declarations


    #endregion

    #region Public Declarations

    [Tooltip("Time when drunkenness force changes in seconds")]
    public int DrunkennessTime = 120;

    public DrunkControls DrunkenControls {
        get { return Controls as DrunkControls; }
        private set { Controls = value; }
    }

    #endregion

    #region MonoBehaviour

    // Start is called before the first frame update
    protected override void Start () {
        DrunkenControls = new DrunkControls(PlayerId);
        InvokeRepeating("NewDrunkenForce", 10f, DrunkennessTime);
    }

    // Update is called once per frame
    private void Update () {
        Move();

        _animator.SetFloat(SPEED_ANIM, _speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(_turn, Vector3.up), Time.deltaTime);
    }

    #endregion

    #region Private Methods

    private void AddDrunkennessForce () {
        _speed = Mathf.Lerp(_speed, _drunkennessSpeed, Time.deltaTime);
        _turn = Mathf.Lerp(_turn, _drunkennessTurn, Time.deltaTime);
    }

    private void NewDrunkenForce() {
        _drunkennessSpeed = Random.value;
        _drunkennessTurn = Random.Range(-135f, 135f);
        DrunkenControls.SwapControls();
    }

    #endregion

    #region Protected Methods

    protected override void Move () {
        AddDrunkennessForce();

        // Check if forward or back key is pressed
        if (Input.GetKey(DrunkenControls.ForwardKeyCode) || Input.GetKey(DrunkenControls.BackKeyCode)) {
            if (Input.GetKey(DrunkenControls.BackKeyCode)) {
                _speed -= 0.25f;
            }
            else {
                _speed += 0.25f;
            }
        }

        _speed = Mathf.Clamp(_speed, -1f, 1f);

        // Check if left or right key is pressed
        if (Input.GetKey(DrunkenControls.LeftKeyCode) || Input.GetKey(DrunkenControls.RightKeyCode)) {
            if (Input.GetKey(DrunkenControls.LeftKeyCode)) {
                _turn -= 5f;
            }
            else {
                _turn += 5f;
            }
        }
    }

    #endregion

    #region Public Methods

    public void DisableAnimator () {
        _animator.enabled = false;
    }

    #endregion
}


public class DrunkControls : KeyboardControls
{
    #region Private Declarations

    private readonly List<string> _controls = new List<string>() { "LeftArrow", "RightArrow", "UpArrow", "DownArrow" };
    private int _swapCount = 0;

    #endregion

    #region Public Methods

    public DrunkControls (int playerId) : base(playerId) { }

    public void SwapControls () {
        List<string> swappedControls = new List<string>(_controls);

        if (++_swapCount > 3) {
            _swapCount = 0;
        }

        switch (_swapCount) {
            case 1:
                swappedControls[0] = _controls[3];
                swappedControls[1] = _controls[2];
                swappedControls[2] = _controls[0];
                swappedControls[3] = _controls[1];
                break;
            case 2:
                swappedControls[0] = _controls[1];
                swappedControls[1] = _controls[0];
                swappedControls[2] = _controls[3];
                swappedControls[3] = _controls[2];
                break;
            case 3:
                swappedControls[0] = _controls[2];
                swappedControls[1] = _controls[3];
                swappedControls[2] = _controls[1];
                swappedControls[3] = _controls[0];
                break;
            default:
                break;
        }

        left = swappedControls[0];
        right = swappedControls[1];
        forward = swappedControls[2];
        back = swappedControls[3];

        UpdateStringToKeyCodeDic();
    }

    #endregion
}
