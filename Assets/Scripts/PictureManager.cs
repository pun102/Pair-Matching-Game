using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureManager : MonoBehaviour
{
    public Picture PictureClone; //Reference to the Picture prefab to be instantiated
    public Transform PicSquawnPosition; //Position where the pictures will initially be spawned
    public Vector2 StartPosition = new Vector2(0f, 0f); // Starting position for the first picture

    //List to hold all the instantiated pictures
    [HideInInspector]
    public List<Picture> PictureList;

    private Vector2 _offset = new Vector2(2.6f, 2.5f); //Offset value to determine the distance between pictures in the grid

    private List<Material> _materialList = new List<Material>();
    private List<string> _texturePathList = new List<string>();
    private Material _firstmaterial;
    private string _firstTexturePath;

    void Start()
    {
        LoadMaterials();
        SpawnPictureMesh(4, 5, StartPosition, _offset, false);
        MovePicture(4, 5, StartPosition, _offset);
    }
    private void LoadMaterials()
    {
        var materialFilePath = GameSettings.Instance.GetMaterialDirectoryName();
        var textureFilePath = GameSettings.Instance.GetTilesCategoryTextureDirectoryName();
        var pairNumber = (int)GameSettings.Instance.GetPairNumber();
        const string matBaseName = "Pic";
        var firstMaterialName = "Back";

        for(var index = 1;  index <= pairNumber; index++)
        {
            var currentFilePath = materialFilePath + matBaseName + index;
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
            _materialList.Add(mat);

            var currentTextureFilePath = textureFilePath + matBaseName + index;
            _texturePathList.Add(currentFilePath);
        }
        _firstTexturePath = textureFilePath + firstMaterialName;
        _firstmaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;
    }


    void Update()
    {

    }
    private void SpawnPictureMesh(int rows, int colums, Vector2 Pos, Vector2 offset, bool scaleDown)
    {
        for (int col = 0; col < colums; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var tempPicture = (Picture)Instantiate(PictureClone, PicSquawnPosition.position, PicSquawnPosition.transform.rotation);

                tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                PictureList.Add(tempPicture);
            }
        }
        ApplyTextures();
    }
    public void ApplyTextures()
    {
        var rndMatIndex = Random.Range(0, _materialList.Count);
        var AppliedTimes = new int[_materialList.Count];

        for(int i = 0; i < _materialList.Count; i++)
        {
            AppliedTimes[i] = 0;
        }
        foreach(var o in PictureList)
        {
            var randPrevious = rndMatIndex;
            var counter = 0;
            var forceMat = false;

            while (AppliedTimes[rndMatIndex] >= 2 || ((randPrevious == rndMatIndex) && !forceMat))
            {
                rndMatIndex = Random.Range(0, _materialList.Count);
                counter++;
                if(counter > 100)
                {
                    for(var j = 0; j < _materialList.Count; j++)
                    {
                        if (AppliedTimes[j] < 2)
                        {
                            rndMatIndex = j;
                            forceMat = true;
                        }
                    }
                    if(forceMat == false)
                        return;
                }
            }
            o.SetFirstMaterial(_firstmaterial, _firstTexturePath);
            o.ApplyFirstMaterial();
            o.SetSecondMaterial(_materialList[rndMatIndex], _texturePathList[rndMatIndex]);

 //           o.ApplySecondMaterial(); //test

            AppliedTimes[rndMatIndex] += 1;
            forceMat = false;
        }
    }
    private void MovePicture(int rows, int colums, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for(var col = 0; col < colums; col++)
        {
            for(int row = 0; row < rows; row++)
            {
                var targetPosition = new Vector3((pos.x + (offset.x * row)), (pos.y + (offset.y * col)), 0.0f);
                StartCoroutine(MoveToPosition(targetPosition, PictureList[index]));
                index++;
            }

        }
    }
    private IEnumerator MoveToPosition(Vector3 target, Picture obj)
    {
        var randomDis = 7;
        while(obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis * Time.deltaTime);
            yield return 0;
        }
    }
}
