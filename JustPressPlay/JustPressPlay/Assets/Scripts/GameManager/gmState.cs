using UnityEngine;

public interface gmState
{
    //While we can assume that the GameManager is a singleton, we pass it in to avoid any dependency issues
    public string stateName { get; }
    abstract gmState DoState(GameManager gm);

    abstract void onEntrance();

    abstract gmState endConditions(GameManager gm);
}
