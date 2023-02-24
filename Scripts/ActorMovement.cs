using UnityEngine;

public class ActorMovement : MonoBehaviour
{
    [SerializeField] private ActorType actorType;

    public enum ActorType
    {
        PLAYER,
        GHOST
    }

    private GhostController ghostController;
    private int currentReplayIndex;



    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public Transform charModel;
    public Transform CharModel => charModel;
    Vector3 moveDirection;
    private Vector3 horizontalMoveDirection;
    public Vector3 HorizontalMoveDirection => horizontalMoveDirection;

    private bool hasPlayedTheFullMemory;

    Rigidbody rb;
    float horizontalInput;
    float verticalInput;
    public float HorizontalInput => horizontalInput;
    public float VerticalInput => verticalInput;
    private bool isMoving;
    public bool IsMoving => isMoving;

    [Header("Animations")]
    [SerializeField] private Animator anim;

    [Header("Ground Check")]
    public float actorHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlatforms;


    [Header("TerrainType Check")]
    [SerializeField] private LayerMask whatIsGrass;
    [SerializeField] private LayerMask whatIsRocks;
    [SerializeField] private LayerMask whatIsWood;



    [Header("Rotation")]
    public float rotationSpeed;

    [Header("Slipping Off Surfaces")]
    [SerializeField] private float slipSpeed;
    [SerializeField] private float startPointOfSlipRays;



    [Header("Jumping")]
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    [SerializeField] private float airDrag;
    bool canJump = true;
    private bool isJumping = false;
    public bool IsJumping => isJumping;



    private void OnEnable()
    {
        Actions.OnMemorizeDeactivated += DeactivatePlayerIsJumping;
    }

    private void OnDisable()
    {
        Actions.OnMemorizeDeactivated -= DeactivatePlayerIsJumping;
    }



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (actorType == ActorType.GHOST)
        {
            ghostController = GetComponent<GhostController>();
        }
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        if (horizontalInput >= 0.02f || verticalInput >= 0.02f || verticalInput < 0f || horizontalInput < 0f) //MOVEMENT INPUT WASD
        {
            isMoving = true;

            if (IsGrounded()) //ON THE GROUND
            {
                anim.SetBool("isGrounded", true);
                anim.SetBool("isFalling", false);


                
                if(!(actorType == ActorType.GHOST && hasPlayedTheFullMemory)) //if it is not a ghost that has to idle on the last memorized frame
                {
                    anim.SetBool("isIdling", false);
                }


                //////////////GHOST STUFF///////////
                if (actorType == ActorType.GHOST)
                {
                    if (!hasPlayedTheFullMemory)
                    {
                        anim.SetBool("isRunning", true);
                    }
                }
                else
                {
                    anim.SetBool("isRunning", true);
                }
                ////////////////////////////////////
            }
            else //IN THE AIR
            {
                anim.SetBool("isIdling", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isGrounded", false);

                if (IsFalling()) //if vertical vel. < 0
                {
                    anim.SetBool("isIdling", false);
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isFalling", true);
                }
            }

        }
        else  //NO MOVEMENT INPUT
        {
            isMoving = false;
            if (IsGrounded()) //ON THE GROUND
            {
                anim.SetBool("isGrounded", true);

                anim.SetBool("isFalling", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isIdling", true);
            }
            else //IN THE AIR
            {
                anim.SetBool("isIdling", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isGrounded", false);

                if (IsFalling()) //if vertical vel. < 0
                {
                    anim.SetBool("isIdling", false);
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isFalling", true);
                }
            }
        }

    }

    private void FixedUpdate()
    {
        if(actorType == ActorType.PLAYER) 
        {
            GetInput();
            if (!GameController.instance.hasWon)
            {
                Move();
            }


            var activeRecording = Recordings.instance.GetActiveRecording();
            if (activeRecording != null) //if we are recording
            {

                activeRecording.AddRecordData(
                                                    transform.position,
                                                    charModel.rotation,
                                                    horizontalInput,
                                                    verticalInput,
                                                    isJumping,
                                                    horizontalMoveDirection
                                                    );
            }
        }

        else if(actorType == ActorType.GHOST) 
        {
            //we want to go through all recorded frames
            int nextIndex = currentReplayIndex + 1;

            if (nextIndex < Recordings.instance.recordings[ghostController.GhostId].RecordedDatas.Count) //we don't want to go over the last recorded frame
            {
                GetFakeInputs(nextIndex);
                MoveGhost(nextIndex);
            }
            else
            {
                isMoving = false;
                hasPlayedTheFullMemory = true;
                anim.SetBool("isRunning", false);
                anim.SetBool("isIdling", true);
            }
        }






        if (!IsGrounded() && IsFalling())
        {
            MakeActorSlip();
        }


        if (moveDirection != Vector3.zero) //if we are moving
        {
            RotateSmoothly();
        }

        ApplyDrag();
        SpeedControl();

    }

    private bool IsFalling()
    {
        return rb.velocity.y < 0f;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        isJumping = Input.GetButton("Jump");



        if (isJumping && canJump && IsGrounded())
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }


    private void GetFakeInputs(int index)
    {
        currentReplayIndex = index;
        RecordedData recordData = Recordings.instance.recordings[ghostController.GhostId].RecordedDatas[index];

        charModel.rotation = recordData.modelRotation;
        horizontalInput = recordData.horInput;
        verticalInput = recordData.verInput;
        isJumping = recordData.isJumping;

        if (isJumping && canJump && IsGrounded())
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }


    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //reset velocity
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        anim.SetBool("isRunning", false);
        anim.SetBool("isIdling", false);
        anim.SetTrigger("goJump");
    }



    private void ResetJump()
    {
        canJump = true;
    }


    private void DeactivatePlayerIsJumping(int ghostId)
    {
        if(actorType == ActorType.PLAYER)
        {
            anim.SetTrigger("endMemorize");
        }
    }




    private void Move()
    {
        moveDirection = Camera.main.transform.forward * verticalInput + Camera.main.transform.right * horizontalInput; //so "forward" always means move in "camera forward" direction
        var right = Vector3.Cross(moveDirection, Vector3.up); //we use "up" of the world. Cross Product gives a vector, that is perpendicular to both
        horizontalMoveDirection = Vector3.Cross(Vector3.up, right); //this avoids the player "slowing down" when the camera looks down and the "jumping" when the camera looks up, because it is always parallel to the ground plane


        MoveInternal();
    }



    private void MoveGhost(int index)
    {
        currentReplayIndex = index;

        {
            RecordedData recordData = Recordings.instance.recordings[ghostController.GhostId].RecordedDatas[index];
            horizontalMoveDirection = recordData.horizontalMoveDirection;
        }

        MoveInternal();
    }


    private void MoveInternal()
    {
        var multiplier = IsGrounded() ? 1 : airMultiplier;
            rb.AddForce(horizontalMoveDirection.normalized * moveSpeed * 10f * multiplier, ForceMode.Force);
    }



    private void RotateSmoothly()
    {
        Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up); //our "target rotation". the rotation that looks in the desired direction
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        AdjustRotationError();
    }


    private void AdjustRotationError()
    {
        //to avoid the player beeing diagonal, we manually set the rotation to 0 in x and z components
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, eulerRotation.y, 0);
    }

    private void ApplyDrag()
    {
        if (IsGrounded())
            rb.drag = groundDrag;
        else
        {
            if (rb.velocity.y > 0)
            {
                rb.drag = airDrag;
            }
            else
            {
                rb.drag = airDrag/3;
            }
        }

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }


    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, actorHeight / 2f + 0.2f, whatIsGround); //we want to draw this green
    }

    private void MakeActorSlip()
    {
        RaycastHit hit;

        Ray frontRay = new Ray(transform.position + Vector3.down * (actorHeight / startPointOfSlipRays), transform.forward);
        Ray backRay = new Ray(transform.position + Vector3.down * (actorHeight / startPointOfSlipRays), -transform.forward);
        Ray rightRay = new Ray(transform.position + Vector3.down * (actorHeight / startPointOfSlipRays), transform.right);
        Ray leftRay = new Ray(transform.position + Vector3.down * (actorHeight / startPointOfSlipRays), -transform.right);

        if(Physics.Raycast(frontRay, out hit, 1f, whatIsGround) || Physics.Raycast(backRay, out hit, 1f, whatIsGround) || Physics.Raycast(rightRay, out hit, 1f, whatIsGround) || Physics.Raycast(leftRay, out hit, 1f, whatIsGround))
        {
            ApplySlipForce(hit.normal);
        }
    }

    private void ApplySlipForce(Vector3 slipDirection)
    {
        rb.AddForce(((slipDirection * slipSpeed) + Vector3.down) * Time.deltaTime);
    }




    public bool IsOnGrass()
    {
        return Physics.Raycast(transform.position, Vector3.down, actorHeight / 2f + 0.2f, whatIsGrass);
    }

    public bool IsOnRocks()
    {
        return Physics.Raycast(transform.position, Vector3.down, actorHeight / 2f + 0.2f, whatIsRocks);
    }
    public bool IsOnWood()
    {
        return Physics.Raycast(transform.position, Vector3.down, actorHeight / 2f + 0.2f, whatIsWood);
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * (actorHeight / 2f + 0.2f));

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.down * (actorHeight / startPointOfSlipRays), transform.forward);
        Gizmos.DrawRay(transform.position + Vector3.down * (actorHeight / startPointOfSlipRays), -transform.forward);
        Gizmos.DrawRay(transform.position + Vector3.down * (actorHeight / startPointOfSlipRays), transform.right);
        Gizmos.DrawRay(transform.position + Vector3.down * (actorHeight / startPointOfSlipRays), -transform.right);
    }

}
