using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp.Entities
{
    public class DrivingLessonInstructor
    {
        public int DrivingLessonId { get; set; }
        public int UserId { get; set; }
        public int InstructorId { get; set; }
        public string InstructorFirstName { get; set; }
        public string InstructorLastName { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }
    }
}
