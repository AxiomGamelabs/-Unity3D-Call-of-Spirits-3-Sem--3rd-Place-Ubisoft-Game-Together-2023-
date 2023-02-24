using System;
using UnityEngine;

public static class Actions
{
    public static Action OnOpenDoor;
    public static Action OnWorldSwitched;

    public static Action<GameObject> OnTimefieldEntered; //we pass the position where the player has to respawn when finished recording
    public static Action<GameObject, bool, int, Checkpoint.EncounterType> OnCheckpointEntered; //we pass the position where the player has to respawn after death, if we are visiting the cp for the first time, and if it is the first cp. We also say if it has red or white keys

    public static Action OnPlayerDeath;
    public static Action<int> OnGhostDeath;

    public static Action OnRestartFromLastCp;

    public static Action OnTimerReachedZero;

    public static Action<int> OnMemorizeActivated;
    public static Action<int> OnMemorizeDeactivated;

    public static Action OnDimensionshiftUnlocked;

    public static Action<int> OnRedActivationBtnEnter; //we communicate the id of the Btn in the encounter, so we know which element on the Hud has to be filled
    public static Action<int> OnRedActivationBtnExit;
    public static Action<int> OnWhiteKeyBtnEnter; //we communicate the id of the Btn in the encounter, so we know which element on the Hud has to be filled


    public static Action OnGhostsPlayed;
    public static Action OnLastGhostDead;

    public static Action<int> OnGhostPlayed;
    public static Action<int> OnGhostKilled;

    public static Action OnEncounterDoorIsOpen;
    public static Action OnEncounterDoorIsClosed;

    public static Action OnGameLoaded;

}
