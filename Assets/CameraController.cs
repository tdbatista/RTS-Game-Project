using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float Speed; //velocidade da camera
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
        

    }

    // Update is called once per frame
    void Update()
    {
        //move a camera usando teclado
        float y = (Input.GetKey(KeyCode.PageUp)?1:0) + (Input.GetKey(KeyCode.PageDown)?-1:0);
        Vector3 axis = new Vector3(Input.GetAxis("Horizontal"),y, Input.GetAxis("Vertical")) * Time.deltaTime *Speed;

        //ajusta o centro da camera de acordo com o angulo dela
        //calculo para achar o cateto oposto sendo o angulo da camera adjacente a hipotenusa
        //e a altura como cateto adjacente
        //calcula o Tan já que o mesmo é CO/CA e não precisa da hipotenusa
        //tem que transformar o eulerangles também para radiano já que é calculado em radianos no unity
        float zPos = Mathf.Tan(cam.transform.localEulerAngles.x* Mathf.Deg2Rad)*transform.position.y;
        cam.transform.localPosition = new Vector3(0,0,-zPos);



        transform.Translate(axis);
        
    }
}
