namespace kotelezoImpoertProjekt
{
    internal class Loaning
    {
        public int id;
        public int specimentId;
        public int studentId;
        public string away;
        public string back;

        public Loaning(int id, int specimentId, int studentId, string away, string back)
        {
            this.id = id;
            this.specimentId = specimentId;
            this.studentId = studentId;
            this.away = away;
            this.back = back;
        }
    }
}