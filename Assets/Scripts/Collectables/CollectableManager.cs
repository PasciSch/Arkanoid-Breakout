using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    #region Signleton
    private static CollectableManager _instance;

    public static CollectableManager Instance => _instance;

    private void Awake() {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public List<Collectable> AvailableBuffs;
    public List<Collectable> AvailableDebuffs;

    public List<Collectable> CurrentCollectables;

    [Range(0, 100)]
    public float BuffChance;

    [Range(0, 100)]
    public float DebuffChance;

    public void ClearRemainingCollectables()
    {
        foreach (Collectable collectable in this.CurrentCollectables.ToList())
        {
            Destroy(collectable.gameObject);
        }
        CurrentCollectables.Clear();
    }
}
