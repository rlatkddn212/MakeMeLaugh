// using System.Collections;
// using System.Collections.Generic;
// using Cysharp.Threading.Tasks;
// using UnityEngine;

// public class CharacterController : MonoBehaviour 
// {
//     public float speed = 10.0f;

//     [SerializeField]
//     private Light m_Light = null;
//     [SerializeField]
//     private Rigidbody m_Rigidbody = null;

//     void Start ()
//     {
//         speed = InGameMgr.In.PlayerSpeed;

//         // turn off the cursor
//         Cursor.lockState = CursorLockMode.Locked;	
// 	}
	
// 	// Update is called once per frame
// 	void Update ()
//     {
//         if(!InGameMgr.In.IsStart)
//         {
//             return;
//         }
//         var horizontal = Input.GetAxis("Horizontal");
//         var vertical = Input.GetAxis("Vertical");
//         var movement = new Vector3(horizontal,0.0f,vertical);
//         m_Rigidbody.MovePosition(m_Rigidbody.position+speed * Time.deltaTime * movement);

//         if(Input.GetKeyDown(KeyCode.Escape))
//         {
//             // turn on the cursor
//             Cursor.lockState = CursorLockMode.None;
//         }
//     }

//     public async UniTask DiePlayerAsync()
//     {
//         UniTaskTools.ShakePositionAsync(transform,new Vector3(0.0f,0.5f,0.0f),2.0f,null).Forget();

//         await UniTaskTools.ExecuteOverTimeAsync(0.0f,1.0f,1.0f,(progress)=>
//         {
//             m_Light.color = Color.Lerp(Color.white,Color.red,progress);
//         });

//         await UniTaskTools.ExecuteOverTimeAsync(0.0f,1.0f,1.0f,(progress)=>
//         {
//             m_Light.color = Color.Lerp(Color.red,new Color(1.0f,0.0f,0.0f,0.0f),progress);
//         });
//     }
// }
