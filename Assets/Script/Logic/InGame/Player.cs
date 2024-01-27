using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
	private bool m_Die = false;

	public float speed = 10.0f;

    [SerializeField]
    private Light m_Light = null;
    [SerializeField]
    private Transform m_Camera = null;
    [SerializeField]
    private Rigidbody m_Rigidbody = null;
	[SerializeField]
	private AudioSource m_AudioSource;

	public float MinYaw = -360;
	public float MaxYaw = 360;
	public float MinPitch = -60;
	public float MaxPitch = 60;
	public float LookSensitivity = 1;

	public float MoveSpeed = 10;

	protected CharacterController movementController;
	protected Camera playerCamera;

	protected bool isControlling;
	protected float yaw;
	protected float pitch;

	protected Vector3 velocity;

	private Tween m_Tween = null;


	protected virtual void Start()
	{
		m_Die = false;

		MoveSpeed = InGameMgr.In.PlayerSpeed;

		LookSensitivity = InGameMgr.In.PlayerSensitivity;

		movementController = GetComponent<CharacterController>();   //  Character Controller
		playerCamera = GetComponentInChildren<Camera>();            //  Player Camera

		isControlling = true;
		Cursor.lockState = CursorLockMode.Locked;

		m_Light.color = Color.white;

		m_Tween = transform.DOShakePosition(20.0f,new Vector3(0.0f,2.0f,0.0f));
		m_Tween.Pause();
	}

	protected virtual void Update()
	{
		if(m_Die)
		{
			return;
		}

		if(Input.GetKeyDown(KeyCode.Escape))
        {
            // turn on the cursor
            Cursor.lockState = CursorLockMode.None;
        }

		Vector3 direction = Vector3.zero;
		direction += transform.forward * Input.GetAxisRaw("Vertical");
		direction += transform.right * Input.GetAxisRaw("Horizontal");

		direction.Normalize();

		if(movementController.isGrounded)
		{
			velocity = Vector3.zero;
		}
		else
		{
			velocity += 9.81f * 10 * Time.deltaTime * -transform.up; // Gravity
		}

		direction += velocity * Time.deltaTime;
		movementController.Move(MoveSpeed * Time.deltaTime * direction);

		// Camera Look
		yaw += Input.GetAxisRaw("Mouse X") * LookSensitivity;
		// pitch -= Input.GetAxisRaw("Mouse Y") * LookSensitivity;

		yaw = ClampAngle(yaw, MinYaw, MaxYaw);
		// pitch = ClampAngle(pitch, MinPitch, MaxPitch);

		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
	}

	protected float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;

		return Mathf.Clamp(angle, min, max);
	}

	public async UniTask DiePlayerAsync()
    {
		m_AudioSource.Play();

        movementController.enabled = false;
		m_Rigidbody.isKinematic = true;
		m_Tween.Restart();

        await UniTaskTools.ExecuteOverTimeAsync(0.0f,1.0f,1.0f,(progress)=>
        {
            m_Light.color = Color.Lerp(Color.white,Color.red,progress);
        });

        await UniTaskTools.ExecuteOverTimeAsync(0.0f,1.0f,1.0f,(progress)=>
        {
            m_Light.color = Color.Lerp(Color.red,new Color(1.0f,0.0f,0.0f,0.0f),progress);
        });

		m_Tween.Pause();
    }
}