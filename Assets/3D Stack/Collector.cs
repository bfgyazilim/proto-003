using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] List<Collectible> _gatherables;
    
    [SerializeField] UnityEvent OnCompleteEvent, OnPickedUpFirstEvent;

    List<Collectible> _collectiblesRemaining;
    

    /// <summary>
    /// Register for the OnPickup Event on the all collectibles in the list (in the scene for now, static!)
    /// </summary>
    void OnEnable()
    {
        _collectiblesRemaining = new List<Collectible>(_gatherables);

        foreach (var collectible in _collectiblesRemaining)
            collectible.OnPickup += HandlePickup; // Registering for the OnPickup event on Collectible
        
        UpdateText();
    }

    /// <summary>
    /// Process the method for the result of the Pickup event that you have registered before.
    /// </summary>
    /// <param name="collectible"></param>
    void HandlePickup(Collectible collectible)
    {
        _collectiblesRemaining.Remove(collectible);
        UpdateText();
        
        if (_collectiblesRemaining.Count == 0)
            OnCompleteEvent.Invoke();

        // For example If I want to Invoke a new UnityEvent when the FIRST collectible is collected,
        // I can make it like this below...
        if(_collectiblesRemaining.Count == _gatherables.Count -1)
        {
            OnPickedUpFirstEvent.Invoke();
        }
    }

    /// <summary>
    /// Show result on the screen, when you collect all the collectibles... 
    /// </summary>
    void UpdateText()
    {
        _text.SetText($"{_collectiblesRemaining.Count} more...");
        if (_collectiblesRemaining.Count == 0 || _collectiblesRemaining.Count == _gatherables.Count)
        {
            //_text.enabled = false;
            _text.SetText("You Win!!!");
        }
        else
            _text.enabled = true;
    }

    /// <summary>
    /// Unity Editor fill the list automatically with the objects of given name
    /// </summary>
    [ContextMenu("AutoFill Collectibles")]
    void AutoFillCollectibles()
    {
        _gatherables = GetComponentsInChildren<Collectible>()
            .Where(t=> t.name.ToLower().Contains("pick"))
            .ToList();
    }
}