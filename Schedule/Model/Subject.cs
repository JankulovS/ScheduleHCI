using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule.Model
{
    public class Subject : NameClass
    {
        private string id;
        private string name;
        private Course course;
        private string description;
        private int groupSize;
        private int classLength;
        private int noOfClasses;
        private int noOfClassesSet;
        private bool projector;
        private bool board;
        private bool smartBoard;
        private string os;
        private List<Software> software;

        public Subject()
        {
        }

        public Subject(string id, string name, Course course, string description, int groupSize, int classLength, int noOfClasses, int noOfClassesSet, bool projector, bool board, bool smartBoard, string os)
        {
            this.id = id;
            this.name = name;
            this.course = course;
            this.description = description;
            this.groupSize = groupSize;
            this.classLength = classLength;
            this.noOfClasses = noOfClasses;
            this.noOfClassesSet = noOfClassesSet;
            this.projector = projector;
            this.board = board;
            this.smartBoard = smartBoard;
            this.os = os;
            this.software = new List<Software>();
        }

        public Subject(string id, string name, Course course, string description, int groupSize, int classLength, int noOfClasses, int noOfClassesSet, bool projector, bool board, bool smartBoard, string os, List<Software> software)
        {
            this.id = id;
            this.name = name;
            this.course = course;
            this.description = description;
            this.groupSize = groupSize;
            this.classLength = classLength;
            this.noOfClasses = noOfClasses;
            this.noOfClassesSet = noOfClassesSet;
            this.projector = projector;
            this.board = board;
            this.smartBoard = smartBoard;
            this.os = os;
            this.software = software;
        }

        public List<Software> Software
        {
            get { return software; }
            set { software = value; }
        }

        public string OS
        {
            get { return os; }
            set { os = value; }
        }

        public bool SmartBoard
        {
            get { return smartBoard; }
            set { smartBoard = value; }
        }

        public bool Board
        {
            get { return board; }
            set { board = value; }
        }

        public bool Projector
        {
            get { return projector; }
            set { projector = value; }
        }


        public int NoOfClasses
        {
            get { return noOfClasses; }
            set { noOfClasses = value; }
        }

        public int NoOfClassesSet
        {
            get { return noOfClassesSet; }
            set { noOfClassesSet = value; }
        }


        public int ClassLength
        {
            get { return classLength; }
            set { classLength = value; }
        }

        public int GroupSize
        {
            get { return groupSize; }
            set { groupSize = value; }
        }


        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        public Course Course
        {
            get { return course; }
            set { course = value; }
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

    }
}
