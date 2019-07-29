using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CataclysmScript
{
    public List<List<GameObject>> CalculateAOE(int x, int z, GameObject[,] matrix, List<CataclysmSettingsSO> cataclysmsSettings)
    {
        List<List<GameObject>> tempObjects = new List<List<GameObject>>();

        switch (GlobalContainer.GetInstance().Cataclysm)
        {
            case Cataclysm.Lightning:
                if (GlobalContainer.GetInstance().LevelSettings.Lightning > 0)
                {
                    cataclysmsSettings.FirstOrDefault(c => c.name == GlobalContainer.GetInstance().Cataclysm.ToString()).WavesSettings.ForEach(w =>
                    {
                        List<GameObject> tempWave = new List<GameObject>();
                        w.wave.ForEach(e =>
                        {
                            if (x + e.x >= 0 && x + e.x < matrix.GetLength(0) && z + e.z >= 0 && z + e.z < matrix.GetLength(1))
                                tempWave.Add(matrix[x + e.x, z + e.z]);
                        });
                        tempObjects.Add(tempWave);
                    });

                }
                break;
            case Cataclysm.Meteor:
                if (GlobalContainer.GetInstance().LevelSettings.Meteor > 0)
                {
                    cataclysmsSettings.FirstOrDefault(c => c.name == GlobalContainer.GetInstance().Cataclysm.ToString()).WavesSettings.ForEach(w =>
                    {
                        List<GameObject> tempWave = new List<GameObject>();
                        w.wave.ForEach(e =>
                        {
                            if (x + e.x >= 0 && x + e.x < matrix.GetLength(0) && z + e.z >= 0 && z + e.z < matrix.GetLength(1))
                                tempWave.Add(matrix[x + e.x, z + e.z]);
                        });
                        tempObjects.Add(tempWave);
                    });

                }
                break;
            case Cataclysm.Tornado:
                if (GlobalContainer.GetInstance().LevelSettings.Tornado > 0)
                {
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        List<GameObject> tempWave = new List<GameObject>();
                        if (i == 0)
                            tempWave.Add(matrix[x, z]);
                        else
                        {
                            if (x + i < matrix.GetLength(0))
                                tempWave.Add(matrix[x + i, z]);
                            if (x - i >= 0)
                                tempWave.Add(matrix[x - i, z]);
                            if (z + i < matrix.GetLength(1))
                                tempWave.Add(matrix[x, z + i]);
                            if (z - i >= 0)
                                tempWave.Add(matrix[x, z - i]);
                        }
                        tempObjects.Add(tempWave);
                    }

                }
                break;
            case Cataclysm.Earthquake:
                if (GlobalContainer.GetInstance().LevelSettings.Earthquake > 0)
                {
                    if (GlobalContainer.GetInstance().EarthquakeState == EarthquakeState.Vertical)
                    {
                        for (int i = 0; i < matrix.GetLength(1); i++)
                        {
                            List<GameObject> tempWave = new List<GameObject>();

                            tempWave.Add(matrix[x, i]);

                            if (x - 1 >= 0)
                                tempWave.Add(matrix[x - 1, i]);
                            else
                                tempWave.Add(matrix[x + 1, i]);

                            tempObjects.Add(tempWave);
                        }
                    }
                    if (GlobalContainer.GetInstance().EarthquakeState == EarthquakeState.Horizontal)
                    {
                        for (int i = 0; i < matrix.GetLength(0); i++)
                        {
                            List<GameObject> tempWave = new List<GameObject>();

                            tempWave.Add(matrix[i, z]);

                            if (z - 1 >= 0)
                                tempWave.Add(matrix[i, z - 1]);
                            else
                                tempWave.Add(matrix[i, z + 1]);

                            tempObjects.Add(tempWave);
                        }
                    }
                }
                break;
            default:
                break;
        }

        return tempObjects;
    }
}
