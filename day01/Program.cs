// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Day 1!");

var input = File.ReadAllLines("data/input.txt");

var lista = new List<int>();
var listb = new List<int>();

foreach (var line in input)
{
    var split = line.Replace("   ", " ").Split();
    var a = int.Parse(split[0]);
    var b = int.Parse(split[1]);
    lista.Add(a);
    listb.Add(b);
}

lista.Sort();
listb.Sort();

var acc = 0;

for (var i = 0; i < lista.Count; i++)
{
    acc += int.Abs((lista[i] - listb[i]));
}

Console.WriteLine($"Part1: {acc}");

acc = 0;

lista.ForEach(a => {
    acc += a * listb.FindAll(b => b == a).Count() ;
});

Console.WriteLine($"Part2: {acc}");