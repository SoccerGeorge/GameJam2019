using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;
using GameFramework;
using GameFramework.Controls;


public class DrunkPlayer : Player
{
    #region Private Declarations

    [SerializeField]
    [Tooltip("Animator for the player")]
    private Animator _animator = null;

    private const string SPEED_ANIM = "Speed";

    private float _speed = 0f;
    private float _turn = 0f;
    private float _drunkennessSpeed = 0f;
    private float _drunkennessTurn = 0f;
    private Transform _target;
    private bool _canMove = false;
    private int _score = 60;

    private Text _scoreText;
    private Text _distance;

    #endregion

    #region Protected Declarations


    #endregion

    #region Public Declarations

    [Tooltip("Time when drunkenness force changes in seconds")]
    public int DrunkennessTime = 20;

    public DrunkControls DrunkenControls {
        get { return Controls as DrunkControls; }
        private set { Controls = value; }
    }

    #endregion

    #region MonoBehaviour

    // Start is called before the first frame update
    protected override void Start () {
        DrunkenControls = new DrunkControls(PlayerId);
        _turn = transform.rotation.eulerAngles.y;
        _drunkennessTurn = transform.rotation.eulerAngles.y;
        InvokeRepeating("NewDrunkenForce", 6f, DrunkennessTime);

        Transform overlay = UIManager.Instance.GetActiveMenu().transform.GetChild(0).GetChild(0);
        _scoreText = overlay.GetChild(0).GetComponent<Text>();
        _distance = overlay.GetChild(1).GetComponent<Text>();
    }

    private void Update () {
        if (Input.GetKey(DrunkenControls.PauseKeyCode)) {
            UIManager.Instance.SwitchToMenuByIndex(4);
            GameManager.Instance.Pause();
        }
    }

    // Update is called once per frame
    private void LateUpdate () {
        if (_animator.enabled && _canMove) {
            Move();

            _animator.SetFloat(SPEED_ANIM, _speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(_turn, Vector3.up), Time.deltaTime * 5f);
            if (_speed > 0.1f || _speed < -0.1) {
                _speed = Mathf.Lerp(_speed, 0f, Time.deltaTime);
            }
            else {
                _speed = 0f;
            }

        }

        _scoreText.text = _score.ToString();
        _distance.text = "{0} meters from home".FormatStr(Vector3.Distance(transform.position, _target.position).ToString("F3"));
    }

    #endregion

    #region Private Methods

    private void AddDrunkennessForce () {
        _speed = Mathf.Lerp(_speed, _drunkennessSpeed, Time.deltaTime);
        _turn = Mathf.Lerp(_turn, _drunkennessTurn, Time.deltaTime);
    }

    private void NewDrunkenForce () {
        _drunkennessSpeed = Random.Range(-0.2f, 0.2f);
        _drunkennessTurn = Random.Range(-5f, 5f);
        DrunkenControls.SwapControls();
        _canMove = true;
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
                _turn -= 10f;
            }
            else {
                _turn += 10f;
            }
        }
    }

    #endregion

    #region Public Methods

    public void Win () {
        UIManager.Instance.SwitchToMenuByIndex(5);
    }

    public void HighVal () {
        UIManager.Instance.SwitchToMenuByIndex(5);

    }

    public void MidVal () {
        UIManager.Instance.SwitchToMenuByIndex(5);
    }

    public void LowVal () {
        UIManager.Instance.SwitchToMenuByIndex(5);
    }

    public void Lose () {
        UIManager.Instance.SwitchToMenuByIndex(6);
    }

    public void SetTarget (Transform target) {
        _target = target;
    }

    public void EnableAnimator () {
        _animator.enabled = true;
    }

    public void DisableAnimator () {
        _animator.enabled = false;
    }

    public void CheckCollisionEnter (Collision collision) {
        int numContacts = collision.contactCount;
        ContactPoint[] contactPoints = new ContactPoint[numContacts];
        collision.GetContacts(contactPoints);
        foreach (ContactPoint contactPoint in contactPoints) {
            switch (contactPoint.otherCollider.tag) {
                case "Ignore":
                    break;
                case "Win":
                    _score += 50;
                    Invoke("Win", 5f);
                    DisableAnimator();
                    break;
                case "HighVal":
                    _score += 100;
                    Invoke("HighVal", 5f);
                    DisableAnimator();
                    break;
                case "MidVal":
                    _score += 25;
                    Invoke("MidVal", 5f);
                    DisableAnimator();
                    break;
                case "LowVal":
                    _score += 10;
                    Invoke("LowVal", 5f);
                    DisableAnimator();
                    break;
                case "Lose":
                    Invoke("Lose", 5f);
                    DisableAnimator();
                    break;
                default:
                    _score -= 2;
                    _animator.SetFloat(SPEED_ANIM, 0f);
                    transform.position += contactPoint.normal * 0.1f;
                    _animator.SetFloat(SPEED_ANIM, _speed);
                    break;
            }
        }
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

        if (++_swapCount == 3) {
            _swapCount = 0;

            swappedControls[0] = _controls[1];
            swappedControls[1] = _controls[0];
            swappedControls[2] = _controls[3];
            swappedControls[3] = _controls[2];
        }

        left = swappedControls[0];
        right = swappedControls[1];
        forward = swappedControls[2];
        back = swappedControls[3];

        UpdateStringToKeyCodeDic();
    }

    #endregion
}
