using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance { get; private set; }

    public event EventHandler OnSelectedChanged;
    [SerializeField] private List<ConstructionType> placedConstructionList;
    private ConstructionType placedConstruction;
    private ConstructionType.Dir dir = ConstructionType.Dir.Down;

    [SerializeField] private GameObject player;
    private Inventory playerInventory;

    private Grid<GridObject> grid;
    private int currentConstructionIndex = 0;

    [SerializeField] private TextMeshProUGUI glueCostText;
    [SerializeField] private TextMeshProUGUI woodCostText;
    [SerializeField] private TextMeshProUGUI stoneCostText;

    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private TextMeshProUGUI buildingDescription;
    [SerializeField] private TextMeshProUGUI buildingAmount;

    private void Awake()
    {
        playerInventory = player.GetComponent<Inventory>();

        if (Instance != null)
        {
            Debug.LogError("Mais de uma instância de GridBuildingSystem encontrada!");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        int gridWidth = 15;
        int gridHeight = 15;
        float cellSize = 4f;

        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(-30, -30, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        placedConstruction = placedConstructionList[currentConstructionIndex];

        List<Vector2Int> initialOccupiedPositions = new List<Vector2Int>
    {
        new Vector2Int(7, 7),
        new Vector2Int(8, 7),
        new Vector2Int(7, 8),
        new Vector2Int(8, 8)
    };

        foreach (Vector2Int pos in initialOccupiedPositions)
        {
            var gridObject = grid.GetGridObject(pos.x, pos.y);
            gridObject.SetOccupied(true);
        }

        ResetPlacedConstruction();
        RefreshSelectedObjectType();
    }

    public class GridObject
    {
        private Grid<GridObject> grid;
        private int x;
        private int y;
        private PlacedObject placedObject;
        private bool isOccupied = false;

        public GridObject(Grid<GridObject> grid, int width, int height)
        {
            this.grid = grid;
            this.x = width;
            this.y = height;
        }

        public void SetTransform(PlacedObject placedObject)
        {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void SetOccupied(bool occupied)
        {
            isOccupied = occupied;
            grid.TriggerGridObjectChanged(x, y);
        }

        public PlacedObject GetConstruction()
        {
            return placedObject;
        }

        public void ClearTransform()
        {
            placedObject = null;
            isOccupied = false;
        }

        public bool CanBuild()
        {
            return !isOccupied && placedObject == null;
        }

        public override string ToString()
        {
            return x + ", " + y + "\n" + placedObject;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.GetXY(GetMouseWorldPosition(), out int x, out int y);
            List<Vector2Int> gridPositionList = placedConstruction.GetGridPositionList(new Vector2Int(x, y), dir);

            // Testa se é possível construir
            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (!canBuild)
            {
                Debug.Log("Já ocupado");
                return; // Se as células não estiverem livres, não prossegue
            }

            // Verifica o limite de construções antes de gastar recursos
            bool withinMaxLimit = placedConstruction.maxAmount == 0 || placedConstruction.placedAmount < placedConstruction.maxAmount;
            if (!withinMaxLimit)
            {
                Debug.Log("Limite de construções atingido");
                return; // Se o limite for atingido, não prossegue
            }

            // Testa se tem materiais suficientes
            bool hasResources = false;
            if (playerInventory.glue >= placedConstruction.glueCost &&
                playerInventory.wood >= placedConstruction.woodCost &&
                playerInventory.stone >= placedConstruction.stoneCost)
            {
                playerInventory.SpendResources(placedConstruction.glueCost, placedConstruction.woodCost, placedConstruction.stoneCost);
                hasResources = true;
            }

            if (!hasResources)
            {
                Debug.Log("Sem materiais suficientes");
                return;
            }

            Vector2Int rotationOffset = placedConstruction.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

            PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, y), dir, placedConstruction);

            placedConstruction.placedAmount++;
            RefreshSelectedObjectType();

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetTransform(placedObject);
            }
        }

        // Destruir Construções
        if (Input.GetMouseButtonDown(1))
        {
            GridObject gridObject = grid.GetGridObject(GetMouseWorldPosition());
            PlacedObject deletingConstruction = gridObject.GetConstruction();
            if (deletingConstruction != null)
            {
                deletingConstruction.DestroySelf();

                List<Vector2Int> gridPositionList = deletingConstruction.GetGridPositionList();
                Debug.Log(deletingConstruction.placedObjectType.nameString + " destruído");
                deletingConstruction.placedObjectType.placedAmount--;
                RefreshSelectedObjectType();
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).ClearTransform();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && placedConstruction.isRotatable)
        {
            dir = ConstructionType.GetNextDir(dir);
        }

        // Altera a construção selecionada com o scroll do mouse
        if (Input.mouseScrollDelta.y != 0)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                currentConstructionIndex++;
            }
            else
            {
                currentConstructionIndex--;
            }

            if (currentConstructionIndex >= placedConstructionList.Count)
            {
                currentConstructionIndex = 0;
            }
            else if (currentConstructionIndex < 0)
            {
                currentConstructionIndex = placedConstructionList.Count - 1;
            }

            placedConstruction = placedConstructionList[currentConstructionIndex];
            Debug.Log("Selected Construction: " + placedConstruction.nameString);
            RefreshSelectedObjectType();
        }
    }


    private void ResetPlacedConstruction()
    {
        foreach (ConstructionType construction in placedConstructionList)
        {
            construction.placedAmount = 0;
        }

    }

    private void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);

        glueCostText.text = "Glue: " + playerInventory.glue + " / " + placedConstruction.glueCost.ToString();
        woodCostText.text = "Wood: " + playerInventory.wood + " / " + placedConstruction.woodCost.ToString();
        stoneCostText.text = "Stone: " + playerInventory.stone + " / " + placedConstruction.stoneCost.ToString();
        buildingAmount.text = "Amount: " + placedConstruction.placedAmount + " / " + placedConstruction.maxAmount;

        buildingName.text = placedConstruction.nameString;
        buildingDescription.text = placedConstruction.descriptionString;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public Quaternion GetConstructionRotation()
    {
        if (placedConstruction != null)
        {
            return Quaternion.Euler(0, 0, -placedConstruction.GetRotationAngle(dir));
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public ConstructionType GetConstructionType()
    {
        return placedConstruction;
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        grid.GetXY(mousePosition, out int x, out int y);

        if (placedConstruction != null)
        {
            Vector2Int rotationOffset = placedConstruction.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }
}