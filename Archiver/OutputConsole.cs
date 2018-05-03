﻿using System;
using System.Text;

namespace archiver
{
    /// <summary>
    /// Обеспечивает потокобезопасный вывод информации в консоль
    /// </summary>
    public static class OutputConsole
    {
        private static object lockObj = new object();
        private static void Green()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        private static void Gray()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        private static void Red()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        public static string ToString(byte[] hash)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var @byte in hash)
            {
                builder.Append(@byte.ToString("x2"));
            }

            return builder.ToString();
        }
        public static void DisplayErrorStack(Exception ex)
        {
            lock (lockObj)
            {
                Green();
                Console.Write("ОШИБКА: ");
                Gray();
                Console.Write(ex.Message + "\n\n");
                Green();
                Console.Write("Вывод стека: ");
                Gray();
                Console.Write(ex.StackTrace + "\n\n");
            }
        }
        public static void DisplayError(Exception ex)
        {
            lock (lockObj)
            {
                Green();
                Console.Write("ОШИБКА: ");
                Gray();
                Console.Write(ex.Message + "\n\n");
            }
        }
        public static void ExitMessage()
        {
            lock (lockObj)
            {
                Gray();
                Console.WriteLine();
                Console.WriteLine("Работа программы завершена.Для выхода нажмите любую клавишу");
            }
        }

        public static void StartMessage()
        {
            Console.WriteLine("Задать параметры работы программы можно в командной строке");
            Console.WriteLine("Синтаксис -i источник [-s размер]");
            Console.WriteLine("-i задает путь до входного файла,где [источник] - путь до файла");
            Console.WriteLine("-s задает размер блока в байтах");
            Console.WriteLine("Завершить программу досрочно можно комбинацией клавиш Ctrl+C");
            Green();
            Console.WriteLine("Для начала работы нажмите любую клавишу");
            Gray();
        }
        public static void ShowMessage(string s)
        {
            Console.WriteLine(s);
        }
    }
}

