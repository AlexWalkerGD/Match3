using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Class by Level Editor
    [Header("Level Editor")]
    public int level;
    public int timer;
    public int numberColumns;
    public int numberRows;

    //Objects to spawn on grid
    [Header("Grid")]
    public string[]      allSprites;
    public GameObject[]  allSquares;
    public GameObject    nullSquare;
    public GameObject    iceSquare;
    public GameObject    bombSquare;
    public GameObject    effectParticleSystem;
    public ParticleSystem particle;

    //Movement to Refreshing Grid
    [Header("Refresh Grid")]
    public  bool        isRefreshingGrid = false;

    //Swap squares
    [Header("Swap Squares")]
    public GameObject    firstSquareTouched;
    public GameObject    secondSquareTouched;
    public GameObject[,] squaresBidi;
    public float         speedSwap;
    public bool          isMoving = false;
    public bool          isSwap = false;
    private Vector3      tempPosition;
    private Vector3      tempPosition2;
    public int           numberMatchs = 0;


    //Match squares
    [Header("Match")]
    public bool    isMatching = false;

    //Settings UI
    [Header("UI")]
    public float    timerLevel;
    public Text     timerTxt;
    public int      countdown;
    public Text     countdownTxt;
    public int      countFinal;
    private ShowScore showScore;

    //AudioSource
    [Header("Audio Source")]
    public AudioSource  fxSource;
    public AudioClip    fxClique;
    public AudioClip    fxMatch;


    void Awake()
    {
        LoadLevel();
    }

    void Start()
    {
        showScore = FindObjectOfType(typeof(ShowScore)) as ShowScore;
        particle = GetComponent<ParticleSystem>();
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

        if (isMoving)
        {
            firstSquareTouched.transform.position = Vector3.MoveTowards(firstSquareTouched.transform.position, tempPosition2, speedSwap);
            secondSquareTouched.transform.position = Vector3.MoveTowards(secondSquareTouched.transform.position, tempPosition, speedSwap);

            if(firstSquareTouched.transform.position == tempPosition2)
            {
                if (secondSquareTouched.GetComponent<SpriteRenderer>().sprite.name == "whitSprite_0")
                {                   

                    BurstBomb((int)secondSquareTouched.transform.position.x);
                }

                if (firstSquareTouched.GetComponent<SpriteRenderer>().sprite.name == "whitSprite_0")
                {
                    BurstBomb((int)firstSquareTouched.transform.position.x);
                }
                isMoving = false;
            }
        }

        

       /* if (isRefreshingGrid)
        {
            if (!isMatching)
            {
                isMoving = true;
                firstSquareTouched.transform.position = Vector3.MoveTowards(firstSquareTouched.transform.position, tempPosition, speedSwap);
                secondSquareTouched.transform.position = Vector3.MoveTowards(secondSquareTouched.transform.position, tempPosition2, speedSwap);

                if (firstSquareTouched.transform.position == tempPosition)
                {
                    isRefreshingGrid = false;
                    isMoving = false;
                    isSwap = false;

                    firstSquareTouched = null;
                    secondSquareTouched = null;
                }
            }
        }*/

        ReadGrid();

        if (!isMoving && !isMatching)
        {
            Match();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null)
            {
                firstSquareTouched = hit.collider.gameObject;
                fxSource.PlayOneShot(fxClique);
            }
        }

     

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null && firstSquareTouched != null)
            {
                if (hit.collider.gameObject.transform.position.x == firstSquareTouched.transform.position.x - 1 && hit.collider.gameObject.transform.position.y == firstSquareTouched.transform.position.y)
                {
                    secondSquareTouched = hit.collider.gameObject;
                    fxSource.PlayOneShot(fxClique);
                    Swap();
                }
                else if (hit.collider.gameObject.transform.position.x == firstSquareTouched.transform.position.x + 1 && hit.collider.gameObject.transform.position.y == firstSquareTouched.transform.position.y)
                {
                    secondSquareTouched = hit.collider.gameObject;
                    fxSource.PlayOneShot(fxClique);
                    Swap();
                }
                else if (hit.collider.gameObject.transform.position.y == firstSquareTouched.transform.position.y - 1 && hit.collider.gameObject.transform.position.x == firstSquareTouched.transform.position.x)
                {
                    secondSquareTouched = hit.collider.gameObject;
                    fxSource.PlayOneShot(fxClique);
                    Swap();
                }
                else if (hit.collider.gameObject.transform.position.y == firstSquareTouched.transform.position.y + 1 && hit.collider.gameObject.transform.position.x == firstSquareTouched.transform.position.x)
                {
                    secondSquareTouched = hit.collider.gameObject;
                    fxSource.PlayOneShot(fxClique);
                    Swap();
                }
            }
        }
    }

    void BurstBomb(int X)
    {
        particle.Play();
        for (int row = 0; row < numberRows; row++)
        {
            Destroy(squaresBidi[X, row]);            
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
        if (!isSwap)
        {
            numberMatchs = 0;

            tempPosition = firstSquareTouched.transform.position;
            tempPosition2 = secondSquareTouched.transform.position;

            isMoving = true;             

            squaresBidi[(int)tempPosition.x, (int)tempPosition.y * -1] = secondSquareTouched;
            squaresBidi[(int)tempPosition2.x, (int)tempPosition2.y * -1] = firstSquareTouched;
        }
        
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
                                squaresBidi[column, row - 1].transform.position = new Vector3(column, -row,0);                          
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
                if (column > 0 && column < numberColumns - 1)
                {
                    if (squaresBidi[column - 1, row] != null && squaresBidi[column, row] != null && squaresBidi[column + 1, row] != null)
                    {
                        GameObject leftSquare = squaresBidi[column - 1, row];
                        GameObject rightSquare = squaresBidi[column + 1, row];

                        if (squaresBidi[column, row].GetComponent<SpriteRenderer>().sprite.name != "iceSprite_0")
                        {
                            if (squaresBidi[column, row].name == leftSquare.name && squaresBidi[column, row].name == rightSquare.name)
                            {
                                isMatching = true;
                                numberMatchs++;

                                if (numberMatchs > 1)
                                {
                                    showScore.ShowCanva(numberMatchs);
                                }

                                SquareFalling(leftSquare);
                                SquareFalling(squaresBidi[column, row]);
                                SquareFalling(rightSquare);

                                if (countdown >= countFinal)
                                {
                                    LoadScreens("ScreenWin");
                                }
                                else
                                {
                                    countdown += 3;
                                    countdownTxt.text = countdown.ToString();
                                    fxSource.PlayOneShot(fxMatch);
                                }

                                squaresBidi[column - 1, row] = null;
                                squaresBidi[column + 1, row] = null;

                                if (numberMatchs >= 3)
                                {
                                    GameObject newSquare = Instantiate(bombSquare, new Vector3(column, -row, 0), Quaternion.identity);
                                    squaresBidi[column, row] = newSquare;
                                }
                                else
                                {
                                    squaresBidi[column, row] = null;
                                }

                                isSwap = false;
                            }
                        }
                    }
                }

                //Vertical
                if (row > 0 && row < numberColumns - 1)
                {
                    if (squaresBidi[column, row - 1] != null && squaresBidi[column, row] != null && squaresBidi[column, row + 1] != null)
                    {
                        GameObject upSquare = squaresBidi[column, row - 1];
                        GameObject downSquare = squaresBidi[column, row + 1];

                        if (squaresBidi[column, row].GetComponent<SpriteRenderer>().sprite.name != "iceSprite_0")
                        {
                            if (squaresBidi[column, row].name == upSquare.name && squaresBidi[column, row].name == downSquare.name)
                            {
                                isMatching = true;
                                numberMatchs++;

                                SquareFalling(upSquare);
                                SquareFalling(squaresBidi[column, row]);
                                SquareFalling(downSquare);

                                if (countdown >= countFinal)
                                {
                                    LoadScreens("ScreenWin");
                                }
                                else
                                {
                                    countdown += 3;
                                    countdownTxt.text = countdown.ToString();
                                    fxSource.PlayOneShot(fxMatch);
                                }

                                squaresBidi[column, row - 1] = null;
                                squaresBidi[column, row + 1] = null;

                                if (numberMatchs >= 3)
                                {
                                    GameObject newSquare = Instantiate(bombSquare, new Vector3(column, -row, 0), Quaternion.identity);
                                    squaresBidi[column, row] = newSquare;
                                }
                                else
                                {
                                    squaresBidi[column, row] = null;
                                }

                                isSwap = false;
                            }
                        }

                    }
                }
            }

        }
    }

    void SquareFalling(GameObject name)
    {
        name.GetComponent<SpriteRenderer>().sortingLayerName = "SquaresFalling";
        name.GetComponent<Rigidbody2D>().isKinematic = false;
        name.transform.Find("win").gameObject.SetActive(true);        
    }

    void LoadScreens(string name)
    {
        SceneManager.LoadScene(name);
    }
}
