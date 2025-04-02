using UnityEngine;

public class LogicaPies : MonoBehaviour
{

    public PlayerMove playerMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnTriggerStay(Collider other)
    {

        playerMove.puedoSaltar = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerMove.puedoSaltar = false;
    }


}
