using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireApps.Data.Models;

public class Ship
{
    public string Name { get; set; }
    public int Size { get; set; }
    public List<(int Row, int Col)> Coordinates { get; set; }
    public bool IsSunk => Coordinates.All(c => c == (-1, -1)); // All parts are hit (-1, -1)

    public Ship(string name, int size)
    {
        Name = name;
        Size = size;
        Coordinates = new List<(int Row, int Col)>();
    }

    public void Hit(int row, int col)
    {
        for (int i = 0; i < Coordinates.Count; i++)
        {
            if (Coordinates[i] == (row, col))
            {
                Coordinates[i] = (-1, -1); // Mark as hit
            }
        }
    }
}
