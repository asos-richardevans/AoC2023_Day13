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

var total = 0;
var gridCount = 1;

foreach (var g in grids)
{
    var hLine = 0;

    for (var i = 0; i < g.Keys.Max(k => k.Item2); i++)
    {
        if (CheckRowsMirrored(g, i))
        {
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
                vLine = i + 1;
                Console.WriteLine($"{gridCount} Found vertical line at {vLine}");
                break;
            }
        }
        total += vLine;
    }
    else
    {
        total += (100 * hLine);
    }

    gridCount++;
}

Console.WriteLine(total);

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