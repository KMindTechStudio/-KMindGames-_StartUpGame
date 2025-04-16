using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private bool _isLocked = false;
    [SerializeField] private bool _isVisited = false;
    [SerializeField] private GameObject _floor;
    [SerializeField] private GameObject[] _gates;
    private void Start()
    {
        // set up the gates for the room when reaching the room for the first time
        // if (!_isVisited)
        // {
        //     SetUpGate();
        // }
    }

    public void SetUpGate()
    {
        foreach (var gate in _gates)
        {
            gate.GetComponent<Gate>().IsLooked = _isLocked;
            gate.gameObject.SetActive(false);
        }

        int randomNumber = Random.Range(0, 4);
        _gates[randomNumber].SetActive(true);
    }



}
