using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour {
    public Image SecPointer, MinPointer, HourPointer;
    public bool PlayPointerSound = true,  SystemDate = false, PlayingSound = false;
    int Year , Month, Day , Hour , Minute , Second ; 
    public System.DateTime CustomClock;
    public float speed = 10.0f;
  
    private float sSecond = 0.0f, sMinute = 0.0f, sHour = 0.0f;
    private System.DateTime currentTime;
    private AudioSource Asource;
 
    
    
     string ClockType = "Clock1";

    // Use this for initialization
   public void Start () {
        //Alarm = new System.DateTime(1, 1, 1, AlarmHour, AlarmMin, 0);
        //sAlarmM = Alarm.Minute;
        //sAlarmH = Alarm.Hour % 12;
        Year = System.DateTime.Now.Year;
        Month = System.DateTime.Now.Month;
        Day = System.DateTime.Now.Day;
        Hour = System.DateTime.Now.Hour % 12;
        Minute = System.DateTime.Now.Minute;
        Second = 0; 
                    //if (!SystemDate)
                    //{
        CustomClock = new System.DateTime(Year, Month, Day, Hour, Minute, Second);
        //}
        
            Asource = GetComponent<AudioSource>();

    }

    IEnumerator PlaySound()
    {
        Asource.Play();
        yield return new WaitForSeconds(60 / speed);
        PlayingSound = false;
    }

    //void VerifyAlarm()
    //{
    //    if (Alarming && AlarmActive)
    //    {
    //        double ang = (float)Mathf.Sin(Time.time * 100.0f) * 20;
    //        //Hammer.localRotation = Quaternion.Euler(0, 0, (float)ang);
    //        AlarmAudio.enabled = true;
    //    }
    //    else
    //    {
    //        Alarming = false;
    //        AlarmAudio.enabled = false;
    //    }
    //}

    private void SystemClock()
    {

        //
        if (SystemDate)
        {
            speed = 1.0f;
            currentTime = System.DateTime.Now;
            sSecond = System.DateTime.Now.Second;
            sMinute = System.DateTime.Now.Minute;
            sHour = System.DateTime.Now.Hour % 12;
            sSecond = System.DateTime.Now.Second;
            sMinute = System.DateTime.Now.Minute;
            sHour = System.DateTime.Now.Hour % 12;
            //
            Year = currentTime.Year;
            Month = currentTime.Month;
            Day = currentTime.Day;
            Hour = currentTime.Hour;
            Minute = currentTime.Minute;
            Second = currentTime.Second;
            //
        }
        else
        {
            CustomClock = CustomClock.AddSeconds(Time.deltaTime * speed);
            sSecond = CustomClock.Second;
            sMinute = CustomClock.Minute;
            sHour = CustomClock.Hour % 12;
            //
            Year = CustomClock.Year;
            Month = CustomClock.Month;
            Day = CustomClock.Day;
            Hour = CustomClock.Hour;
            Minute = CustomClock.Minute;
            Second = CustomClock.Second;
            currentTime = CustomClock;
        }
        if (PlayPointerSound )
        {
            if (PlayingSound == false)
            {
                PlayingSound = true;
                //StartCoroutine(PlaySound());
            }
        }
        

        float SecondAngle = 360 * sSecond / 60;
        float minuteAngle = 360 * (sMinute / 60);
        float hourAngle = (360 * (sHour / 12)) + (sMinute / 2);
        //float AlarmAngle = (360 * (sAlarmH / 12)) + (sAlarmM / 2);
        
            SecPointer.gameObject.transform.localRotation = Quaternion.Euler(0, 0, -SecondAngle);
       
        
        MinPointer.gameObject.transform.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
        HourPointer.gameObject.transform.localRotation = Quaternion.Euler(0, 0, -hourAngle);
        //
        //if (ClockType == "Clock3")
        //{
        //    AlarmPointer.localRotation = Quaternion.Euler(0, 0, AlarmAngle);

        //    //
        //    if (AlarmActive)
        //    {
        //        if (sHour == sAlarmH && sMinute == sAlarmM)
        //        {
        //            Alarming = true;
        //        }
        //    }
        //}
        //
        //AlarmHour = Mathf.Clamp(AlarmHour, 0, 24);
        //AlarmMin = Mathf.Clamp(AlarmMin, 0, 60);
        Day = Mathf.Clamp(Day, 1, 31);
    }

   
    

    //Set a new Date and time for CustomClock.
    public void SetNewDateTime(int Year, int Month, int Day, int Hour, int Minute, int Second)
    {
        SystemDate = true;
        CustomClock = new System.DateTime(Year, Month, Day, Hour, Minute, Second);
        SystemDate = false;
    }
    void Update()
    {
        SystemClock();
        if (Second == 1)
        {
            Asource.Play();
        }
        //if (ClockType == "Clock3")
        //{
        //    VerifyAlarm();
        //    Alarm = new System.DateTime(1, 1, 1, AlarmHour, AlarmMin, 0);
        //    sAlarmM = Alarm.Minute;
        //    sAlarmH = Alarm.Hour % 12;
        //}

    }
}
