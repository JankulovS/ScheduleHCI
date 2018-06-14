namespace Schedule.Model
{
    public class Software : NameClass
    {
        private string id;
        private string name;
        private string os;
        private string maker;
        private string website;
        private int year;
        private float price;
        private string description;
        
        public Software()
        {
        }

        public Software(string id, string name, string os, string maker, string website, int year, float price, string description)
        {
            this.id = id;
            this.name = name;
            this.os = os;
            this.maker = maker;
            this.website = website;
            this.year = year;
            this.price = price;
            this.description = description;
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public float Price
        {
            get { return price; }
            set { price = value; }
        }
        
        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public string Website
        {
            get { return website; }
            set { website = value; }
        }

        public string Maker
        {
            get { return maker; }
            set { maker = value; }
        }

        public string OS
        {
            get { return os; }
            set { os = value; }
        }
        
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public override string ToString()
        {
            return Name;
        }

    }
}