using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoMarkerManager : MonoBehaviour {

    public enum MarkerState { Hiro, Kanji, Both, None};
    //reference to the Hiro Marker
    [SerializeField]
    GameObject HiroMarker1;
    //reference to the Kanji Marker
    [SerializeField]
    GameObject KanjiMarker2;
    //reference to the object we want to change color to
    public GameObject ObjectToChangeColor;
    //hold all of our colors associated with states
    public Material[] MyMaterial = new Material[4];
    private Dictionary<MarkerState, Material> _myMaterialDictionary = new Dictionary<MarkerState, Material>();
    ///<NOTE>
    ///could write a seperate script to manage the materials and choice of dictionary verbage
    ///</NOTE>

    #region Variables Related to the Specific Properties of the Tracked Marker
    private ARTrackedObject _hiro;
    private ARTrackedObject _kanji;

    private MarkerState currentMarkerState;
    private MarkerState lastFrameMarkerState;

    private ARMarker _hiroCache;
    private ARMarker _kanjiCache;
    #endregion
    private void Awake()
    {
        
        _hiro = HiroMarker1.GetComponent<ARTrackedObject>();
        _kanji =KanjiMarker2.GetComponent<ARTrackedObject>();
        _hiroCache = _hiro.GetMarker();
        _kanjiCache = _kanji.GetMarker();
        currentMarkerState = MarkerState.None;
        lastFrameMarkerState = MarkerState.None;
    }
    private void Start()
    {
        BuildMaterialDictionary();
    }
    /// <summary>
    /// Builds our Dictionary with our array of materials
    /// </summary>
    private void BuildMaterialDictionary()
    {
        _myMaterialDictionary.Add(MarkerState.Both, MyMaterial[0]);
        _myMaterialDictionary.Add(MarkerState.Hiro, MyMaterial[1]);
        _myMaterialDictionary.Add(MarkerState.Kanji, MyMaterial[2]);
        _myMaterialDictionary.Add(MarkerState.None, MyMaterial[3]);
    }
    private void Update()
    {
        currentMarkerState = CheckMarkers();
        if (currentMarkerState != lastFrameMarkerState)
        {
            //only if we have a change do we want to make a material change
            Material[] mats = ObjectToChangeColor.GetComponent<Renderer>().materials;
            mats[0] = _myMaterialDictionary[currentMarkerState];
            ObjectToChangeColor.GetComponent<Renderer>().materials = mats;
        }

    }
    private void LateUpdate()
    {
        //all the work is going to be done in lateupdate
        lastFrameMarkerState = CheckMarkers();
        
        
    }
    /// <summary>
    /// Just Checks our marker state for what markers are active and returns the state of this
    /// </summary>
    /// <returns></returns>
    private MarkerState CheckMarkers()
    {
        if (_hiroCache != null && _kanjiCache != null)
        {
            if (_hiroCache.Visible && _kanjiCache.Visible)
            {
                
                return MarkerState.Both;
            }
            else
            {
                if (_hiroCache.Visible)
                {
                
                    return MarkerState.Hiro;
                }
                else
                {
                    if (_kanjiCache.Visible)
                    {
                        return MarkerState.Kanji;
                    }
                    else
                    {
                        return MarkerState.None;
                    }
                }
            }
        }else
        {
            return MarkerState.None;
        }
    }
   

}
