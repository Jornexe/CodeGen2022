using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

using IHost host = Host.CreateDefaultBuilder(args).Build();

// Ask the service provider for the configuration abstraction.
IConfiguration config = host.Services.GetRequiredService<IConfiguration>();


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
    int codelen = Convert.ToInt32(config["options:codelen"]); // length of returned strings
    int amount = -2; // amount of strings to return - prep

    bool trycatch = true;
    while (trycatch)
    {
        try
        {
            //codelen = Convert.ToInt32(File.ReadAllText("codelen.txt"));

            Console.WriteLine("old len: " + olenght);
            Console.WriteLine("codelen: " + codelen);
            Console.WriteLine("threads: " + config["options:threads"]);
            Console.WriteLine("maxcodes:" + (Math.Pow(charcodes.Length, codelen) - olenght));
            Console.WriteLine("How many codes?: ");
            amount = Convert.ToInt32(Console.ReadLine());
            codelen = Convert.ToInt32(config["options:codelen"]);
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
        catch (FormatException)
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
    Stopwatch startWatch = new Stopwatch();
    startWatch.Start();
    if (amount == -1)
    {
        List<int> ints = new List<int>();
        for (int i = 0; i < codelen; i++)
        {
            ints.Add(0);
        }
        int intslen = codelen;
        int progress = 0;
        while (!ints.All(a => a >= charcodes.Length - 1))
        {
            progress++;
            string temp = "";
            ints[intslen - 1]++;
            if (ints.Any(x => x > charcodes.Length - 1))
            {
                for (int x = intslen - 1; x >= 0; x--)
                {
                    if (ints[x] > charcodes.Length - 1)
                    {
                        if (!ints.All(a => a > charcodes.Length - 1))
                        {
                            ints[x] = 0;
                        }
                        if (x - 1 >= 0 && x - 1 <= intslen)
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
            if (progress % Math.Round(Math.Pow(charcodes.Length, codelen) / 100) == 0)
            {
                Console.WriteLine("Progress: " + progress + " | " + Math.Pow(charcodes.Length, codelen) + " -- "
                    + (Math.Round((progress / Math.Pow(charcodes.Length, codelen)) * 100)) + "% -- " +
                    (DateTime.Now - startTime).ToString());
            }

        }
    }
    else if (amount <= -2)
    {
        List<int> ints = new List<int>();
        for (int i = 0; i < codelen; i++)
        {
            ints.Add(0);
        }
        Console.WriteLine("--Test Mode--");
        //Console.WriteLine(String.Join(" ", ints.ToArray()));
        Stopwatch debugwatch = new Stopwatch();
        debugwatch.Start();
        for (int i = Convert.ToInt32(Math.Pow(charcodes.Length, codelen)); i > 0; i--)
        {
            string temp = "";
            foreach (int x in ints)
            {
                temp += charcodes[x];
            }
            output.Add(temp);

            ints[codelen - 1]++;
            if (ints.Any(x => x >= charcodes.Length - 1))
            {
                for (int z = ints.Count; z > 0; z--)
                {
                    List<int> indexes = ints.Select((v, i) => new { v, i })
                        .Where(x => x.v > charcodes.Length - 1)
                        .Select(x => x.i).ToList();

                    foreach (int index in indexes)
                    {
                        ints[index] = 0;
                        if (index >= 1 && index <= codelen)
                        {
                            ints[index - 1]++;
                        }
                    }

                }
            }
        }
        debugwatch.Stop();
        Console.WriteLine(debugwatch.Elapsed);

    }
    else
    {
        //while (output.Count() - olenght < amount && output.Count() < Math.Pow(charcodes.Length, codelen))
        //{
        //    string temp = "";
        //    for (int y = codelen; y > 0; y--)
        //    {
        //        temp += charcodes[rand.Next(0, charcodes.Length - 1)];
        //    }
        //    output.Add(temp);
        //}
        int threads = Convert.ToInt32(config["options:threads"]);
        List<Task> tasks = new List<Task>();
        object lockObject = new object();
        int outputCount = output.Count() - olenght;
        void codegenThreads()
        {
            HashSet<string> strings = new HashSet<string>();

            while (outputCount < amount && outputCount < Math.Pow(charcodes.Length, codelen) && ((amount - outputCount) / threads + threads) > strings.Count())
            {
                string temp = "";
                for (int y = codelen; y > 0; y--)
                {
                    temp += charcodes[rand.Next(0, charcodes.Length - 1)];
                }
                strings.Add(temp);
            }
            lock (lockObject)
            {
                output.UnionWith(strings);
                // outputCount = output.Count() - olenght;
            }
            Console.WriteLine("Added: " + strings.Count());
        }
        async Task RandGenParAsync()
        {
            //tasks.Add(Task.Run(() => codegenMain()));
            // Int32.Parse(config["settings:threads"])
            for (int i = threads; i > 0; i--)
            {
                tasks.Add(Task.Run(() => codegenThreads()));
            }
            await Task.WhenAll(tasks);
        }
        while ((output.Count() - olenght) < amount && (output.Count() < Math.Pow(charcodes.Length, codelen)))
        {
            await RandGenParAsync();
            outputCount = output.Count() - olenght;
            Console.WriteLine("Current: " + outputCount + " - " + amount);
        }
        int countrem = (output.Count - olenght) - amount;
        output.RemoveWhere(x => countrem-- > 0);
    }


    startWatch.Stop();

    Console.WriteLine("------\n" + "Finish time: " + (startWatch.Elapsed).ToString() +
        "\nAmount Generated: " + (output.Count() - olenght));
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
