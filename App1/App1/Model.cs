using System.Collections.Generic;

namespace App1
{


    public class ModelCard
    {
        public string code { get; set; }
        public string image { get; set; }
        public string value { get; set; }
        public string suit { get; set; }
    }
    public class CardS //Колода
    {
        public string code { get; set; }
        public string image { get; set; }
        public Images images { get; set; }
        public string value { get; set; }
        public string suit { get; set; }
    }

    public class Images
    {
        public string svg { get; set; }
        public string png { get; set; }
    }
    public class MakeDeckClass
    {
        public bool success { get; set; }
        public string deck_id { get; set; }
        public bool shuffled { get; set; }
        public int remaining { get; set; }
    }
    public class Root //Центральный класс для получения карт
    {
        public bool success { get; set; }
        public string deck_id { get; set; }
        public List<CardS> cards { get; set; }
        public int remaining { get; set; }
    }


}