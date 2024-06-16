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

    void Start()
    {
        SpawnPictureMesh(4, 5, StartPosition, _offset, false);
        MovePicture(4, 5, StartPosition, _offset);
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
