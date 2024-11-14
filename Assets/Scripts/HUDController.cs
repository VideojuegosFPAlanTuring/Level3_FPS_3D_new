using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Image damageFlash;
    [SerializeField] private float damageTime;

    private Coroutine disappearCoroutine;

    public static HUDController instance;


    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Turn on Damage Flash when the Player receives a shoot
    /// </summary>
    public void ShowDamageFlash()
    {
        //if there is a coroutine running stop it
        if (disappearCoroutine != null) 
            StopCoroutine(disappearCoroutine);


        //restart the image Color
        damageFlash.color = Color.white;

        //Start coroutine
        disappearCoroutine = StartCoroutine(DamageDissapear());
    }

    IEnumerator DamageDissapear()
    {
        //Alpha variable reset to 1
        float alpha = 1.0f;

        while (alpha > 0.0f)
        {
            alpha -= (1.0f / damageTime) * Time.deltaTime;
            damageFlash.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }        
    }

}
