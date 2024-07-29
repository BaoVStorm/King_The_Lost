using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyInf> ListEnemy;

    public float Point;

    public float Times;

    private float Time_;

    [SerializeField]
    private float AmoutOfEnemy;
    [SerializeField]
    private float MaxEnemySpawn;
    [SerializeField]
    private int DifficultOfLevel;
    
    [SerializeField]
    private bool IsDelay;

    private float TimeSpawnEnemy;

    public Player Player;

    public BarAni PointBar;

    // Spawn Enemy
    public bool SpawnEnemy1 = false;
    public bool SpawnEnemy2 = false;
    public bool SpawnEnemy3 = false;

    //SingleTon
    private static EnemyManager _instance;
    public static EnemyManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = GameObject.FindObjectOfType<EnemyManager>();

            return _instance;
        }
    }
    
    void Start()
    {
        _instance = this;

        Point = 0f;

        Times = 0f;
        Time_ = 0f;

        AmoutOfEnemy = 10;

        MaxEnemySpawn = 0;

        Start_PoolingEnemy();

        StartCoroutine(DelayStartGame());
    }

    void Update()
    {
        // Spawn Enemy
        if(SpawnEnemy1)
        {
            SpawnEnemy1 = false;

            SpawnListEnemy(0, UnityEngine.Random.Range(1, 3));
        }

        if(SpawnEnemy2)
        {
            SpawnEnemy2 = false;

            SpawnListEnemy(1, UnityEngine.Random.Range(1, 3));
        }

        if(SpawnEnemy3)
        {
            SpawnEnemy3 = false;

            SpawnListEnemy(2, UnityEngine.Random.Range(1, 3));
        }


        // Update Info

        Times += 1 * Time.deltaTime;
        Time_ += 1 * Time.deltaTime;

        // if(Times / TimeSpawnEnemy >= DifficultOfLevel)
        // {
        //     MaxEnemySpawn += 1;
        //     DifficultOfLevel ++;

        //     TimeSpawnEnemy = UnityEngine.Random.Range(15f, 23f);
        // }

        if(Time_ >= TimeSpawnEnemy)
        {
            MaxEnemySpawn += 1;
            DifficultOfLevel ++;

            Time_ = 0f;

            TimeSpawnEnemy = UnityEngine.Random.Range(24f + DifficultOfLevel * 1.3f, 28f + DifficultOfLevel * 1.3f);
        }

        if(AmoutOfEnemy < MaxEnemySpawn && IsDelay == false)
        {
            int Direct = UnityEngine.Random.Range(1, 3);

            StartCoroutine(DelaySpawnEnemy());

            if(ListEnemy.Count != 0)
            {   
                AmoutOfEnemy ++;

                int TypeOfEnemy = UnityEngine.Random.Range(0, Math.Min(ListEnemy.Count, DifficultOfLevel));

                if(TypeOfEnemy != 0 && TypeOfEnemy == UnityEngine.Random.Range(0, Math.Min(ListEnemy.Count, DifficultOfLevel)))
                    TypeOfEnemy --;

                SpawnListEnemy(TypeOfEnemy, Direct);
            }
        }
    }

    IEnumerator DelaySpawnEnemy()
    {
        IsDelay = true;

        if(AmoutOfEnemy >= 1)
            yield return new WaitForSeconds(UnityEngine.Random.Range(3f + (AmoutOfEnemy - 1) * 1.5f, 5f + (AmoutOfEnemy - 1) * 1.5f));
        else   
            yield return new WaitForSeconds(3f);

        IsDelay = false;
    }

    IEnumerator DelayStartGame()
    {
        yield return new WaitForSeconds(2.5f);
        
        AmoutOfEnemy = 0;

        MaxEnemySpawn = 2;

        DifficultOfLevel = 1;

        IsDelay = false;

        TimeSpawnEnemy = 20f;
    }

    public Enemy_Base SpawnListEnemy(int i, int Direct) // Direct = 1(Right), Direct = 2(Left) 
    {
        Enemy_Base Ey = null;

        if(i < ListEnemy.Count)
        {
            Ey = GetPoolingEnemy(i);

            if(Direct == 1)
            {
                Ey.transform.position = new UnityEngine.Vector3(-16, 0, 0);
            }
            else
            {
                Ey.transform.position = new UnityEngine.Vector3(16, 0, 0);
            }

            Ey.SetActiveObj(true);

            Ey.Set_DirectionMoveAndSkin(Direct, ListEnemy[i].Skin_Path);
        }

        return Ey;
    }

    public void EnemyKILL()
    {
        AmoutOfEnemy --;

        Point ++;

        PointBar.Set_IsAct_True();
    }

    // Pooling
    private List<List<Enemy_Base>> PoolingEnemy = new List<List<Enemy_Base>>();
    private int AmountPoolingEnemy = 4;

    private void Start_PoolingEnemy()
    {
        for(int i = 0; i < ListEnemy.Count; i++)
        {
            List<Enemy_Base> TempList = new List<Enemy_Base>();

            for(int j = 0; j < Math.Max(AmountPoolingEnemy - i, 1); j++)
            {
                Enemy_Base En = Instantiate(ListEnemy[i].Enemy);

                En.SetActiveObj(false); 

                if(Player != null)
                    En.Set_Player(Player);

                TempList.Add(En);
            }

            PoolingEnemy.Add(TempList);
        }
    }

    private Enemy_Base GetPoolingEnemy(int index)
    {
        for(int i = 0; i < PoolingEnemy[index].Count; i++)
        {
            if(!PoolingEnemy[index][i].gameObject.activeInHierarchy)
            {
                return PoolingEnemy[index][i];
            }
        }

        Enemy_Base En = Instantiate(ListEnemy[index].Enemy);

        En.SetActiveObj(false); 

        if(Player != null)
            En.Set_Player(Player);

        PoolingEnemy[index].Add(En);

        return En;
    }
}

[System.Serializable]
public class EnemyInf
{
    public String Name;

    public int MaxEnemySpawn;

    public String Skin_Path;

    public Enemy_Base Enemy;
}

// enemymanager     singleton
//      list <Enemy> 
//      point
//      time
// 
// Enemy Confibs    json(Singleton)
//      Hp, Point, knife, refabs
