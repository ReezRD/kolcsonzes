using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace kotelezoImpoertProjekt
{
    internal class SQLImport
    {
        private string fileLoaning;
        private string fileOpus;
        private string fileSpeciment;
        private string fileStudent;
        private string fileKotelezoImport;

        List<Loaning> loaningList = new List<Loaning>();
        List<Opus> opusList = new List<Opus>();
        List<Speciment> specimentList = new List<Speciment>();
        List<Student> studentList = new List<Student>();
        Dictionary<string, int> idDictionary = new Dictionary<string, int>();
        UTF8Encoding utf8 = new UTF8Encoding();

        public SQLImport(string fileLoaning, string fileOpus, string fileSpeciment, string fileStudent, string fileKotelezoImport)
        {
            this.fileLoaning = fileLoaning;
            this.fileOpus = fileOpus;
            this.fileSpeciment = fileSpeciment;
            this.fileStudent = fileStudent;
            this.fileKotelezoImport = fileKotelezoImport;
            DataIntoLoaning();
            DataIntoOpus();
            DataIntoSpeciment();
            DataIntoStudent();
            ImportGenerator();
        }

        private void ImportGenerator()
        {
            DeleteGenerator();
            OpusGenerator();
            SpecimentGenerator();
            StudentGenerator();
            LoaningGenerator();
            SelectGenerator();
        }

        private void SelectGenerator()
        {
            List<string> flines = new List<string>();
            flines.Add("");
            flines.Add("SELECT * FROM loaning;");
            flines.Add("SELECT * FROM opus;");
            flines.Add("SELECT * FROM specimen;");
            flines.Add("SELECT * FROM student;");

            File.AppendAllLines(fileKotelezoImport, flines);
        }

        private void DeleteGenerator()
        {
            List<string> flines = new List<string>();
            flines.Add("DELETE FROM loaning;");
            flines.Add("DELETE FROM opus;");
            flines.Add("DELETE FROM specimen;");
            flines.Add("DELETE FROM student;");

            File.WriteAllLines(fileKotelezoImport, flines);
        }

        private void LoaningGenerator()
        {
            List<string> flines = new List<string>();

            string text = Environment.NewLine + "# Loanings" + Environment.NewLine;
            text += "INSERT INTO loaning (id, specimentid, studentid, away, back) VALUES" + Environment.NewLine;

            foreach (var loaning in loaningList)
            {
                string line = $"({loaning.id}, {loaning.specimentId}, {loaning.studentId}, {loaning.away}, {loaning.back})";
                flines.Add(line);
            }

            text += String.Join("," + Environment.NewLine, flines);
            text += ";";

            File.AppendAllText(fileKotelezoImport, text);
        }

        private void OpusGenerator()
        {
            List<string> flines = new List<string>();

            string text = Environment.NewLine + "# Opus" + Environment.NewLine;
            text += "INSERT INTO opus (id, writer, title, year) VALUES" + Environment.NewLine;

            foreach (var opus in opusList)
            {
                string line = $"({opus.id}, {opus.writer}, {opus.title}, {opus.year})";
                flines.Add(line);
            }

            text += String.Join("," + Environment.NewLine, flines);
            text += ";";

            
            File.AppendAllText(fileKotelezoImport, text, utf8 );
        }

        private void SpecimentGenerator()
        {
            List<string> flines = new List<string>();

            string text = Environment.NewLine + "# Specimens" + Environment.NewLine;
            text += "INSERT INTO specimen (id, opusid, price, acquisition) VALUES" + Environment.NewLine;

            foreach (var specimen in specimentList)
            {
                string line = $"({specimen.id}, {specimen.opus}, {specimen.price}, {specimen.acquisition})";
                flines.Add(line);
            }

            text += String.Join("," + Environment.NewLine, flines);
            text += ";";

            File.AppendAllText(fileKotelezoImport, text);
        }

        private void StudentGenerator()
        {
            List<string> flines = new List<string>();

            string text = Environment.NewLine + "# Students" + Environment.NewLine;
            text += "INSERT INTO student (id, name, year) VALUES" + Environment.NewLine;

            foreach (var student in studentList)
            {
                string line = $"({student.id}, {student.name}, {student.year})";
                flines.Add(line);
            }

            text += String.Join("," + Environment.NewLine, flines);
            text += ";";

            File.AppendAllText(fileKotelezoImport, text);
        }

        private void DataIntoLoaning()
        {
            string[] lines = File.ReadAllLines(fileLoaning);
            foreach (var line in lines.Skip(1))
            {
                string[] collums = line.Split('\t');
                int id = int.Parse(collums[0]);
                int specimentId = int.Parse(collums[1]);
                int studentId = int.Parse(collums[2]);
                string away = $"'{collums[3]}'";
                string back = $"'{collums[4]}'";

                loaningList.Add(new Loaning(id, specimentId, studentId, away, back));
            }
        }

        private void DataIntoOpus()
        {
            string[] lines = File.ReadAllLines(fileOpus);
            foreach (var line in lines.Skip(1))
            {
                string[] collums = line.Split('\t');
                int id = int.Parse(collums[0]);
                string writer = $"'{collums[1]}'";
                string title = $"'{collums[2]}'";
                int year = int.Parse(collums[3]);

                opusList.Add(new Opus(id, writer, title, year));
            }
        }

        private void DataIntoSpeciment()
        {
            string[] lines = File.ReadAllLines(fileSpeciment);
            foreach (var line in lines.Skip(1))
            {
                string[] collums = line.Split('\t');
                int id = int.Parse(collums[0]);
                int opus = int.Parse(collums[1]);
                int price = int.Parse(collums[2]);
                string acquisition = $"'{collums[3]}'";

                specimentList.Add(new Speciment(id, opus, price, acquisition));
            }
        }

        private void DataIntoStudent()
        {
            string[] lines = File.ReadAllLines(fileStudent);
            foreach (var line in lines.Skip(1))
            {
                string[] collums = line.Split('\t');
                int id = int.Parse(collums[0]);
                string name = $"'{collums[1]}'";
                int year = int.Parse(collums[2]);

                studentList.Add(new Student(id, name, year));
            }
        }
    }
}