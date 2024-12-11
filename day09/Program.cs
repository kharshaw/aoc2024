// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.VisualBasic;

Console.WriteLine("Hello, Day 9!");




var input = File.ReadAllLines("data/input.txt");

var condensed = input[0];

var id = 0;
var blocks = new List<int>();
var files = new List<(int, int)>();

for (int sizeIdx = 0, freeIdx = 1; sizeIdx < condensed.Length; sizeIdx += 2, freeIdx += 2)
{

    var size = int.Parse(condensed[sizeIdx].ToString());

    var free = freeIdx < condensed.Length ? int.Parse(condensed[freeIdx].ToString()) : 0;

    for (var i = 0; i < size; i++)
    {
        blocks.Add(id);
    }

    files.Add((id, size));

    for (var i = 0; i < free; i++)
    {
        blocks.Add(-1);
    }

    files.Add((-1, free));

    id++;
}

// compact blocks
for (var i = blocks.Count - 1; i > 0; i--)
{
    var move = blocks[i];

    if (move == -1) continue;

    blocks[i] = -1;

    for (var j = 0; j < blocks.Count; j++)
    {
        if (blocks[j] == -1) 
        {
            blocks[j] = move;
            break;
        }
    }
}

long checksum = 0;
for (var i = 0; i < blocks.Count; i++)
{
    if (blocks[i] == -1) continue;
    checksum += i * blocks[i];
}

Console.WriteLine($"Part1: checksum: {checksum}");


// part 2
// PrintFilesystem(files);

for (var fId = id - 1; fId > 0; fId--)
{
    var idx = files.FindIndex(x => x.Item1 == fId);
    var moveFile = files[idx];


    // Console.WriteLine($"Moving {moveFile.Item1} {moveFile.Item2}");
    // find free space
    for (var j = 0; j < files.Count; j++)
    {
        var freespace = files[j];

    
        // don't move past myself...
        if (fId == freespace.Item1) break;
   
        // if this isn't free space then skip
        if (freespace.Item1 != -1) continue;


        // do we have space for this file
        if (freespace.Item2 >= moveFile.Item2)
        {
            var free = freespace.Item2;
            
            // put file in free space
            files[j] = moveFile;
            
            // if we have remaining free space, add it
            if (free - moveFile.Item2 > 0)
            {
                files.Insert(j+1, (-1, free - moveFile.Item2));
                files[idx+1] = (-1, moveFile.Item2);
            }
            else
            {
                files[idx] = (-1, moveFile.Item2);
            }
            // PrintFilesystem(files);
            break;
        }        
    }
}

PrintFilesystem(files);

// 00...111...2...333.44.5555.6666.777.888899

// 00992111777.44.333....5555.6666.....8888..

checksum = 0;
var position = 0;

foreach (var file in files)
{
    
    for (var i = 0; i < file.Item2; i++)
    {
        if (file.Item1 != -1) checksum += position * file.Item1;
        position++;
    }

}

Console.WriteLine($"Part2: checksum: {checksum}");

static void PrintFilesystem(List<(int, int)> files)
{
    // print files
    foreach (var file in files)
    {
        for (var i = 0; i < file.Item2; i++)
        {
            Console.Write("{0}", file.Item1 == -1 ? "." : file.Item1);
        }
    }

    Console.WriteLine();
}