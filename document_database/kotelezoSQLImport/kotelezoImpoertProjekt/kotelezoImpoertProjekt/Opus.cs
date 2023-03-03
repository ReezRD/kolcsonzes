namespace kotelezoImpoertProjekt
{
    internal class Opus
    {
        public int id;
        public string writer;
        public string title;
        public int year;

        public Opus(int id, string writer, string title, int year)
        {
            this.id = id;
            this.writer = writer;
            this.title = title;
            this.year = year;
        }
    }
}