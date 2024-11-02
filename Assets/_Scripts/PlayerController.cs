using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private CharacterController _player;

    [SerializeField] private float _movedSpeed, _gravity, _fallVelocity, _jumpForce;
    
    private Vector3 _axis, _movePlayer;

    private void Awake(){
        _player = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
        _axis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(_axis.magnitude > 1) _axis= transform.TransformDirection(_axis).normalized;
        else _axis= transform.TransformDirection(_axis);

        _movePlayer.x = _axis.x;
        _movePlayer.z = _axis.z;
        setGravity();

        _player.Move(_movePlayer * _movedSpeed * Time.deltaTime);
    }

    private void setGravity(){
        if(_player.isGrounded){

            _fallVelocity = -_gravity * Time.deltaTime;
            if(Input.GetKey(KeyCode.Space))
            {
                _fallVelocity = _jumpForce;
            }
        }
        else{
            _fallVelocity -= _gravity * Time.deltaTime;
        }
        _movePlayer.y = _fallVelocity;
    }
}
