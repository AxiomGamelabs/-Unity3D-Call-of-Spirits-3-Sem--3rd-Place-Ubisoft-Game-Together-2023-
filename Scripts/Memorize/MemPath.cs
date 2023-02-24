using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemPath : MonoBehaviour
{
    private int currentReplayIndex;
    [SerializeField] private ColorType color;




    private void FixedUpdate()
    {
        //we want to go through all recorded frames
        int nextIndex = currentReplayIndex + 1;

        switch(color)
        {
            case ColorType.BLUE:
                if (nextIndex < Recordings.instance.recordings[0].RecordedDatas.Count) //we don't want to go over the last recorded frame
                {
                    Move(nextIndex, 0);
                }
                break;
            case ColorType.ORANGE:
                if (nextIndex < Recordings.instance.recordings[1].RecordedDatas.Count) //we don't want to go over the last recorded frame
                {
                    Move(nextIndex, 1);
                }
                break;
        }
    }


    private void Move(int index, int ghostId)
    {
        currentReplayIndex = index;
        RecordedData recordData = Recordings.instance.recordings[ghostId].RecordedDatas[index];

        transform.position = recordData.position;
    }



    public enum ColorType
    {
        BLUE,
        ORANGE
    }
}
