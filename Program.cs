using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

// See https://aka.ms/new-console-template for more information
Console.OutputEncoding = Encoding.UTF8;
// Console.OutputEncoding = System.Text.Encoding.UTF8;


while (true)
{
    // check if needed direc exists
    if (Directory.Exists("codesuccess") == false)
    {
        Directory.CreateDirectory("codesuccess");
    }
    // the output
    HashSet<string> output = new HashSet<string>();

    // gets old strings
    if (File.Exists("codesuccess\\.netOutput.txt"))
    {
        Console.WriteLine("Reading Old Codes . . .");
        string[] old = File.ReadAllText("codesuccess\\.netOutput.txt").Split(" "); ;
        foreach (string x in old)
        {
            output.Add(x);
        }
    }
    int olenght = output.Count();


    string charcodes = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZæÆøØåÅüÜöÖäÄßĖĘĚĔƏÈÉÊËĒėęěĕəèéêëēŔŘŕřÞŤȚŢþťțţÝýŲŰŮŪÛÚÙųűůūûúùİĮĪÏÎÍÌıįīïîíìŒŐÕÔÓÒœőõôóòĀĂĄÀÁÂÃāăąàáâã§ŚŠŞśšşĎĐďđĢĞģğĶķŁĽĻĹłľļĺŹŻŽźżžÇĆČçćčŇŅŃÑňņńñ0123456789!@\"#$/\\{}[]()?¨+'*-_.:,;<>%=×÷€£¥₩٪^—–‐|~`´&µςερτυθιοπασδφγηξκλζχψωβν√©∞°ð‖‰…⁞Ω∑∆≈∩∫∝≪≫∓≅∪℃∇∴ϵ⋮≉≊≋≌≍≡≠≢≣≤≥≦≧≨≩≭≮≯≰≱≲≳≴≵≶≷≸≹⊂⊃⊄⊅⊆⊇⊈⊉⊊⊋⊌⊍⊎⋘⋙⋚⋛⋜⋝⋂⋃⋕⋇⋈⋉⋊⋋⋌≁≂≃≄≆≇¶®¦¿±™∵∶∷⩂⩃";
    byte[] bytes = Encoding.Default.GetBytes(charcodes);
    charcodes = Encoding.UTF8.GetString(bytes);

    var rand = new Random();
    int codelen = 3; // length of returned strings
    if (!File.Exists("codelen.txt"))
    {
        File.WriteAllText("codelen.txt", "3");
    }
    int amount = 0; // amount of strings to return - prep

    bool trycatch = true;
    while (trycatch)
    {
        try
        {
            codelen = Convert.ToInt32(File.ReadAllText("codelen.txt"));
            Console.WriteLine("old len: " + olenght);
            Console.WriteLine("codelen: " + codelen);
            Console.WriteLine("maxcodes:" + (Math.Pow(charcodes.Length, codelen) - olenght));
            Console.WriteLine("How many codes?: ");
            amount = Convert.ToInt32(Console.ReadLine());
            if (!File.Exists("codesuccess\\.netOutput.txt"))
            {
                output.Clear();
                olenght = 0;
            }
            if (amount + olenght > Math.Pow(charcodes.Length, codelen))
            {
                throw new IndexOutOfRangeException();
            }
            trycatch = false;
        }
        catch (FormatException e)
        {
            Console.WriteLine("Invalid Input: Not An INTEGER");
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine("Total Is Out Of Range. You Tried: " + amount + "\nWhich Is " + (amount + olenght - Math.Pow(charcodes.Length, codelen)) +
                " Above the Available Amount of Unique Combinations Possible");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Larger Than INT32 - Try A Valid INT32 Number");
        }
    }

    DateTime startTime = DateTime.Now;
    if (amount == -1)
    {
        List<int> ints = new List<int>();
        for (int i = 0; i < codelen; i++)
        {
            ints.Add(0);
        }
        int progress = 0;
        while (!ints.All(a => a >= charcodes.Length - 1))
        {
            progress++;
            string temp = "";
            ints[ints.Count() - 1]++;
            if (ints.Any(x => x > charcodes.Length - 1))
            {
                for (int x = ints.Count() - 1; x >= 0; x--)
                {
                    if (ints[x] > charcodes.Length - 1)
                    {
                        if (!ints.All(a => a > charcodes.Length - 1))
                        {
                            ints[x] = 0;
                        }
                        if (x - 1 >= 0 && x - 1 <= ints.Count())
                        {
                            ints[x - 1]++;
                        }
                    }
                }
            }
            foreach (int x in ints)
            {
                temp += charcodes[x];
            }
            output.Add(temp);
            if (progress % Math.Round(Math.Pow(charcodes.Length, codelen)/100) == 0)
            {
                Console.WriteLine("Progress: " + progress + " | " + Math.Pow(charcodes.Length, codelen) + " -- " + (Math.Round((progress/Math.Pow(charcodes.Length, codelen)) * 100)) + "% -- "+(DateTime.Now - startTime).ToString());
            }

        }
    }
    else
    {
        while (output.Count() - olenght < amount && output.Count() < Math.Pow(charcodes.Length, codelen))
        {
            string temp = "";
            for (int y = codelen; y > 0; y--)
            {
                temp += charcodes[rand.Next(0, charcodes.Length - 1)];
            }
            output.Add(temp);
        }
    }




    Console.WriteLine("------\n" + "Finish time: " + (DateTime.Now - startTime).ToString() +
        "\nAmount Requested: " + amount);
    Console.WriteLine("Requested: " + amount.ToString() + " at: " + (DateTime.Now).ToString());


    string stringout = String.Join(" ", output);
    Console.WriteLine("string manipulation done: writing to file now");

    File.WriteAllText("codesuccess\\.netOutput.txt", stringout);
    File.AppendAllText("codesuccess\\.netRequestList.txt", "Requested: " + amount.ToString() +
        " at: " + (DateTime.Now).ToString() + "\n");
    Console.WriteLine("Complete\n-----------");
}



public static class EnumExtension
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
       => self.Select((item, index) => (item, index));
}