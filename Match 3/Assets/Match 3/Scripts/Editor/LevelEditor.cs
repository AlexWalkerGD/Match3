using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelEditor : EditorWindow
{
    //Level Editor
    int level;
    int time = 100;
    int numberRows = 9;
    int numberColumns = 9;

    //SpritesSquare
    GameObject redSquare, yellowSquare, blueSquare, pinkSquare, purpleSquare, greenSquare, iceSquare, nullSquare, randomSquare;
    Texture redTexture, yellowTexture, blueTexture, pinkTexture, purpleTexture, greenTexture, iceTexture, nullTexture, randomTexture;

    Texture[] allTextures;
    Texture currentTexture;
    public string[] squaresName;

    [MenuItem("Match3/Editor")]

    public static void ShowWindow(){
        LevelEditor.GetWindow(typeof(LevelEditor));
    }

    private void CreateGUI()
    {
        allTextures = new Texture[81];
        squaresName = new string[81];

        for(int i = 0; i < 81; i++)
        {
            allTextures[i] = null;
            squaresName[i] = "";

        }
    }

    private void OnGUI()
    {
        Menu();
        Level();
        TimeLevel();
        Row();
        Columns();
        Tools();
        Grid();
        GetTextures();
        LoadLevel();
    }

    void GetTextures()
    {
        redSquare = Resources.Load<GameObject>("Prefabs/RedSquare");
        redTexture = redSquare.GetComponent<SpriteRenderer>().sprite.texture;

        yellowSquare = Resources.Load<GameObject>("Prefabs/YellowSquare");
        yellowTexture = yellowSquare.GetComponent<SpriteRenderer>().sprite.texture;

        blueSquare = Resources.Load<GameObject>("Prefabs/BlueSquare");
        blueTexture = blueSquare.GetComponent<SpriteRenderer>().sprite.texture;

        pinkSquare = Resources.Load<GameObject>("Prefabs/PinkSquare");
        pinkTexture = pinkSquare.GetComponent<SpriteRenderer>().sprite.texture;

        purpleSquare = Resources.Load<GameObject>("Prefabs/PurpleSquare");
        purpleTexture = purpleSquare.GetComponent<SpriteRenderer>().sprite.texture;

        greenSquare = Resources.Load<GameObject>("Prefabs/GreenSquare");
        greenTexture = greenSquare.GetComponent<SpriteRenderer>().sprite.texture;

        iceSquare = Resources.Load<GameObject>("Prefabs/IceSquare");
        iceTexture = iceSquare.GetComponent<SpriteRenderer>().sprite.texture;

        randomSquare = Resources.Load<GameObject>("Prefabs/RandomSquare");
        randomTexture = randomSquare.GetComponent<SpriteRenderer>().sprite.texture;

    }

    void Menu()
    {
        GUILayout.Label("Menu", GUILayout.Width(200), GUILayout.Height(18));

        GUILayout.BeginHorizontal();

            if (GUILayout.Button("New", GUILayout.Width(39), GUILayout.Height(39)))
            {
                 ResetGrid();
            }
            if (GUILayout.Button("Save", GUILayout.Width(39), GUILayout.Height(39)))
            {
                Save();
            }
            if (GUILayout.Button("Load", GUILayout.Width(39), GUILayout.Height(39)))
            {
                LoadLevel();
            }

        GUILayout.EndHorizontal();
    }

    void Level()
    {
        GUILayout.Label("Level", GUILayout.Width(200), GUILayout.Height(18));

        GUILayout.BeginHorizontal();

            if (GUILayout.Button("<<<", GUILayout.Width(39), GUILayout.Height(39)))
            {
                if (level > 0)
                {
                    level--;
                }
            }
            if (GUILayout.Button(level.ToString(), GUILayout.Width(39), GUILayout.Height(39)))
            {

            }
            if (GUILayout.Button(">>>", GUILayout.Width(39), GUILayout.Height(39)))
            {
                level++;
            }

        GUILayout.EndHorizontal();
    }

    void TimeLevel()
    {
        GUILayout.Label("Time", GUILayout.Width(200), GUILayout.Height(18));

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("<<<", GUILayout.Width(39), GUILayout.Height(39)))
        {
            if (time > 0)
            {
                time -= 5;
            }
        }
        if (GUILayout.Button(time.ToString(), GUILayout.Width(39), GUILayout.Height(39)))
        {

        }
        if (GUILayout.Button(">>>", GUILayout.Width(39), GUILayout.Height(39)))
        {
            time += 5;

        }

        GUILayout.EndHorizontal();
    }

    void Row()
    {
        GUILayout.Label("Row", GUILayout.Width(200), GUILayout.Height(18));

        GUILayout.BeginHorizontal();

            if (GUILayout.Button("<<<", GUILayout.Width(39), GUILayout.Height(39)))
            {
                if (numberRows > 3)
                {
                     numberRows--;   
                }
            
            }
            if (GUILayout.Button(numberRows.ToString(), GUILayout.Width(39), GUILayout.Height(39)))
            {

            }
            if (GUILayout.Button(">>>", GUILayout.Width(39), GUILayout.Height(39)))
            {
                if(numberRows < 9)
                {
                    numberRows++;
                }
            }

        GUILayout.EndHorizontal();
    }

    void Columns()
    {
        GUILayout.Label("Columns", GUILayout.Width(200), GUILayout.Height(18));

        GUILayout.BeginHorizontal();

            if (GUILayout.Button("<<<", GUILayout.Width(39), GUILayout.Height(39)))
            {
                if (numberColumns > 3)
                {
                    numberColumns--;
                }   
             }
            if (GUILayout.Button(numberColumns.ToString(), GUILayout.Width(39), GUILayout.Height(39)))
            {

            }
            if (GUILayout.Button(">>>", GUILayout.Width(39), GUILayout.Height(39)))
            {
                if (numberColumns < 9)
                {
                    numberColumns++;
                }
            }

        GUILayout.EndHorizontal();
    }

    void Tools()
    {
        GUILayout.Label("Tools", GUILayout.Width(200), GUILayout.Height(18));

        GUILayout.BeginHorizontal();

            if (GUILayout.Button("Null", GUILayout.Width(39), GUILayout.Height(39)))
            {
                currentTexture = null;
            }
            if (GUILayout.Button(iceTexture, GUILayout.Width(39), GUILayout.Height(39)))
            {
                currentTexture = iceTexture;
            }
            if (GUILayout.Button(redTexture, GUILayout.Width(39), GUILayout.Height(39)))
            {
               currentTexture = redTexture;
            }
            if (GUILayout.Button(greenTexture, GUILayout.Width(39), GUILayout.Height(39)))
            {
                currentTexture = greenTexture;
            }
            if (GUILayout.Button(blueTexture, GUILayout.Width(39), GUILayout.Height(39)))
            {
                currentTexture = blueTexture;
            }
            if (GUILayout.Button(pinkTexture, GUILayout.Width(39), GUILayout.Height(39)))
            {
                currentTexture = pinkTexture;
            }
            if (GUILayout.Button(yellowTexture, GUILayout.Width(39), GUILayout.Height(39)))
            {
                currentTexture = yellowTexture;
            }
            if (GUILayout.Button(purpleTexture, GUILayout.Width(39), GUILayout.Height(39)))
            {
                currentTexture = purpleTexture;
            }
            if (GUILayout.Button(randomTexture, GUILayout.Width(39), GUILayout.Height(39)))
            {
                currentTexture = randomTexture;
            }

        GUILayout.EndHorizontal();
    }

    void Grid()
    {
        GUILayout.Label("Grid", GUILayout.Width(200), GUILayout.Height(18));

        for ( int Row = 0; Row < numberRows; Row++)
        {
            GUILayout.BeginHorizontal();
            for (int Column = 0; Column < numberColumns; Column++)
            {
                if (GUILayout.Button(allTextures[Row * numberColumns + Column], GUILayout.Width(39), GUILayout.Height(39)))
                {
                    allTextures[Row * numberColumns + Column] = currentTexture;
                    if (currentTexture != null)
                    {
                        squaresName[Row * numberColumns + Column] = currentTexture.name;
                    }                    
                }
            }
            GUILayout.EndHorizontal();
        }
    }

    void Save()
    {
        Level levelC = new Level();
        levelC.level = level;
        levelC.time = time;
        levelC.row = numberRows;
        levelC.column = numberColumns;

        levelC.squares = squaresName;

        string levelJson = JsonUtility.ToJson(levelC, true);
        File.WriteAllText("Assets/Match 3/Resources/Levels/" + level + ".json", levelJson);

    }

    void LoadLevel()
    {
        Level levelClass = new Level();

        var jsonTextFile = Resources.Load<TextAsset>("Levels/" + level) as TextAsset;

        if (jsonTextFile)
        {
            JsonUtility.FromJsonOverwrite(jsonTextFile.ToString(), levelClass);

            time = levelClass.time;
            numberRows = levelClass.row;
            numberColumns = levelClass.column;
            squaresName = levelClass.squares;

            for(int i = 0; i < 81; i++)
            {
                if (squaresName[i] == "redSprite"){
                    allTextures[i] = redTexture;
                }
                if (squaresName[i] == "blueSprite"){
                    allTextures[i] = blueTexture;
                }
                if (squaresName[i] == "greenSprite"){
                    allTextures[i] = greenTexture;
                }
                if (squaresName[i] == "yellowSprite"){
                    allTextures[i] = yellowTexture;
                }
                if (squaresName[i] == "pinkSprite"){
                    allTextures[i] = pinkTexture;
                }
                if (squaresName[i] == "purpleSprite"){
                    allTextures[i] = purpleTexture;
                }
                if (squaresName[i] == "iceSprite"){
                    allTextures[i] = iceTexture;
                }
                if (squaresName[i] == "redSprite"){
                    allTextures[i] = redTexture;
                }
                if (squaresName[i] == "randomSprite")
                {
                    allTextures[i] = randomTexture;
                }
                if (squaresName[i] == ""){
                    allTextures[i] = null;
                }
            }
        }
        else
        {
            Debug.Log("End");
        }
    }

    void ResetGrid()
    {
        time = 100;
        numberColumns = 9;
        numberRows = 9;
        for(int i = 0; i < 81; i++)
        {
            squaresName[i] = "";
            allTextures[i] = null;
        }
    }
}

