using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Class by Level Editor
    public int level;
    public int timer;
    public int numberColumns;
    public int numberRows;

    //Objects to spawn on grid
    public string[]      allSprites;
    public GameObject[]  allSquares;
    public GameObject    nullSquare;
    public GameObject    iceSquare;

    //Swap of squares
    public GameObject    firstSquareTouched;
    public GameObject    secondSquareTouched;
    public GameObject[,] squaresBidi;

    //Settings UI
    public float timerLevel;
    public Text timerTxt;
    public int countdown;
    public Text countdownTxt;
    public int countFinal;


    private void Awake()
    {
        LoadLevel();
    }

    void Start()
    {
        CreateGrid();
        timerLevel = timer;
    }

    void Update()
    {
        if (timerLevel <= 0)
        {
            LoadScreens("ScreenGameOver");
        }
        else
        {
            timerLevel -= +Time.deltaTime;
            timerTxt.text = timerLevel.ToString("0");
        }   

        ReadGrid();
        Match();

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null)
            {
                firstSquareTouched = hit.collider.gameObject;
               // Destroy(hit.collider.gameObject);
            }
        }

     

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null)
            {
                 secondSquareTouched = hit.collider.gameObject;
                 Swap();
            }
        }
    }

    void LoadLevel()
    {
        Level levelClass = new Level();

        var jsonTextFile = Resources.Load<TextAsset>("Levels/" + level) as TextAsset;

        if (jsonTextFile)
        {
            JsonUtility.FromJsonOverwrite(jsonTextFile.ToString(), levelClass);

            timer = levelClass.time;
            numberRows = levelClass.row;
            numberColumns = levelClass.column;
            allSprites = levelClass.squares;
        }
    }

    void CreateGrid()
    {
        squaresBidi = new GameObject[numberColumns,numberRows];

        for(int column = 0; column < numberColumns; column++)
        {
            for(int row = 0; row < numberRows; row++)
            {
                if (allSprites[row * numberColumns + column] == "randomSprite")
                {
                    GameObject newSquare = Instantiate(allSquares[Random.Range(0,allSquares.Length)], new Vector3(column, -row, 0), Quaternion.identity);
                    squaresBidi[column, row] = newSquare;
                }
                if (allSprites[row * numberColumns + column] == "redSprite")
                {
                    GameObject newSquare = Instantiate(allSquares[0], new Vector3(column, -row, 0), Quaternion.identity);
                    squaresBidi[column, row] = newSquare;
                }
                if (allSprites[row * numberColumns + column] == "yellowSprite")
                {
                    GameObject newSquare = Instantiate(allSquares[1], new Vector3(column, -row, 0), Quaternion.identity);
                    squaresBidi[column, row] = newSquare;
                }
                if (allSprites[row * numberColumns + column] == "pinkSprite")
                {
                    GameObject newSquare = Instantiate(allSquares[2], new Vector3(column, -row, 0), Quaternion.identity);
                    squaresBidi[column, row] = newSquare;
                }
                if (allSprites[row * numberColumns + column] == "purpleSprite")
                {
                    GameObject newSquare = Instantiate(allSquares[3], new Vector3(column, -row, 0), Quaternion.identity);
                    squaresBidi[column, row] = newSquare;
                }
                if (allSprites[row * numberColumns + column] == "blueSprite")
                {
                    GameObject newSquare = Instantiate(allSquares[4], new Vector3(column, -row, 0), Quaternion.identity);
                    squaresBidi[column, row] = newSquare;
                }
                if (allSprites[row * numberColumns + column] == "greenSprite")
                {
                    GameObject newSquare = Instantiate(allSquares[5], new Vector3(column, -row, 0), Quaternion.identity);
                    squaresBidi[column, row] = newSquare;
                }
                if (allSprites[row * numberColumns + column] == "iceSprite")
                {
                    GameObject newSquare = Instantiate(iceSquare, new Vector3(column, -row, 0), Quaternion.identity);
                    squaresBidi[column, row] = newSquare;
                }
                if (allSprites[row * numberColumns + column] != null)
                {
                    GameObject newSquare = Instantiate(nullSquare, new Vector3(column, -row, 0), Quaternion.identity);
                }

            }
        }
    }

    void Swap()
    {
        Vector3 tempPosition = firstSquareTouched.transform.position;
        Vector3 tempPosition2 = secondSquareTouched.transform.position;

        firstSquareTouched.transform.position = secondSquareTouched.transform.position;
        secondSquareTouched.transform.position = tempPosition;

        squaresBidi[(int)tempPosition.x, (int)tempPosition.y * -1] = secondSquareTouched;
        squaresBidi[(int)tempPosition2.x, (int)tempPosition2.y * -1] = firstSquareTouched;

        firstSquareTouched = null;
        secondSquareTouched = null;

        Match();

    }

    void ReadGrid()
    {
        for (int column = 0; column < numberColumns; column++)
        {
            for (int row = 0; row < numberRows; row++)
            {
                if (squaresBidi[column, row] == null)
                {
                    if(row == 0)
                    {
                        GameObject newSquare = Instantiate(allSquares[Random.Range(0, allSquares.Length)], new Vector3(column, -row, 0), Quaternion.identity);
                        squaresBidi[column, row] = newSquare;
                    }
                }

                if (squaresBidi[column, row] == null)
                {
                    if(row >= 1)
                    {
                        if (squaresBidi[column, row - 1] != null)
                        {
                            if(squaresBidi[column, row - 1].GetComponent<SpriteRenderer>().sprite.name != "iceSprite_0")
                            {
                                squaresBidi[column, row - 1].transform.position = new Vector3(column, -row, 0);
                                squaresBidi[column, row] = squaresBidi[column, row - 1];
                                squaresBidi[column, row - 1] = null;
                            }
                        }
                    }                   
                }

                if (squaresBidi[column, row] == null)
                {
                    if (row >= 1)
                    {
                        if (squaresBidi[column, row - 1] != null)
                        {
                            if (squaresBidi[column, row - 1].GetComponent<SpriteRenderer>().sprite.name == "iceSprite_0")
                            {
                                if (column < numberColumns - 1)
                                {
                                    if (squaresBidi[column + 1, row - 1] != null)
                                    {
                                        if (squaresBidi[column + 1, row - 1].GetComponent<SpriteRenderer>().sprite.name != "iceSprite_0")
                                        {
                                            squaresBidi[column + 1, row - 1].transform.position = new Vector3(column, -row, 0);
                                            squaresBidi[column, row] = squaresBidi[column + 1, row - 1];
                                            squaresBidi[column + 1, row - 1] = null;
                                        }
                                    }                                   
                                }

                            }
                        }
                    }                    
                }

                if (squaresBidi[column, row] == null)
                {
                    if (row >= 1)
                    {
                        if (squaresBidi[column, row - 1] != null)
                        {
                            if (squaresBidi[column, row - 1].GetComponent<SpriteRenderer>().sprite.name == "iceSprite_0")
                            {
                                if (column > 0)
                                {
                                    if (squaresBidi[column - 1, row - 1] != null)
                                    {
                                        if (squaresBidi[column - 1, row - 1].GetComponent<SpriteRenderer>().sprite.name != "iceSprite_0")
                                        {
                                            squaresBidi[column - 1, row - 1].transform.position = new Vector3(column, -row, 0);
                                            squaresBidi[column, row] = squaresBidi[column - 1, row - 1];
                                            squaresBidi[column - 1, row - 1] = null;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
    }

    void Match()
    {
        for (int column = 0; column < numberColumns; column++)
        {
            for (int row = 0; row < numberRows; row++)
            {
                //Horizontal
                if(column > 0 && column < numberColumns - 1)
                {
                    if (squaresBidi[column -1, row] != null && squaresBidi[column, row] != null && squaresBidi[column + 1, row] != null)
                    {
                        GameObject leftSquare = squaresBidi[column - 1, row];
                        GameObject rightSquare = squaresBidi[column + 1, row];

                        if (squaresBidi[column, row].GetComponent<SpriteRenderer>().sprite.name != "iceSprite_0")
                        {
                            if (squaresBidi[column, row].name == leftSquare.name && squaresBidi[column, row].name == rightSquare.name)
                            {
                                leftSquare.GetComponent<Rigidbody2D>().isKinematic = false;
                                squaresBidi[column, row].GetComponent<Rigidbody2D>().isKinematic = false;
                                rightSquare.GetComponent<Rigidbody2D>().isKinematic = false;

                                squaresBidi[column - 1, row] = null;
                                squaresBidi[column, row] = null;
                                squaresBidi[column +1, row] = null;

                                if (countdown >= countFinal )
                                {
                                    LoadScreens("ScreenWin");
                                }
                                else
                                {
                                    countdown += 3;
                                    countdownTxt.text = countdown.ToString();
                                }                          
                            }
                        }                       
                    }                   
                }

                //Vertical
                if (row > 0 && row < numberColumns - 1)
                {
                    if (squaresBidi[column, row -1] != null && squaresBidi[column, row] != null && squaresBidi[column, row + 1] != null)
                    {
                        GameObject upSquare = squaresBidi[column, row - 1];
                        GameObject downSquare = squaresBidi[column, row + 1];

                        if (squaresBidi[column, row].GetComponent<SpriteRenderer>().sprite.name != "iceSprite_0")
                        {
                            if (squaresBidi[column, row].name == upSquare.name && squaresBidi[column, row].name == downSquare.name)
                            {
                                upSquare.GetComponent<Rigidbody2D>().isKinematic = false;
                                squaresBidi[column, row].GetComponent<Rigidbody2D>().isKinematic = false;
                                downSquare.GetComponent<Rigidbody2D>().isKinematic = false;

                                squaresBidi[column, row - 1] = null;
                                squaresBidi[column, row] = null;
                                squaresBidi[column, row + 1] = null;

                                countdown += 3;
                                countdownTxt.text = countdown.ToString();
                            }
                        }
                                               
                    }                   
                }
            }
        }       
    }

    void LoadScreens(string name)
    {
        SceneManager.LoadScene(name);
    }
}
