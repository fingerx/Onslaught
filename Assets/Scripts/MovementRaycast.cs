using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class MovementRaycast : MonoBehaviour
{
    public GameObject navigationAidPrefab = null;
    public LayerMask layerMask;
    public AudioClip moveSound = null;

    private NavMeshAgent m_nmAgent = null;
    private GameObject m_NavigationAidPrefab = null;
    private Transform m_Transform = null;
    private AudioSource m_audioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        m_Transform = transform;
        m_nmAgent = GameManager.instance.player.GetComponent<NavMeshAgent>();

        if (moveSound != null)
        {
            m_audioSource = gameObject.AddComponent<AudioSource>();
            m_audioSource.clip = moveSound;
        }

        m_NavigationAidPrefab = Instantiate(navigationAidPrefab);
        m_NavigationAidPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_nmAgent == null)
            m_nmAgent = GameManager.instance.player.GetComponent<NavMeshAgent>();

        // Do Raycast
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            // Show navigation aid
            m_NavigationAidPrefab.SetActive(true);
            m_NavigationAidPrefab.transform.position = hit.point;

            // Handle input
            if (InputAbstraction.GetButtonDown(InputAbstraction.ButtonAlias.AXIS_CLICK, InputAbstraction.PreferedHand()))
            {
                if (m_nmAgent == null)
                    m_nmAgent = GameManager.instance.player.GetComponent<NavMeshAgent>();

                if (m_nmAgent != null)
                {
                    if (!m_nmAgent.isOnNavMesh)
                        GameManager.instance.PlayerIsLost();

                    m_nmAgent.destination = hit.point;
                    if (m_audioSource != null)
                        m_audioSource.Play();
                }
            }
        }
        else
        {
            m_NavigationAidPrefab.SetActive(false);
        }
    }
}
