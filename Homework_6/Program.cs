using System;
using System.IO;
using System.IO.Compression;

namespace Homework_6
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\dir\file.txt"; //файл с числом
            string path2 = @"D:\dir\result.txt"; //файл хранения результата
            string path3 = @"D:\dir\result.zip"; //файл архива
            if (!double.TryParse(File.ReadAllText(path), out double num) || num < 1 || num > 1_000_000_000) //проверка что файл содержит число и не выходит за границы 
            {
                Console.WriteLine("Файл пуст или число выходит за границы");
                return;
            }
            double d = 0; //степень двойки для формирования групп

            Console.WriteLine("Разбить числа по группам и записать в файл? нажмете 'y'(англ.) для подтверждения.\n" +
                "Иначе только будет выведено количество групп.");
            char key = Console.ReadKey().KeyChar; //получение нажатой пользователем клавиши

            DateTime dt = DateTime.Now; //время начала обработки
            if (key == 'y') //запись в файл если пользователь нажал 'y'
            {
                StreamWriter sw = File.CreateText(path2); //открывается поток записи в файл
                for (double i = 1; i <= num; i++)
                {
                    if (i == Math.Pow(2, d)) //проверка является ли число степенью двойки
                    {
                        if (d == 0)
                        {
                            sw.Write($"Gr{d + 1}: {i}");
                        }
                        else
                        {
                            sw.WriteLine();
                            sw.Write($"Gr{d + 1}: {i} ");
                        }
                        d++;
                    }
                    else
                    {
                        sw.Write($"{i} ");
                    }
                }
                sw.Close();
                Console.WriteLine($"\nРасчёт окончен, количество групп: {d}. Запись в файл завершена");
            }
            else //расчёт количества групп по степени двойки без записи в файл
            {
                for (double i = 1; i <= num; i++)
                {
                    if (i == Math.Pow(2, d)) d++;
                }
                Console.WriteLine($"\nКоличество групп: {d}"); //при распределении по степеням двойки, количество групп= максимальному значению степени
            }
            TimeSpan ts = DateTime.Now - dt; //время вычисления
            Console.WriteLine($"Время расчёта: {ts}");
            if (key != 'y') return; //проверка если пользователь не выбирал запись в файл то дальнейшая архивация не нужна
            key = ' ';
            Console.WriteLine("\nПоместить файл в архив? нажмете 'y'(англ.) для подтверждения.");
            key = Console.ReadKey().KeyChar;
            if (key == 'y') //начала архивации если пользователь это выбрал
            {
                using (FileStream fsread = File.OpenRead(path2)) //считываем в поток из файла результата
                {
                    using (FileStream fswrite = File.OpenWrite(path3)) //записываем в файл архива
                    {
                        using (GZipStream gz = new GZipStream(fswrite, CompressionMode.Compress))
                        {
                            fsread.CopyTo(gz);
                        }
                    }
                }
                Console.WriteLine("Размер файла: {0} байт \nРазмер архива: {1} байт", //вывод статистики размеров файлов
                                            File.ReadAllBytes(path2).Length,
                                            File.ReadAllBytes(path3).Length);
            }

            Console.ReadKey();
        }
    }
}
