namespace kotelezoImpoertProjekt
{
    internal class Speciment
    {
        public int id;
        public int opus;
        public int price;
        public string acquisition;

        public Speciment(int id, int opus, int price, string acquisition)
        {
            this.id = id;
            this.opus = opus;
            this.price = price;
            this.acquisition = acquisition;
        }
    }
}