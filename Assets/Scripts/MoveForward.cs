﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{

  [SerializeField] private float speed = 10f;
  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
    // Move object forward
    transform.Translate(Vector3.forward * Time.deltaTime * speed);
  }
}
