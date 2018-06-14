using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule.Model
{
    public class Classroom
    {
        private string id;
        private string description;
        private int noOfSeats;
        private bool projector;
        private bool board;
        private bool smartBoard;
        private string system;
        private List<Software> software;

        public Classroom()
        {

        }

        public Classroom(string id, string description, int noOfSeats, bool projector, bool board, bool smartBoard, string system, List<Software> software)
        {
            this.id = id;
            this.description = description;
            this.noOfSeats = noOfSeats;
            this.projector = projector;
            this.board = board;
            this.smartBoard = smartBoard;
            this.system = system;
            this.software = software;
        }

        public Classroom(string id, string description, int noOfSeats, bool projector, bool board, bool smartBoard, string system)
        {
            this.id = id;
            this.description = description;
            this.noOfSeats = noOfSeats;
            this.projector = projector;
            this.board = board;
            this.smartBoard = smartBoard;
            this.system = system;
            this.software = new List<Software>();
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }


        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        public int NoOfSeats
        {
            get { return noOfSeats; }
            set { noOfSeats = value; }
        }


        public bool Projector
        {
            get { return projector; }
            set { projector = value; }
        }


        public bool Board
        {
            get { return board; }
            set { board = value; }
        }


        public bool SmartBoard
        {
            get { return smartBoard; }
            set { smartBoard = value; }
        }


        public string System
        {
            get { return system; }
            set { system = value; }
        }


        public List<Software> Software
        {
            get { return software; }
            set { software = value; }
        }


    }
}
