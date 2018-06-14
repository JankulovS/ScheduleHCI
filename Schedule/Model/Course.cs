using System;

namespace Schedule.Model
{
    public class Course : NameClass
    {
        private string id;
        private string name;
        private DateTime date;
        private string description;

        public Course()
        {
        }

        public Course(string id, string name, DateTime date, string description)
        {
            this.id = id;
            this.name = name;
            this.date = date;
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
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
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