using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
    bool TryTransitionState<T>() where T : IState;
}