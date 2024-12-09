using System;

namespace MD3t.DB
{
    public abstract class Person
    {
        private string _name;
        private string _surname;
        private string _gender;

        public int Id { get; set; } // Primary Key
        public string Name
        {
            get => _name;
            set { if (!string.IsNullOrWhiteSpace(value)) _name = value; }
        }
        public string Surname
        {
            get => _surname;
            set => _surname = value;
        }
        public string FullName => $"{Name} {Surname}";
        public string Gender
        {
            get => _gender;
            set
            {
                if (value == "Woman" || value == "Man" || value == "Other")
                    _gender = value;
                else
                    throw new ArgumentException("Gender must be 'Woman', 'Man', or 'Other'.");
            }
        }

        public override string ToString()
        {
            return $"Name: {Name}, Surname: {Surname}, Full Name: {FullName}, Gender: {Gender}";
        }

        protected Person() { }

        protected Person(string name, string surname, string gender)
        {
            _name = name;
            _surname = surname;
            Gender = gender;
        }
    }

    public class Teacher : Person
    {
        public DateTime ContractDate { get; set; }

        public Teacher() { }

        public Teacher(string name, string surname, string gender, DateTime contractDate)
            : base(name, surname, gender)
        {
            ContractDate = contractDate;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Contract Date: {ContractDate.ToShortDateString()}";
        }
    }

    public class Student : Person
    {
        public string StudentIdNumber { get; set; }

        public Student() { }

        public Student(string name, string surname, string gender, string studentIdNumber)
            : base(name, surname, gender)
        {
            StudentIdNumber = studentIdNumber;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Student ID: {StudentIdNumber}";
        }
    }
}