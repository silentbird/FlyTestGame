﻿/***********************************************
				EasyTouch IV
	Copyright © 2014-2015 The Hedgehog Team
  http://www.blitz3dfr.com/teamtalk/index.php
		
	  The.Hedgehog.Team@gmail.com
		
**********************************************/
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class BaseFinger{

	public int fingerIndex;	
	public int touchCount;
	public Vector2 startPosition;
	public Vector2 position;
	public Vector2 deltaPosition;	
	public float actionTime;
	public float deltaTime;		
	
	public Camera pickedCamera;
	public GameObject pickedObject;
	public bool isGuiCamera;
		
	public bool isOverGui;
	public GameObject pickedUIElement;

	private static readonly Gesture gesture = new Gesture();
	public Gesture GetGesture(){

		// 目前不需要缓存此对象,改成单例,不需要每次都分配 xuzhihui 2022.08.02
		// Gesture gesture = new Gesture();
		gesture.fingerIndex = fingerIndex;
		gesture.touchCount = touchCount;
		gesture.startPosition = startPosition;
		gesture.position = position;
		gesture.deltaPosition = deltaPosition;
		gesture.actionTime = actionTime;
		gesture.deltaTime = deltaTime;
		gesture.isOverGui = isOverGui;

		gesture.pickedCamera = pickedCamera;
		gesture.pickedObject = pickedObject;
		gesture.isGuiCamera = isGuiCamera;

		gesture.pickedUIElement = pickedUIElement;

		return gesture;
	}

}
