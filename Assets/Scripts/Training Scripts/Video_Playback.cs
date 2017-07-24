﻿/*  Author: Benjamin Ferguson
    Date: 10/05/17
    Purpose: To match the playback speed of the 360 video with the rowing speed.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class Video_Playback : MonoBehaviour {
    public double boat_speed; //speed of the boat
    public const double video_speed = 0.875; //speed the video was recorded at in m/s
    public  double normalise_multiplier; //video playback speed at 1m/s
    public double video_playback; //the video playback speed to match the boat speed
    public VideoPlayer video; //video object
	public Text SpeedDisplay; //text object to display speed
    private Pace_Boat pb; //pace boat object
    public float framerate; //rate to refresh video playback speed
    public float deltatime; //time since last refresh

    public double minSpeed; //speed at which the player needs to speed up
    public double maxSpeed; //speed at which the player needs to slow down
    public AudioClip speedUp;
    public AudioClip slowDown;
    private float lastPlayed; //time since audio was last played
    private Color green;
    private Color red;

    // Use this for initialization
    void Start () {
        video = GameObject.Find("VideoSphere").GetComponent<VideoPlayer>();
        pb = GameObject.Find("SceneController").GetComponent<Pace_Boat>();
        boat_speed = 1; ;
		refreshVideoSpeed();
        normalise_multiplier = 1 / video_speed;
        lastPlayed = 0;
        green = new Color32(0x00, 0xFF, 0x4C, 0xFF);
        red = new Color32(0xFF, 0x00, 0x00, 0xFF);
        //framerate = 0.05f;
        //deltatime = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        deltatime += Time.deltaTime;
		//boat_speed = Rowing_Speed.speed;
		// Debug.Log("deltatime:" + deltatime);
		refreshVideoSpeed();
        if (video_playback > pb.pbspeed)
        {
            SpeedDisplay.color = green;
			Debug.Log("Colour green");
        }
        else
        {
            SpeedDisplay.color = red;
			Debug.Log("Colour red");
		}
        SpeedDisplay.text = convertSpeed().ToString() + "km/h";
        lastPlayed += Time.deltaTime;
		//Debug.Log(lastPlayed);
        if (lastPlayed > 5)
        {
            audioController();
        }
		/* if (deltatime >= framerate)
		 {
			 refreshVideoSpeed();
			 deltatime = 0.0f;
		 }
		 */
	}

	// Used to update the speed of the environment
    public void refreshVideoSpeed() {
        video_playback = boat_speed * normalise_multiplier;
        video.playbackSpeed = (float) video_playback;
        //Debug.Log("boat speed:" + boat_speed);
        //Debug.Log("video playback speed:" + video_playback);
    }

    //convert speed from m/s to km/h
    public double convertSpeed() 
	{
		double convertedSpeed = boat_speed * 3600/ 1000; //convert m/s to km/h
		return convertedSpeed;
	}

    public void audioController()
    {
       if (convertSpeed() < minSpeed)
        {
			Debug.Log("Speed up");
            GetComponent<AudioSource>().PlayOneShot(speedUp);
            lastPlayed = 0;
        }
       if (convertSpeed() > maxSpeed)
        {
			Debug.Log("Slow Down");
			GetComponent<AudioSource>().PlayOneShot(slowDown);
            lastPlayed = 0;
        }
    }
}