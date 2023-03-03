using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kotelezoImpoertProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileStudent = "diak.txt";
            string fileLoaning = "kolcsonzes.txt";
            string fileSpeciment = "peldany.txt";
            string fileOpus = "mu.txt";
            string fileKotelezoImport = "kotelezo.sql";

            SQLImport s = new SQLImport(fileLoaning, fileOpus, fileSpeciment, fileStudent, fileKotelezoImport);

            Console.WriteLine("forrás -> {0}", fileKotelezoImport);


            Console.ReadLine();
        }
    }
}
