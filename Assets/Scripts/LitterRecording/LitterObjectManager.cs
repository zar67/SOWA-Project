using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LitterObjectManager : MonoBehaviour
{
    [SerializeField] private AbstractMap m_map;
    [SerializeField] private Transform m_litterObjectHolder;
    [SerializeField] private GameObject m_litterObjectPrefab;

    [SerializeField] private float m_maxDistance = 1000f;
    [SerializeField] private float m_mergedAmountScaleFactor = 0.5f;

    private bool m_locationPinsEnabled = true;

    private Camera m_uiCamera;
    private List<GameObject> m_litterObjects = new List<GameObject>();

    private void Awake()
    {
        m_uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        m_locationPinsEnabled = PlayerPrefs.GetInt(PrefsKeys.LOCATION_PINS_ENABLED_KEY, 1) == 1;
    }

    private void OnEnable()
    {
        SettingsPopup.OnLocationPinsEnabledChanged += OnLocationPinsEnabledChanged;
    }

    private void OnDisable()
    {
        SettingsPopup.OnLocationPinsEnabledChanged -= OnLocationPinsEnabledChanged;
    }

    private void Update()
    {
        if (!m_locationPinsEnabled)
        {
            return;
        }

        List<LitterData> cachedLitter = LitterRecordingManager.Instance.CondensedLitterData;

        int objectCount = 0;
        for (int i = 0; i < cachedLitter.Count; i++)
        {
            Vector2d location = Conversions.StringToLatLon(cachedLitter[i].Location);
            Vector3 worldPosition = m_map.GeoToWorldPosition(location, false);

            if (!IsInRange(worldPosition))
            {
                continue;
            }

            if (objectCount >= m_litterObjects.Count)
            {
                SpawnNewLitterObject();
            }

            m_litterObjects[objectCount].SetActive(true);
            m_litterObjects[objectCount].transform.localPosition = worldPosition;
            m_litterObjects[objectCount].GetComponentInChildren<TextMeshProUGUI>().text = cachedLitter[i].MergedAmount.ToString();

            objectCount++;
        }

        for (; objectCount < m_litterObjects.Count; objectCount++)
        {
            m_litterObjects[objectCount].SetActive(false);
        }
    }

    private void OnLocationPinsEnabledChanged(bool enabled)
    {
        m_locationPinsEnabled = enabled;

        if (!m_locationPinsEnabled)
        {
            foreach (GameObject obj in m_litterObjects)
            {
                obj.SetActive(false);
            }
        }
    }

    private bool IsInRange(Vector3 position)
    {
        return position.magnitude <= m_maxDistance;
    }

    private void SpawnNewLitterObject()
    {
        GameObject newLitterObject = Instantiate(m_litterObjectPrefab, m_litterObjectHolder);
        newLitterObject.transform.localPosition = Vector3.zero;
        newLitterObject.transform.localScale = Vector3.one;

        Canvas canvas = newLitterObject.GetComponentInChildren<Canvas>();
        canvas.worldCamera = m_uiCamera;

        m_litterObjects.Add(newLitterObject);
    }
}
