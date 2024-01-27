using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLocation : MonoBehaviour
{

    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.V))
    //    {
    //        FaceMgr.In.StopDetect();
    //        SceneManager.LoadScene("TitleScene");
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exit"); 
            // FaceMgr.In.StopDetect();
            SceneManager.LoadScene("TitleScene");
        }
    }
}
