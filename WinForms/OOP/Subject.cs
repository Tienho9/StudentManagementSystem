using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapNhom.OOP
{
    internal class Subject
    {
        private string _subjectID;
        private string _subjectName;
        private int _credits;
        private int _semester;
        private string _description;

        public Subject() { }

        public Subject(string subjectID, string subjectName, int credits, int semester, string description)
        {
            SubjectID = subjectID;
            SubjectName = subjectName;
            Credits = credits;
            Semester = semester;
            Description = description;
        }

        public string SubjectID { get => _subjectID; set => _subjectID = value; }
        public string SubjectName { get => _subjectName; set => _subjectName = value; }
        public int Credits { get => _credits; set => _credits = value; }
        public int Semester { get => _semester; set => _semester = value; }
        public string Description { get => _description; set => _description = value; }
    }
    
}
