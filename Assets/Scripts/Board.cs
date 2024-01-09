using System.Data;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{

    public Tilemap tilemap {get; private set;}
    public Piece activePiece {get; private set;}
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10,20);
    public SoundController soundController;


    public int ClearedRowsCount { get; private set; }
    public static int TotalScore { get; private set; } = 0;


    public RectInt Bounds{
        get{
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2); 
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake(){
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        for (int i=0;i<this.tetrominoes.Length;i++){
            this.tetrominoes[i].Initialize();
        }
    }

    public void Start(){
        SpawnPiece();
    }
    public void SpawnPiece(){
        int random = Random.Range(0,this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];
        
        this.activePiece.Initalize(this, this.spawnPosition, data);
        
        if(IsValidPosition(this.activePiece,this.spawnPosition)){
            Set(this.activePiece);
        }
        else{
            GameOver();
        }


        int points = 0;
        if(ClearedRowsCount==1){
            points = 400;
        }
        else if(ClearedRowsCount==2){
            points=1000;
        }
        else if(ClearedRowsCount==3){
            points=3000;
        }else if(ClearedRowsCount==4){
            points = 12000;
        }
        else if(ClearedRowsCount>4){
            points = 12000 + (ClearedRowsCount-4)*3000;
        }
        TotalScore+=points;
        Debug.Log(TotalScore);
        ClearedRowsCount = 0;

    }

    private void GameOver(){
        this.tilemap.ClearAllTiles();
        TotalScore = 0;
    }

    public void Set(Piece piece){
        for (int i=0;i<piece.cells.Length;i++){
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece){
        for (int i=0;i<piece.cells.Length;i++){
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position){
        RectInt bounds = this.Bounds;
        for(int i=0;i<piece.cells.Length;i++){
            Vector3Int tilePosition = piece.cells[i]+ position;

            if (!bounds.Contains((Vector2Int)tilePosition)){
                return false;
            }
            if (this.tilemap.HasTile(tilePosition)){
                return false;
            }
        }
        return true;
    }


    public void ClearLines(){
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        ClearedRowsCount = 0;

        while(row < bounds.yMax){
            if(IsLineFull(row)){
                LineClear(row);
                ClearedRowsCount++;
            }else{
                row++;
            }
        }
        
        


    }

    private bool IsLineFull(int row){
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col<bounds.xMax; col++){
            Vector3Int position = new Vector3Int(col, row, 0);

            if(!this.tilemap.HasTile(position)){
                return false;
            }
        }
        return true;
    }

    private void LineClear(int row){

        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col<bounds.xMax;col++){
            Vector3Int position = new Vector3Int(col,row, 0);
            this.tilemap.SetTile(position,null);
        }
        while(row<bounds.yMax){
            for (int col = bounds.xMin; col<bounds.xMax; col++){
                Vector3Int position = new Vector3Int(col,row+1,0);
                TileBase above = this.tilemap.GetTile(position);
                
                position = new Vector3Int(col,row,0);
                this.tilemap.SetTile(position,above);
            }
            row++;
        }
        soundController.clearSound();
    }
}
