using UnityEngine;

// ========================
// Revision 2020.10.30
// ========================

namespace TheGame
{
    public class Figure : MonoBehaviour
    {
        // ========================================================================================
        public const int LINE_BLOCKS_COUNT = 4;
        public const int BLOCKS_COUNT = LINE_BLOCKS_COUNT * LINE_BLOCKS_COUNT;

        public static readonly Vector2Int[,] JLSTZ_OFFSET_DATA = new Vector2Int[5, 4]
                                             {{ new Vector2Int(0, 0),  new Vector2Int(0, 0),  new Vector2Int(0, 0),  new Vector2Int(0, 0) },
                                              { new Vector2Int(0, 0),  new Vector2Int(1, 0),  new Vector2Int(0, 0),  new Vector2Int(-1, 0) },
                                              { new Vector2Int(0, 0),  new Vector2Int(1, -1), new Vector2Int(0, 0),  new Vector2Int(-1, -1) },
                                              { new Vector2Int(0, 0),  new Vector2Int(0, 2),  new Vector2Int(0, 0),  new Vector2Int(0, 2) },
                                              { new Vector2Int(0, 0),  new Vector2Int(1, 2),  new Vector2Int(0, 0),  new Vector2Int(-1, 2) }};

        public static readonly Vector2Int[,] I_OFFSET_DATA = new Vector2Int[5, 4]
                                             //{{ new Vector2Int(0, 0),  new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1)  },
                                             {{ new Vector2Int(0, 0),  new Vector2Int(0, 0),  new Vector2Int(0, 0),  new Vector2Int(0, 0) },
                                              { new Vector2Int(-1, 0), new Vector2Int(0, 0),  new Vector2Int(1, 1),  new Vector2Int(0, 1) },
                                              { new Vector2Int(2, 0),  new Vector2Int(0, 0),  new Vector2Int(-2, 1), new Vector2Int(0, 1) },
                                              { new Vector2Int(-1, 0), new Vector2Int(0, 1),  new Vector2Int(1, 0),  new Vector2Int(0, -1) },
                                              { new Vector2Int(2, 0),  new Vector2Int(0, -2), new Vector2Int(-2, 0), new Vector2Int(0, 2) }};

        public static readonly Vector2Int[,] O_OFFSET_DATA = new Vector2Int[1, 4]
                                             //{{ new Vector2Int(0, 0),  new Vector2Int(0, -1), new Vector2Int(-1, -1),new Vector2Int(-1, 0) }};
                                             {{ new Vector2Int(0, 0),  new Vector2Int(0, 0), new Vector2Int(0, 0),new Vector2Int(0, 0) }};


        // ========================================================================================
        public FigureTypeKeys FigureType { get; private set; }
        public Vector2Int FigurePosition { get; private set; }
        public FigureBlock[] Blocks { get; private set; } = new FigureBlock[BLOCKS_COUNT];
        
        private int _rotationIndex;
        public int RotationIndex 
        {
            get => _rotationIndex;
            private set
            { 
                _rotationIndex = (4 + value % 4) % 4;
            } 
        }

        private int _rotMinX;
        private int _rotMaxX;

        private int _rotMinY;
        private int _rotMaxY;


        // ========================================================================================
        public void Clear(bool returnToPool = false)
        {
            for (var i = 0; i < Blocks.Length; i++)
            {
                if (Blocks[i] != null)
                {
                    if (returnToPool)
                        Blocks[i].ReturnToPool();

                    Blocks[i].transform.SetParent(null);
                    Blocks[i] = null;
                }
            }

            FigureType = FigureTypeKeys.None;
        }

        public void Generate(FigureTypeKeys fType, bool isShadow)
        {
            var fTemplate = GameDataLibrary.Instance[fType];
            var bPrefab = isShadow 
                                ? GameDataLibrary.Instance[FigureBlockColorKeys.White] 
                                : GameDataLibrary.Instance[fTemplate.BlocksColor];

            for (int x = 0; x < LINE_BLOCKS_COUNT; x++)
            {
                for (int y = 0; y < LINE_BLOCKS_COUNT; y++)
                {
                    var i = y * LINE_BLOCKS_COUNT + x;
                    if (fTemplate.DefaultBlocks[i])
                    {
                        var newBlock = MassSpawner.Instance.Spawn(bPrefab);
                        newBlock.transform.parent = transform;
                        newBlock.transform.localPosition = new Vector3(x, y, 0f);
                        Blocks[i] = newBlock;
                    }
                }
            }

            _rotationIndex = 0;

            var halfSize = Mathf.Max(fTemplate.VisualSize.x, fTemplate.VisualSize.y) / 2f;
            _rotMinX = (int)(fTemplate.RotationPivot.x - halfSize);
            _rotMaxX = (int)(fTemplate.RotationPivot.x + halfSize);
            _rotMinY = (int)(fTemplate.RotationPivot.y - halfSize);
            _rotMaxY = (int)(fTemplate.RotationPivot.y + halfSize);

            FigureType = fTemplate.FigureType;
        }

        public void MoveToPosition(Vector2Int position)
        {
            transform.localPosition = new Vector3(position.x, position.y, 0f);
            FigurePosition = position;
        }

        public void Rotate90()
        {
            Transpose();
            ReverseRows();

            RotationIndex += 1;
        }

        public void Rotate180()
        {
            ReverseRows();
            ReverseColumns();

            RotationIndex += 2;
        }

        public void Rotate270()
        {
            ReverseRows();
            Transpose();

            RotationIndex += 3;
        }

        private void Transpose()
        {
            var result = new FigureBlock[BLOCKS_COUNT];
            for (int x = _rotMinX; x < _rotMaxX; x++)
            {
                for (int y = _rotMinY; y < _rotMaxY; y++)
                {
                    var i1 = y * LINE_BLOCKS_COUNT + x;
                    var i2 = x * LINE_BLOCKS_COUNT + y;

                    result[i2] = Blocks[i1];
                    if (Blocks[i1] != null)
                        Blocks[i1].transform.localPosition = new Vector3(y, x, 0f);
                }
            }
            Blocks = result;
        }

        private void ReverseRows()
        {
            for (int x = _rotMinX; x < _rotMaxX; x++)
            {
                for (int y = _rotMinY; y < _rotMaxY / 2; y++)
                {
                    var i1 = y * LINE_BLOCKS_COUNT + x;
                    var i2 = (_rotMaxY - 1 - y) * LINE_BLOCKS_COUNT + x;

                    var tBlock = Blocks[i1];
                    Blocks[i1] = Blocks[i2];
                    if (Blocks[i1] != null)
                        Blocks[i1].transform.localPosition = new Vector3(x, y, 0f);
                    Blocks[i2] = tBlock;
                    if (Blocks[i2] != null)
                        Blocks[i2].transform.localPosition = new Vector3(x, _rotMaxY - 1 - y, 0f);
                }
            }
        }

        private void ReverseColumns()
        {
            for (int x = _rotMinX; x < _rotMaxX / 2; x++)
            {
                for (int y = _rotMinY; y < _rotMaxY; y++)
                {
                    var i1 = y * LINE_BLOCKS_COUNT + x;
                    var i2 = y * LINE_BLOCKS_COUNT + (_rotMaxX - 1 - x);

                    var tBlock = Blocks[i1];
                    Blocks[i1] = Blocks[i2];
                    if (Blocks[i1] != null)
                        Blocks[i1].transform.localPosition = new Vector3(x, y, 0f);
                    Blocks[i2] = tBlock;
                    if (Blocks[i2] != null)
                        Blocks[i2].transform.localPosition = new Vector3(_rotMaxX - 1 - x, y, 0f);
                }
            }
        }
    }
}