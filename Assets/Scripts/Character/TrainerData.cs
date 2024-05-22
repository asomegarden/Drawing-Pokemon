using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrainerData", menuName = "Pokemon/Create New Trainer Data")]
public class TrainerData : ScriptableObject
{
    public string trainerName;
    public Sprite sprite;
    public Sprite portrait;
}
