using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalsDND : MonoBehaviour {

    //public const string TOTAL_COIN_CONTIBUTION = "TOTAL_COIN_CONTIBUTION";

   public const string TOTAL_COINS_START = "TOTAL_COINS_START";

   public const string LAST_LEVEL        = "LAST_LEVEL";
   public const string LAST_LEVEL_SCORE  = "LAST_LEVEL_SCORE";
   public const string LAST_LEVEL_TIME   = "LAST_LEVEL_TIME";
   public const string LAST_LEVEL_TIME2  = "LAST_LEVEL_TIME2";

    //public const string TOTAL_COIN_END    = "TOTAL_COIN_CONTIBUTION";
    public const string TOTAL_COINS_END   = "TOTAL_COINS_END";

    public const string NEXT_LEVEL        = "NEXT_LEVEL";
    

    public static GlobalsDND gloabsDND;

  
    public static Dictionary<string, string> MapStrStr = new Dictionary<string, string>();

    public static void Add(string key, string value)
    {
        if (GlobalsDND.MapStrStr.ContainsKey(key))
        {
            GlobalsDND.MapStrStr.Remove(key);
        }
        GlobalsDND.MapStrStr.Add(key, value);
    }

    public static string Get(string key)
    {
        string o = null;
        GlobalsDND.MapStrStr.TryGetValue(key, out o);
        return o;
    }


    void Awake()
    {
        if (gloabsDND == null)
        {
            gloabsDND = this.GetComponent<GlobalsDND>();

            DontDestroyOnLoad(this.gameObject);
        }else
        {
            Destroy(this.gameObject);
        }
    }


    void go()
    {

        Scene scene = SceneManager.GetActiveScene();
        GameObject[] rootObjects =   scene.GetRootGameObjects();

        foreach (GameObject go in rootObjects)
        {
         
            
        }
    }

}
