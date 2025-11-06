using System;
using UnityEngine;

public class Apple : MonoBehaviour, ICollectable
{
    public event Action OnCollect;

    public void Collect()
    {
        OnCollect?.Invoke();
    }
}

public interface ICollectable
{
    public void Collect();
}
