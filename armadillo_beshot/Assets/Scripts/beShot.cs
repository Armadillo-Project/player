using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beShot : MonoBehaviour
{   //max 체력과 현재 체력 변수
    public int maxHealth = 100;
    public int currentHealth;
    //죽음 여부 무적모드 여부 판단 변수
    public bool isDie = false;
    public bool isUnBeatTime = false;
    public SpriteRenderer spriteRenderer;
    //속도
    public float speed;
    public Vector2 speed_vec;

    //hp바 움직이기 !!
    private GameObject m_goHpBar;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("아르마딜로 피격 테스트");
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
       //오브젝트 찾기
        m_goHpBar = GameObject.Find("Canvas/Slider");

    }
    //탄환과 충돌 판정
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet") && isUnBeatTime == false) //탄환과 만났고, 무적타임이 아닐 시
        {
            Debug.Log("탄환 피격");

            //체력 25감소
            currentHealth = currentHealth - 25;
            Debug.Log("현재 체력은" + currentHealth);

            //탄환 피격 시 3초 간 무적모드
            StartCoroutine("UnBeatTime");

        }
    }

    // Update is called once per frame
    void Update()
    {
        //체력 체크해서 죽음 판정
        if (currentHealth == 0)
        {
            if (!isDie)
                Die();
            return;
        }

        //아르마딜로 이동
        speed_vec = Vector2.zero;
        if(Input.GetKey(KeyCode.RightArrow))
        {
            speed_vec.x += 0.1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            speed_vec.x -= 0.1f;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            speed_vec.y += 0.1f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            speed_vec.y -= 0.1f;
        }
        transform.Translate(speed_vec);

        //hp_bar 이동 범위 제한 함으로써 아르마딜로 오브젝트를 따라다니게 함
        m_goHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(-1.4f, 3.8f, 0));
    }

    void Die()
    {

        isDie = true;
        //버튼 비활성화
    }

    IEnumerator UnBeatTime()
    {
        Debug.Log("무적모드 시작");
        int countTime = 0;
        //무적모드 플래그 t 변경
        isUnBeatTime = true;

        while (countTime < 30)
        {
            //알파값 수정해서 깜빡이도록 
            if (countTime % 2 == 0)
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                spriteRenderer.color = new Color32(255, 255, 255, 180);
            //3초간 딜레이
            yield return new WaitForSeconds(0.1f);
            countTime++;
        }
        //알파값 원상 복귀
        spriteRenderer.color = new Color32(255, 255, 255, 255);
        //무적모드 아님
        isUnBeatTime = false;
        Debug.Log("무적모드 종료");

        yield return null;
    }
}