using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;




public class UIPlayer : MonoBehaviour
{
    //ActionControl actionControl;

    // elematerMenu °´Ã¼ »ý¼º
   public  GameObject elemanterSlot;

    // ElemanterMenu elemanterMenu;
    public Button elemanterbutton1;
    public Button elemanterbutton2;
    public Button elemanterbutton3;
    public Button elemanterbutton4;

    private void Awake()
    {
      
        elemanterbutton1 = GetComponent<Button>();
        
    }

    

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            elemanterSlot.SetActive(true);


        }
        else if (Input.GetMouseButtonUp(1))
        {
            Vector3 mousepostion = Mouse.current.position.ReadValue();
            Debug.Log(mousepostion);

           if(elemanterbutton1 != null )
            {
                elemanterbutton1.onClick.Invoke();
                
            }

            elemanterSlot.SetActive(false);

        }
    }

    



}
