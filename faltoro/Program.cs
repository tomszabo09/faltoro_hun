using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace faltoro
{
    class Program
    {
        static Random r = new Random();
        static void GameRules()
        {
            Console.WriteLine("          Játékszabályok          ");
            Console.WriteLine("__________________________________\n");
            Console.WriteLine("A játékot két játékos játszhatja (egyik az építő, másik a romboló).");
            Console.WriteLine("A játéktér tetején véletlen színű téglák helyezkednek el.");
            Console.WriteLine("Alul egy-egy ágyú található, amelyet a játékosok a kurzormozgató billentyűkkel irányíthatnak jobbra-balra.");
            Console.WriteLine("Az ágyúgolyó egy véletlen színű tégla, amely kilövést követően a falhoz ütközik.");
            Console.WriteLine("Ha a lövedék azonos színű téglát talál el, akkor ez és az ezzel szomszédos, szintén ilyen színű téglák eltűnnek.");
            Console.WriteLine("Ha a lövedék más színű téglát talál el, akor a lövedék is beépül a falba.");
            Console.WriteLine("Az építő tud rombolni, illetve a romboló is tud építeni.");
            Console.WriteLine("A játékot az építő nyeri, ha a játéktér megtelik. Ha a téglák elfogynak, akkor a romboló nyer.");
            Console.WriteLine("__________________________________\n");
        }
        static void Keybinds()
        {
            Console.WriteLine("          Billentyűbeállítások          ");
            Console.WriteLine("________________________________________\n");
            Console.WriteLine("Jobboldali ágyú: ");
            Console.WriteLine("Jobbra lépés: '->' billentyű");
            Console.WriteLine("Balra lépés: '<-' billentyű");
            Console.WriteLine("Lövés: ENTER\n");
            Console.WriteLine("Baloldali ágyú: ");
            Console.WriteLine("Jobbra lépés: 'D' billentyű");
            Console.WriteLine("Balra lépés: 'A' billentyű");
            Console.WriteLine("Lövés: SZÓKÖZ");
            Console.WriteLine("________________________________________\n");
        }
        static void Menu(char[,] field, int[,] colors, ref int builder, ref int destroyer, ref int cannonballcolor)
        {
            Console.WriteLine("          Faltörő          ");
            Console.WriteLine("___________________________\n");
            Console.WriteLine("1: Játék indítása");
            Console.WriteLine("2: Játékszabályok");
            Console.WriteLine("3: Billentyűbeállítások");
            Console.WriteLine("___________________________\n");
            Console.WriteLine("ESC: Kilépés");
            ConsoleKey key = Console.ReadKey().Key;

            do
            {
                if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1)
                {
                    Console.Clear();
                    ShowField(field, colors, ref cannonballcolor);
                    Play(field, colors, ref builder, ref destroyer, ref cannonballcolor);
                }
                else if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2)
                {
                    Console.Clear();
                    GameRules();
                    Console.WriteLine("A visszalépéshez nyomjon le egy tetszőleges billentyűt!");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(field, colors, ref builder, ref destroyer, ref cannonballcolor);
                }
                else if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3)
                {
                    Console.Clear();
                    Keybinds();
                    Console.WriteLine("A visszalépéshez nyomjon le egy tetszőleges billentyűt!");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(field, colors, ref builder, ref destroyer, ref cannonballcolor);
                }
                else if (key != ConsoleKey.Escape)
                {
                    Console.Clear();
                    Menu(field, colors, ref builder, ref destroyer, ref cannonballcolor);
                }

            } while (key != ConsoleKey.Escape);

            Environment.Exit(0);
        }
        static void Main(string[] args)
        {
            int builder = 3;
            int destroyer = 7;
            int cannonballcolor = r.Next(3, 7);
            char[,] field = PlayingField();
            int[,] colors = GetColors(field);

            Console.OutputEncoding = Encoding.UTF8;

            Menu(field, colors, ref builder, ref destroyer, ref cannonballcolor);
        }
        static char[,] PlayingField()
        {
            char[,] field = new char[10, 11];

            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (i == 0 || i == 1 || i == 2)
                    {
                        field[i, j] = (char)9608;
                    }
                    field[field.GetLength(0) - 1, 3] = 'U';
                    field[field.GetLength(0) - 1, 7] = 'U';
                }
            }
            return field;
        }
        static void NextColor(int cannonballcolor)
        {
            Console.ForegroundColor = (ConsoleColor)cannonballcolor;
            Console.WriteLine((char)9608);
            Console.ResetColor();
        }
        static void ShowField(char[,] field, int[,] colors, ref int cannonballcolor)
        {
            Console.ResetColor();
            Console.WriteLine("___________");
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (colors[i, j] != 0)
                    {
                        Console.ForegroundColor = (ConsoleColor)colors[i, j];
                        Console.Write(field[i, j]);
                        Console.ResetColor();
                    }
                    else if (field[i, j] == 'U')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(field[i, j]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(field[i, j]);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("___________\n");            
            Console.Write("A következő lövedék színe: ");
            NextColor(cannonballcolor);
        }
        static int[,] GetColors(char[,] field)
        {
            int[,] colors = new int[10, 11];

            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j] == (char)9608)
                    {
                        int brickcolor = r.Next(3, 7);
                        colors[i, j] = brickcolor;
                    }
                    else
                    {
                        colors[i, j] = 0;
                    }
                }
            }
            return colors;
        }
        static void Movement(char[,] field, int[,] colors, ref int builder, ref int destroyer, ref int cannonballcolor, ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                Console.Clear();
                Console.WriteLine("Felhasználó általi kilépés miatt vége a játéknak! Új játék kezdéséhez indítsa újra a programot!\n");
                Console.WriteLine("Gomb lenyomására vége a programnak!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            if (key == ConsoleKey.D && builder != field.GetLength(1) - 1)
            {
                if (field[field.GetLength(0) - 1, builder + 1] != 'U')
                {
                    field[field.GetLength(0) - 1, builder] = ' ';
                    field[field.GetLength(0) - 1, builder + 1] = 'U';
                    builder++;
                    Console.Clear();
                    ShowField(field, colors, ref cannonballcolor);
                }
                else if (builder < field.GetLength(1) - 2)
                {
                    field[field.GetLength(0) - 1, builder] = ' ';
                    field[field.GetLength(0) - 1, builder + 2] = 'U';
                    builder = builder + 2;
                    Console.Clear();
                    ShowField(field, colors, ref cannonballcolor);
                }
            }
            if (key == ConsoleKey.A && builder != 0)
            {
                if (field[field.GetLength(0) - 1, builder - 1] != 'U')
                {
                    field[field.GetLength(0) - 1, builder] = ' ';
                    field[field.GetLength(0) - 1, builder - 1] = 'U';
                    builder--;
                    Console.Clear();
                    ShowField(field, colors, ref cannonballcolor);
                }
                else if (builder > 1)
                {
                    field[field.GetLength(0) - 1, builder] = ' ';
                    field[field.GetLength(0) - 1, builder - 2] = 'U';
                    builder = builder - 2;
                    Console.Clear();
                    ShowField(field, colors, ref cannonballcolor);
                }
            }
            if (key == ConsoleKey.RightArrow && destroyer != field.GetLength(1) - 1)
            {
                if (field[field.GetLength(0) - 1, destroyer + 1] != 'U')
                {
                    field[field.GetLength(0) - 1, destroyer] = ' ';
                    field[field.GetLength(0) - 1, destroyer + 1] = 'U';
                    destroyer++;
                    Console.Clear();
                    ShowField(field, colors, ref cannonballcolor);
                }
                else if (destroyer < field.GetLength(1) - 2)
                {
                    field[field.GetLength(0) - 1, destroyer] = ' ';
                    field[field.GetLength(0) - 1, destroyer + 2] = 'U';
                    destroyer = destroyer + 2;
                    Console.Clear();
                    ShowField(field, colors, ref cannonballcolor);
                }
            }
            if (key == ConsoleKey.LeftArrow && destroyer != 0)
            {
                if (field[field.GetLength(0) - 1, destroyer - 1] != 'U')
                {
                    field[field.GetLength(0) - 1, destroyer] = ' ';
                    field[field.GetLength(0) - 1, destroyer - 1] = 'U';
                    destroyer--;
                    Console.Clear();
                    ShowField(field, colors, ref cannonballcolor);
                }
                else if (destroyer > 1)
                {
                    field[field.GetLength(0) - 1, destroyer] = ' ';
                    field[field.GetLength(0) - 1, destroyer - 2] = 'U';
                    destroyer = destroyer - 2;
                    Console.Clear();
                    ShowField(field, colors, ref cannonballcolor);
                }
            }
        }
        static void BrickDestroyer(char[,] field, int[,] colors, int currentColor, int row, int X)
        {
            if (row >= 0 && colors[row, X] == currentColor && field[row, X] == (char)9608)
            {
                field[row, X] = ' ';
                field[row + 1, X] = ' ';
                int defaultrow = row;
                if (X != field.GetLength(1) - 1 && colors[row, X + 1] == currentColor && field[row, X + 1] == (char)9608)
                {
                    field[row, X + 1] = ' ';
                    while (field[row + 1, X + 1] == (char)9608)
                    {
                        int temp = colors[row + 1, X + 1];
                        field[row + 1, X + 1] = ' ';
                        field[row, X + 1] = (char)9608;
                        colors[row, X + 1] = temp;
                        row++;
                    }
                }
                row = defaultrow;
                if (X != 0 && colors[row, X - 1] == currentColor && field[row, X - 1] == (char)9608)
                {
                    field[row, X - 1] = ' ';
                    while (field[row + 1, X - 1] == (char)9608)
                    {
                        int temp = colors[row + 1, X - 1];
                        field[row + 1, X - 1] = ' ';
                        field[row, X - 1] = (char)9608;
                        colors[row, X - 1] = temp;
                        row++;
                    }
                }
                row = defaultrow;
                if (row != 0 && colors[row - 1, X] == currentColor && field[row - 1, X] == (char)9608)
                {
                    field[row - 1, X] = ' ';
                }
            }
        }
        static void ShootCannon(char[,] field, int[,] colors, ConsoleKey key, int builder, int destroyer, ref int cannonballcolor)
        {
            if (key == ConsoleKey.Enter && field[field.GetLength(0) - 3, destroyer] != (char)9608)
            {
                int row = field.GetLength(0) - 3;
                while (row >= 0 && field[row, destroyer] != (char)9608)
                {
                    colors[row, destroyer] = cannonballcolor;
                    field[row, destroyer] = (char)9608;
                    field[row + 1, destroyer] = ' ';
                    row--;
                    Console.ForegroundColor = (ConsoleColor)cannonballcolor;
                }
                BrickDestroyer(field, colors, cannonballcolor, row, destroyer);
                cannonballcolor = r.Next(3, 7);
                Console.Clear();
                ShowField(field, colors, ref cannonballcolor);
            }
            else if (key == ConsoleKey.Spacebar && field[field.GetLength(0) - 3, builder] != (char)9608)
            {
                int row = field.GetLength(0) - 3;
                while (row >= 0 && field[row, builder] != (char)9608)
                {
                    colors[row, builder] = cannonballcolor;
                    field[row, builder] = (char)9608;
                    field[row + 1, builder] = ' ';
                    row--;
                    Console.ForegroundColor = (ConsoleColor)cannonballcolor;
                }
                BrickDestroyer(field, colors, cannonballcolor, row, builder);
                cannonballcolor = r.Next(3, 7);
                Console.Clear();
                ShowField(field, colors, ref cannonballcolor);
            }
        }
        static void Play(char[,] field, int[,] colors, ref int builder, ref int destroyer, ref int cannonColor)
        {
            bool builderwins = false;
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    while (field[field.GetLength(0) - 3, j] != (char)9608 && field[0, j] != ' ')
                    {
                        ConsoleKey key = Console.ReadKey().Key;
                        Movement(field, colors, ref builder, ref destroyer, ref cannonColor, key);
                        ShootCannon(field, colors, key, builder, destroyer, ref cannonColor);
                    }
                    if (field[field.GetLength(0) - 3, j] == (char)9608)
                    {
                        builderwins = true;
                    }
                    else if (field[0, j] == ' ')
                    {
                        builderwins = false;
                    }
                }
            }
            Console.Clear();
            if (builderwins == true)
            {
                Console.WriteLine("A játéktér betelt! Az építő nyert!");
                Console.WriteLine();
                Console.WriteLine("A kilépéshez nyomjon le egy tetszőleges billentyűt!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("A téglák elfogytak! A romboló nyert!");
                Console.WriteLine();
                Console.WriteLine("A kilépéshez nyomjon le egy tetszőleges billentyűt!");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
