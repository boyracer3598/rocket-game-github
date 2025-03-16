using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;


public class rocketControls : MonoBehaviour
{
    Rigidbody rocketRB;
    [SerializeField] AudioClip rockerSound;
    [SerializeField] AudioClip gameoverSound;
    [SerializeField] AudioClip wonSound;
    [SerializeField] AudioClip coinSound;
    AudioSource rocketAudio;
    [SerializeField] float rocketForce;
    [SerializeField] float rotateSpeed;
    bool isGameover = false;
    int coinCount = 0;
    [SerializeField] float fuel = 100;
    [SerializeField] float fuelConsumption = 1;
    [SerializeField] float fuelRefill = 10;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI fuelText;
    // Start is called before the first frame update
    void Start()
    {
        rocketRB = GetComponent<Rigidbody>();
        rocketAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!isGameover)
        {
            UpdateRocketMovement();
        }
            

    }

    private void UpdateRocketMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if(fuel >= 0)
            {
                rocketRB.AddRelativeForce(0, 0, rocketForce * Time.deltaTime);
                fuel -= fuelConsumption * Time.deltaTime;
                fuelText.text = "Fuel: " + fuel.ToString("0");
                Debug.Log("fuel count: " + fuel);
                if (!rocketAudio.isPlaying)
                {
                    rocketAudio.PlayOneShot(rockerSound);
                }
            }
            

        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            rocketAudio.Stop();
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            rocketRB.freezeRotation = true;
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
            rocketRB.freezeRotation = false;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rocketRB.freezeRotation = true;
            transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
            rocketRB.freezeRotation = false;
        }
    }

    

    private void OnCollisionEnter(Collision collision)
    {
       if(!isGameover)
        {
            if (collision.gameObject.tag == "Finish")
            {
                Debug.Log("level won");
                rocketAudio.PlayOneShot(wonSound);
                isGameover = true;
                StartCoroutine(LoadNextLevel());
            }
            else if (collision.gameObject.tag == "Respawn")
            {
                Debug.Log("Game over");
                rocketAudio.PlayOneShot(gameoverSound);
                isGameover = true;
                StartCoroutine(ReloadGame());
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coin")
        {
            coinCount++;
            Debug.Log("coin count: " + coinCount);
            rocketAudio.PlayOneShot(coinSound);
            coinText.text = "Coins: " + coinCount.ToString("0");
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "fuel")
        {
            fuel += fuelRefill;
            Debug.Log("fuel count: " + fuel);
            fuelText.text = "Fuel: " + fuel.ToString("0");
            Destroy(other.gameObject);
        }
    }




    IEnumerator ReloadGame()
    {
        Debug.Log("Reloading game");
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator LoadNextLevel()
    {
        Debug.Log("snene count: " + SceneManager.sceneCountInBuildSettings);
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings-1)
        {
            Debug.Log("Game done");
        }
        else
        {
            Debug.Log("Loading next level");
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }

}
