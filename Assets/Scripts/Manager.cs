using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    private GameObject prefabs_Pin;
    private GameObject currentPin;
    private Transform m_CreatePoint;
    private Transform m_ReadyPoint;
    private Transform m_Circle;

    private Vector3 target;

    private Camera main_Camera;

    private Button exit_Btn;
    private Button shoot_Btn;
    private Button pause_Btn;
    private Text m_Text;
    private int score;

    private bool isReady=false;
    private bool canShoot = false;
    private bool pause = false;
    private bool gameOver = false;
  

    void Start () {
        Init();
        CreatePin();
    }


    private void Init()
    {
        m_CreatePoint = GameObject.Find("CreatePoint").transform;
        m_ReadyPoint = GameObject.Find("ReadyPoint").transform;
        m_Circle = GameObject.Find("Circle").transform;

        target = new Vector3(m_Circle.transform.position.x, m_Circle.transform.position.y - 1.7f, m_Circle.transform.position.z);

        main_Camera = Camera.main;

        exit_Btn = GameObject.Find("Canvas/Exit").GetComponent<Button>();
        shoot_Btn = GameObject.Find("Canvas/Shoot").GetComponent<Button>();
        pause_Btn = GameObject.Find("Canvas/Pause").GetComponent<Button>();

        exit_Btn.onClick.AddListener(()=> Application.Quit());
        shoot_Btn.onClick.AddListener(shootBtnClick);
        pause_Btn.onClick.AddListener(()=> pause = !pause);

        m_Text = GameObject.Find("Canvas/Score").GetComponent<Text>();

        prefabs_Pin = Resources.Load<GameObject>("Prefabs/Pin");
    }


    private void shootBtnClick() {
        if (canShoot)
        {
            StartCoroutine(Shoot());
            score += 1;
            m_Text.text = score.ToString();
            isReady = false;
        }
       
    }


    void Update()
    {
        if (!isReady)
        {
            Ready();
        }
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    private void CreatePin() {
        currentPin = Instantiate<GameObject>(prefabs_Pin, m_CreatePoint.position,Quaternion.Euler(new Vector3(0,0,90)));
    }


    private void Ready() {
        currentPin.transform.position = Vector3.MoveTowards(currentPin.transform.position, m_ReadyPoint.transform.position, 5 * Time.deltaTime);
        if (Vector3.Distance(currentPin.transform.position, m_ReadyPoint.transform.position) < 0.05f)
        {
            currentPin.transform.position = m_ReadyPoint.transform.position;
            isReady = true;
            canShoot = true;
        }
    }


    IEnumerator Shoot() {
        while (true)
        {
            currentPin.transform.position = Vector3.MoveTowards(currentPin.transform.position, target, AppConst.Instance().PinShootSpeed  * Time.deltaTime);
            yield return null;
            if (Vector3.Distance(target, currentPin.transform.position) <= 0.1f)
            {
                currentPin.transform.position = target;
                currentPin.transform.SetParent(m_Circle);
                CreatePin();
                break;
            }
        }
    }

    public void GameOver() {
        if (!gameOver)
        {
        m_Circle.GetComponent<RotateSelf>().enabled = false;
        canShoot = false;
        StartCoroutine(GameOverAnimation());
        Invoke("LoadScene", 1.5f);
         gameOver = true;
        }

    }


    IEnumerator GameOverAnimation() {
            Debug.Log("游戏结束");
            while (true)
            {
                main_Camera.orthographicSize = Mathf.Lerp(main_Camera.orthographicSize, 4, Time.deltaTime);
                main_Camera.backgroundColor = Color.Lerp(main_Camera.backgroundColor, Color.green, Time.deltaTime);
                yield return null;
            }
    }

    private void LoadScene() {
        SceneManager.LoadScene("Main");
    }

}
