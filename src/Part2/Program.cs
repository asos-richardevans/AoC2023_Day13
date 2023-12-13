var input = File.ReadAllLines("./input.txt");
var grids = new List<Dictionary<(int, int), string>>();
Dictionary<(int, int), string> grid = null;
var rowCount = 0;

foreach (var line in input)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        rowCount = 0;
        grids.Add(grid);
        grid = null;
        continue;
    }
    if (grid == null) grid = new Dictionary<(int, int), string>();

    for (var i = 0; i < line.Length; i++)
    {
        grid.Add((i, rowCount), line[i].ToString());
    }
    rowCount++;
}
if (grid != null) grids.Add(grid);

var originalResults = new List<(int, int, Direction)>();
var gridCount = 1;

foreach (var g in grids)
{
    var result = GetGridResult(g, (0, 0, Direction.None));
    originalResults.Add(result);
    gridCount++;
}

var newResults = new List<(int, int, Direction)>();
gridCount = 1;

foreach (var g in grids)
{
    var gMaxX = g.Keys.Max(k => k.Item1);
    var gMaxY = g.Keys.Max(k => k.Item2);
    var gX = 0;
    var gY = 0;
    var originalResult = originalResults.First(x => x.Item1 == gridCount);

    while (true)
    {
        if (gX > gMaxX)
        {
            gX = 0;
            gY++;
            continue;
        }

        if (gY > gMaxY)
        {
            Console.WriteLine("End of Grid");
            break;
        }

        var newGrid = AlterGrid(g, gX, gY);
        var newResult = GetGridResult(newGrid, originalResult);
        if (newResult.Item2 != 0)
        {
            if (newResult.Item2 != originalResult.Item2 || newResult.Item3 != originalResult.Item3)
            {
                newResults.Add(newResult);
                break;
            }
        }

        gX++;
    }
    gridCount++;
}

var total = 0;

foreach (var r in newResults)
{
    if (r.Item3 == Direction.Horizontal)
    {
        total += (100 * r.Item2);
    }
    else
    {
        total += r.Item2;
    }
}

Console.WriteLine(total);

Dictionary<(int, int), string> AlterGrid(Dictionary<(int, int), string> g, int x, int y)
{
    var newGrid = new Dictionary<(int, int), string>(g);
    var originalValue = newGrid.First(z => z.Key.Item1 == x && z.Key.Item2 == y).Value;
    newGrid.Remove((x, y));
    newGrid.Add((x, y), originalValue == "#" ? "." : "#");
    return newGrid;
}

(int, int, Direction) GetGridResult(Dictionary<(int, int), string> g, (int, int, Direction) originalResult)
{
    var hLine = 0;

    for (var i = 0; i < g.Keys.Max(k => k.Item2); i++)
    {
        if (CheckRowsMirrored(g, i))
        {
            if (originalResult.Item3 == Direction.Horizontal && originalResult.Item2 == i + 1)
            {
                //Ignore original result
                continue;
            }
            hLine = i + 1;
            Console.WriteLine($"{gridCount} Found horizontal line at {hLine}");
            break;
        }
    }

    if (hLine == 0)
    {
        var vLine = 0;
        for (var i = 0; i < g.Keys.Max(k => k.Item1); i++)
        {
            if (CheckColumnsMirrored(g, i))
            {
                if (originalResult.Item3 == Direction.Vertical && originalResult.Item2 == i + 1)
                {
                    //Ignore original result
                    continue;
                }
                vLine = i + 1;
                Console.WriteLine($"{gridCount} Found vertical line at {vLine}");
                break;
            }
        }
        return (gridCount, vLine, Direction.Vertical);
    }

    return (gridCount, hLine, Direction.Horizontal);
}

bool CheckRowsMirrored(Dictionary<(int, int), string> grid, int startPoint)
{
    var upperOffset = 0;
    var lowerOffset = 1;
    while (true)
    {
        var upperRow = grid.Where(k => k.Key.Item2 == startPoint - upperOffset).ToList();
        var lowerRow = grid.Where(k => k.Key.Item2 == startPoint + lowerOffset).ToList();
        if (upperRow.Select(x => x.Value).SequenceEqual(lowerRow.Select(x => x.Value)))
        {
            upperOffset++;
            lowerOffset++;
        }
        else
        {
            return false;
        }

        if (startPoint - upperOffset < 0 || startPoint + lowerOffset > grid.Keys.Max(k => k.Item2))
        {
            return true;
        }
    }
}

bool CheckColumnsMirrored(Dictionary<(int, int), string> grid, int startPoint)
{
    var leftOffset = 0;
    var rightOffset = 1;
    while (true)
    {
        var leftColumn = grid.Where(k => k.Key.Item1 == startPoint - leftOffset).ToList();
        var rightColumn = grid.Where(k => k.Key.Item1 == startPoint + rightOffset).ToList();
        if (leftColumn.Select(x => x.Value).SequenceEqual(rightColumn.Select(x => x.Value)))
        {
            leftOffset++;
            rightOffset++;
        }
        else
        {
            return false;
        }

        if (startPoint - leftOffset < 0 || startPoint + rightOffset > grid.Keys.Max(k => k.Item1))
        {
            return true;
        }
    }
}

public enum Direction
{
    None,
    Horizontal,
    Vertical
}