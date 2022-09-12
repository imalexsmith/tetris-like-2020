using System.Collections;
using System.Collections.Generic;
using MyBox;

using NightFramework;

using TheGame;

using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    [MyBox.DefinedValues("раз...", "два...", "три...", "нахуй иди!")]
    public string TestPredef;

    [SearchableEnum]
    public FigureBlockColorKeys TestSearchableEnum;

    [Space]
    public SuperTimer TestSuperTimer;
    public List<SuperTimer> TestListOfTimers;
    
    [Space]
    public RandomisedSet<int> TestRandomSet;
    public List<RandomisedSet<string>> TestListOfRandomSets;

    [Space]
    public PropertyEffectFloat TestPropertyEffect;
    public List<PropertyEffectFloat> TestListOfPropertyEffects;

    [Space]
    public string TestString;
    public List<string> ListOfStrings;
    public GameObject enemy;


    protected void Update()
    {
        TestSuperTimer.Update();
        /*print(nameof(SuperDuperTimer) + " " + SuperDuperTimer.NumberOfCompletions + " " + SuperDuperTimer.Progress);
        foreach (var subTimer in SuperDuperTimer.SubTimers)
        {
            print(nameof(subTimer) + " " + subTimer.NumberOfCompletions + " " + subTimer.Progress);
        }*/


       
    }

    [ButtonMethod]
    public void TestMethod()
    {
        //AdvancedGrid.Instance.ViewAs++;

        
        var v = TestRandomSet.SelectRandomValues();
        var s = "";
        foreach (var item in v)
        {
            s += item + ", ";
        }
        print(s);
    }

    [ButtonMethod]
    public void StartTimer()
    {
        TestSuperTimer.Start();
    }

    [ButtonMethod]
    public void PauseTimer()
    {
        TestSuperTimer.Pause();
    }

    [ButtonMethod]
    public void ResumeTimer()
    {
        TestSuperTimer.Resume();
    }

    [ButtonMethod]
    public void StopTimer()
    {
        TestSuperTimer.Stop();
    }

    /*  public enum SearchTarget
      {
          Position,
          Resource,
          Unexplored
      }

      public Pathfinding PathfindingLink;
      public Pathfinding.DistanceEstimate PathfindingType;
      public SearchTarget Target;
      public Vector2Int SearchRadius;
      public Transform StartPos;
      public Transform EndPos;
      public Tilemap FogOfWar;

      private List<Vector3Int> _path;

      void Start()
      {
          StartCoroutine(SearchPath());
      }

      private void Update()
      {
          if (EndPos)
          {
              if (Input.GetKey(KeyCode.RightArrow))
                  EndPos.position = new Vector3(EndPos.position.x + .1f, EndPos.position.y);

              if (Input.GetKey(KeyCode.LeftArrow))
                  EndPos.position = new Vector3(EndPos.position.x - .1f, EndPos.position.y);

              if (Input.GetKey(KeyCode.UpArrow))
                  EndPos.position = new Vector3(EndPos.position.x, EndPos.position.y + .1f);

              if (Input.GetKey(KeyCode.DownArrow))
                  EndPos.position = new Vector3(EndPos.position.x, EndPos.position.y - .1f);
          }
      }

      IEnumerator SearchPath()
      { 
          while (true)
          {
              if (StartPos && SearchRadius.x > 0 && SearchRadius.y > 0)
              {
                  switch (Target)
                  {
                      case SearchTarget.Position:
                          if (EndPos)
                              _path = PathfindingLink.FindPath(StartPos.position, EndPos.position, SearchRadius.x, SearchRadius.y, PathfindingType).Path;
                          break;
                      case SearchTarget.Resource:
                          _path = PathfindingLink.FindPath(StartPos.position, x => x.Resource, SearchRadius.x, SearchRadius.y, PathfindingType, true).Path;
                          break;
                      case SearchTarget.Unexplored:
                          _path = PathfindingLink.FindPath(StartPos.position, x => !x.Explored, SearchRadius.x, SearchRadius.y, PathfindingType).Path;
                          break;
                      default:
                          break;
                  }

                  if (_path != null && _path.Count > 1)
                      StartPos.position = PathfindingLink.AGrid.GridLayout.GetCellCenterWorld(_path[1]);
              }

              yield return new WaitForSeconds(0.5f);
          }
      }

      protected void OnDrawGizmos()
      {
          Gizmos.color = Color.red;

          Gizmos.DrawWireCube(StartPos.position, new Vector3(SearchRadius.x * 2 + 1, SearchRadius.y * 2 + 1, 1));

          if (_path == null || _path.Count == 0)
              return;

          Gizmos.DrawSphere(PathfindingLink.AGrid.GridLayout.GetCellCenterWorld(_path[0]), 0.1f);

          for (int i = 1; i < _path.Count; i++)
          {
              Gizmos.color = new Color((_path.Count - (i + 1f)) / _path.Count, (i + 1f) / _path.Count, 0, 1);

              var c1 = PathfindingLink.AGrid.GridLayout.GetCellCenterWorld(_path[i - 1]);
              var c2 = PathfindingLink.AGrid.GridLayout.GetCellCenterWorld(_path[i]);
              Gizmos.DrawLine(c1, c2);
              Gizmos.DrawSphere(c2, 0.1f);
          }
      } */
}