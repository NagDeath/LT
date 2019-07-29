using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICataclysm
{
    List<List<GameObject>> CalculateAOE(Cataclysm cataclysm, GameObject[,] matrix, int x, int z, List<CataclysmSettingsSO> cataclysmSettings = null);
}
