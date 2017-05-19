using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hinter.Manager
{
    public class Manager : MonoBehaviour
    {
        public static PlayerManager playerManager;
        public PlayerManager _tempPlayerManager;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            playerManager = transform.GetComponentInChildren<PlayerManager>();
        }

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}