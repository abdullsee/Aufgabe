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
    static void Main()
    {
        //abundante Zahl = 0 --> > zahl
        //vollkommene Zahl = 1 --> ==zahl
        //defiziente Zahl = 2 --> < zahl

        int temp = 12;

        int zahl = checkZahl(temp);
        Console.WriteLine(zahl);
        
        Console.WriteLine("---------------------");

        //Aufgabe 2
        //befuellen mit 100 zahlen
        List<int> werte = new List<int>();
        for (int i = 1; i < 40; i++)
        {
            werte.Add(i);
        }

        List<List<int>> lists = new List<List<int>>();
       lists=getParallelLists(werte);
       
       foreach (var i in lists)
       {
           Console.WriteLine(string.Join(", ", i));
       }

       

    }

    public static int checkZahl(int zahl)
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


    public static List<List<int>> getParallelLists(List<int> werte)
    {
        Mutex global_mutex = new Mutex();
        
        List<List<int>> lists = new List<List<int>>();
        
        List<int> set_A = new List<int>();
        List<int> set_V = new List<int>();
        List<int> set_D = new List<int>();

        int sum = 0;
        
        foreach (var i in werte)
        {
            sum=checkZahl(i);
            if (sum > 0)
            {
                set_A.Add(i);
                sum = 0;
            }
            else if (sum == 0)
            {
                set_V.Add(i);
                sum = 0;
            }
            else
            {
                set_D.Add(i);
                sum = 0;
            }
        }

        lists.Add(set_A);
        lists.Add(set_V);
        lists.Add(set_D);
        return lists;
    }
}
