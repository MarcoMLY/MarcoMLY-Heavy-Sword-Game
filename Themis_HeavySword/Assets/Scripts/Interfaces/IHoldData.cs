using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldData<T>
{
    public T Variable { get; }

    public void ChangeData(T newVariable);
}
