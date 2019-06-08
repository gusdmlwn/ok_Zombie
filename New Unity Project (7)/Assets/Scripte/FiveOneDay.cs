using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveOneDay : MonoBehaviour
{
    public int days;
    public int minutes;
    public int seconds;
    public float counter;

    public int dawnstartTime = 1; //새벽시작시간
    public int daystartTime = 2; //하루가 시작하는시간
    public int duskstartTime = 3;//해가 지는 시작시간
    public int nightstartTime = 4;//밤이 되는 시간

    public float sunDimTume = 0.01f; //태양이 어두워지는 정도
    public float dawnsunIntensity = 0.05f;
    public float daysunIntensity = 1f;
    public float dusksunIntensity = 0.25f;
    public float nightsunIntensity = 0f;

    public float ambientDimTime = 0.0001f; //주변이 어두워짐
    public float dawnambientIntenstiy = 0.5f;
    public float dayambientIntenstiy = 1f;
    public float duskambientIntenstiy = 0.25f;
    public float nightambientIntenstiy = 0f;

    public float dawnSkyboxBlendfactor = 0.5f;
    public float daySkyboxBlendfactor = 1f;
    public float duskSkyboxBlendfactor = 0.25f;
    public float nightSkyboxBlendfactor = 0f;

    public float skyboxBlendfactor;
    public float skyboxBlendspeed = 0.1f;//스카이박스가 변화하는 속도

    public int guiwidth = 100;
    public int guiheight = 20;

    public FiveDayPhases fiveDayPhases; //하루의단계를 새벽 하루 해질녁 밤으로 나눔 
    public enum FiveDayPhases
    {
        Dawn,Day,Dusk,Night
    }
    // Start is called before the first frame update
    private void Awake()
    {
        fiveDayPhases = FiveDayPhases.Night; //시작시 낮을 밤으로 설정
        RenderSettings.ambientIntensity = nightambientIntenstiy;//시작시 어두워짐을 밤으로 설정

        GetComponent<Light>().intensity = nightsunIntensity; //
    }
    void Start()
    {
        StartCoroutine("TimeOfDay");
        days = 1;
    }

    // Update is called once per frame
    void Update()
    {
        SecondsCounter();// 초
        UpdateSkyBox();//스카이박스를 매번 변화
    }

    IEnumerator TimeOfDay()
    {
        while (true)
        {
            switch (fiveDayPhases)
            {
                case FiveDayPhases.Dawn:
                    Dawn();
                    break;
                case FiveDayPhases.Day:
                    Day();
                    break;
                case FiveDayPhases.Dusk:
                    Dusk();
                    break;
                case FiveDayPhases.Night:
                    Night();
                    break;
            }
            yield return null; //yield 키워드는 호출자(Caller)에게 컬렉션 데이타를 하나씩 리턴할 때 사용한다.
        }
    }
    void SecondsCounter()
    {
        if (counter == 60)
            counter = 0;

        counter += Time.deltaTime; //매프레임마다 시간을 1초씩 더함
        seconds = (int)counter;

        if (counter < 60)
            return;

        if (counter > 60) //카운터가 60보다 크면 카운터를 60으로 만들어라
            counter = 60;

        if (counter == 60)
            MintesCounter(); //60초가되면 분 함수를 호출
    }
    void MintesCounter()
    {
        minutes++;
        if (minutes == 5)
        {
            DayCounter();//5분이되면 Daycounter를 호출
            minutes = 0;
        }
    }
    void DayCounter()
    {
        Debug.Log("DayCounter");
        days++;
    }
    void Dawn()//새벽일때 태양의 강도와 주변의밝기
    {
        Debug.Log("Dawn");
        if (GetComponent<Light>().intensity < dawnsunIntensity) 
            GetComponent<Light>().intensity += sunDimTume * Time.deltaTime; 

        if (GetComponent<Light>().intensity > dawnsunIntensity)
            GetComponent<Light>().intensity = dawnsunIntensity;

        if (RenderSettings.ambientIntensity < dawnambientIntenstiy)
            RenderSettings.ambientIntensity += ambientDimTime * Time.deltaTime;

        if (RenderSettings.ambientIntensity > dawnambientIntenstiy)
            RenderSettings.ambientIntensity = dawnambientIntenstiy;

        if (minutes == daystartTime && minutes < duskstartTime)
        {
            fiveDayPhases = FiveDayPhases.Day;
        }
    }
    void Day()//하루가 시작할때 태양의 강도와 주변밝기
    {
        Debug.Log("Day");
        if (GetComponent<Light>().intensity < daysunIntensity)
            GetComponent<Light>().intensity += sunDimTume * Time.deltaTime; 

        if (GetComponent<Light>().intensity > daysunIntensity)
            GetComponent<Light>().intensity = daysunIntensity;

        if (RenderSettings.ambientIntensity < dayambientIntenstiy) 
            RenderSettings.ambientIntensity += ambientDimTime * Time.deltaTime; 

        if (RenderSettings.ambientIntensity > dayambientIntenstiy)
            RenderSettings.ambientIntensity = dayambientIntenstiy;

        if (minutes == duskstartTime && minutes < nightstartTime)
        {
            fiveDayPhases = FiveDayPhases.Dusk;
        }
    }
    void Dusk()//져녁일때 주변밝기 태양강도
    {
        Debug.Log("Dusk");
        if (GetComponent<Light>().intensity > dusksunIntensity) 
            GetComponent<Light>().intensity -= sunDimTume * Time.deltaTime; 

        if (GetComponent<Light>().intensity < dawnsunIntensity)
            GetComponent<Light>().intensity = dawnsunIntensity;

        if (RenderSettings.ambientIntensity > duskambientIntenstiy) 
            RenderSettings.ambientIntensity -= ambientDimTime * Time.deltaTime; 

        if (RenderSettings.ambientIntensity < duskambientIntenstiy)
            RenderSettings.ambientIntensity = duskambientIntenstiy;

        if (minutes == nightstartTime)
        {
            fiveDayPhases = FiveDayPhases.Night;
        }
    }
    void Night()//밤일때 주변밝기와 태양강도
    {
        Debug.Log("Night");
        if (GetComponent<Light>().intensity > nightsunIntensity) 
            GetComponent<Light>().intensity -= sunDimTume * Time.deltaTime; 

        if (GetComponent<Light>().intensity < dawnsunIntensity)
            GetComponent<Light>().intensity = dawnsunIntensity;

        if (RenderSettings.ambientIntensity > nightambientIntenstiy) 
            RenderSettings.ambientIntensity -= ambientDimTime * Time.deltaTime; 

        if (RenderSettings.ambientIntensity < nightambientIntenstiy)
            RenderSettings.ambientIntensity = nightambientIntenstiy;

        if (minutes == dawnstartTime && minutes < daystartTime)
        {
            fiveDayPhases = FiveDayPhases.Dawn;
        }
    }
    private void OnGUI()//GUI
    {
        //GUI로 날짜를 셈
        GUI.Label(new Rect(Screen.width - 50, 5, guiwidth, guiheight), "Day" + days);
        if (seconds < 10)
        {
            GUI.Label(new Rect(Screen.width - 50, 25, guiwidth, guiheight), minutes + ":" + 0 + seconds);
        }
        else
            GUI.Label(new Rect(Screen.width - 50, 25, guiwidth, guiheight), minutes + ":" + seconds);
    }
    private void UpdateSkyBox() //스카이박스 변화함수
    {
        Debug.Log("UpdateSkyBox");
        if (fiveDayPhases == FiveDayPhases.Dawn)
        {
            if (skyboxBlendfactor == dawnSkyboxBlendfactor)
                return;
            skyboxBlendfactor += skyboxBlendspeed * Time.deltaTime;

            if (skyboxBlendfactor > dawnSkyboxBlendfactor)
                skyboxBlendfactor = dawnSkyboxBlendfactor;
        }
        if (fiveDayPhases == FiveDayPhases.Day)
        {
            if (skyboxBlendfactor == daySkyboxBlendfactor)
                return;
            skyboxBlendfactor += skyboxBlendspeed * Time.deltaTime;

            if (skyboxBlendfactor > daySkyboxBlendfactor)
                skyboxBlendfactor = daySkyboxBlendfactor;
        }
        if (fiveDayPhases == FiveDayPhases.Dusk)
        {
            if (skyboxBlendfactor == duskSkyboxBlendfactor)
                return;
            skyboxBlendfactor -= skyboxBlendspeed * Time.deltaTime;

            if (skyboxBlendfactor < duskSkyboxBlendfactor)
                skyboxBlendfactor = duskSkyboxBlendfactor;
        }
        if (fiveDayPhases == FiveDayPhases.Night)
        {
            if (skyboxBlendfactor == nightSkyboxBlendfactor)
                return;
            skyboxBlendfactor -= skyboxBlendspeed * Time.deltaTime;

            if (skyboxBlendfactor < nightSkyboxBlendfactor)
                skyboxBlendfactor = nightSkyboxBlendfactor;
        }
        RenderSettings.skybox.SetFloat("_Blend", skyboxBlendfactor);
    }
}

