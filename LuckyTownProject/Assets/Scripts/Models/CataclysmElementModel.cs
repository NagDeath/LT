using System.Collections.Generic;

[System.Serializable]
public class CataclysmElementModel
{
    public int x;
    public int z;
}

[System.Serializable]
public class CataclysmModel
{
    public List<CataclysmElementModel> wave;
}
