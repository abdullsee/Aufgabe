using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Aufgabe;

class Program
{
    static List<List<int>> lists = new List<List<int>>();
    static List<int>werte=new List<int>();
    static List<int> set_A = new List<int>();
    static List<int> set_V = new List<int>();
    static List<int> set_D = new List<int>();
    static void Main()
    {
        //abundante Zahl = 0 --> > zahl
        //vollkommene Zahl = 1 --> ==zahl
        //defiziente Zahl = 2 --> < zahl

        int n = 100;
        
        for (int i = 1; i <= n; i++)
        {
            werte.Add(i);
        }

        Worker w1 = new Worker(1, n / 4);
        Worker w2 = new Worker((n / 4) + 1, n / 2);
        Worker w3 = new Worker((n / 2) + 1, (3 * n) / 4);
        Worker w4 = new Worker(((3 * n) / 4) + 1, n);

        Thread t1 = new Thread(w1.getParallelLists);
        Thread t2 = new Thread(w2.getParallelLists);
        Thread t3 = new Thread(w3.getParallelLists);
        Thread t4 = new Thread(w4.getParallelLists);
        
        t1.Start();
        t2.Start();
        t3.Start();
        t4.Start();

        t1.Join();
        t2.Join();
        t3.Join();
        t4.Join();

        //hinzufügen der listen in die liste 
        lists.Add(set_A);
        lists.Add(set_V);
        lists.Add(set_D);

        
        //Testausgaben
        Console.WriteLine(string.Join(",", set_A));
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(string.Join(",", set_V));
        Console.WriteLine();
        Console.WriteLine();
        Console.Write(string.Join(",", set_D));

        /**
         * Linq ausdruck zum bestimmen aller Zahlen aus set_A, die durch 10 ohne Rest teilbar sind.
         * Außerdem wird eine neue Menge erzeugt. Siehe Ausgabe
         * Ausgabe mittels String Interpolation
         */
        var filter =
            from zahlen in set_A
            where (zahlen % 10 == 0)
            select new {nummer = zahlen, sum = berechneSum(zahlen)};

        foreach (var i in filter)
        {
            Console.WriteLine($"Ergebnis lautet:\n"+i);
        }


    }

/**
 * berechnet die summe aller teiler, die kleiner als zahl sind
 */
    public static int berechneSum(int zahl)
    {
        List<int> teiler = new List<int>();
        int sum = 0;
        
        //suchen der teiler
        for (int i = 1; i < zahl; i++)
        {
            if (zahl % i == 0)
            {
                teiler.Add(i);
            }
        }

        //berechnen der summe
        foreach (var i in teiler)
        {
            sum += i;
        }

        return sum;
    }

/**
 * bestimmt mittels der berechneten summe,
 * ob eine zahl A , V , D ist (siehe oben)
 */
    public static int checkZahl(int zahl)
    {

        int sum = berechneSum(zahl);

        //verlgleichen mit wert

        if (sum > zahl)
        {
            return 0;
        }
        else if (sum == zahl)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }


/**
 * benoetigte Klasse zum parallelen ausfuehren von Threads.
 * Hier wird dem Thread mittels der beiden Var. Von und Bis mitgeteilt welche Zahlen zu bearbeiten sind.
 * Sonst kann es zu fehlern kommen.
 */
    public class Worker
    {
        public Worker(int von, int bis)
        {
            Von = von;
            Bis = bis;
        }

        public int Von { get; set; }
        public int Bis { get; set; }

        /**
         * Mittels Mutex (Thread-Safe) werden hier 4 Threads parallel abgearbeitet (Lock wuerde auch gehen)
         * bestimmt 3 sets (set_A,set_V,set_D)
         * Hier stehen dann die zahlen drin, die zu einer gruppe gehoeren
         */
        public void getParallelLists()
        {
            int sum = 0;

            Mutex global_mutex = new Mutex();

            for (int i = Von; i < Bis; i++)
            {
                global_mutex.WaitOne(); // wait for entry
                sum = checkZahl(werte[i]);
                if (sum > 0)
                {
                    set_A.Add(werte[i]);
                }
                else if (sum == 0)
                {
                    set_V.Add(werte[i]);
                }
                else
                {
                    set_D.Add(werte[i]);
                }
                global_mutex.ReleaseMutex();
            }
        }
    }
}