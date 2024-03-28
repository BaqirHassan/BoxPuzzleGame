using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongratulationsWritings : MonoBehaviour
{
    [SerializeField] List<GameObject> writings;

    void Start()
    {
        GameEvents.ShowCongratulationsWritings += ShowCongratulationsWritings;
    }

    private void OnDisable()
    {
        GameEvents.ShowCongratulationsWritings -= ShowCongratulationsWritings;
    }

    // Update is called once per frame
    private void ShowCongratulationsWritings()
    {
        var index = Random.Range(0, writings.Count);

        writings[index].SetActive(true);
    }
}
