using Newtonsoft.Json;
using ResumeBuilder.Models;

namespace ResumeBuilder.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            return;
            //if (!context.Users.Any())
            //{
            //    return;
            //}

            //var users = new Account[]
            //{
            //    new Account
            //    {
            //        Email="santiago.carreno05@gmail.com",
            //        Password="MyResume9*",
            //    }
            //};

            //foreach (Account user in users)
            //{
            //    context.Add(user);
            //}

            //context.SaveChanges();

            //int accountId = context.Accounts.Single(i => i.Email.Equals("santiago.carreno05@gmail.com")).Id;
            string userId = "";

            var personalInfo = new PersonalInfo[]
            {
                new PersonalInfo
                {
                    UserId = userId,
                    FirstName="Santiago",
                    LastName="Carreño",
                    Email="santiago.carreno05@gmail.com",
                    PhoneNumber="437-661-2248",
                    Address="775 Midland Avenue, Scarborough, ON M1K 4E5",                    
                    //AdditionalContactInfo =
                    //LinkedInURL="https://www.linkedin.com/in/santiago-felipe-carreno-pardo/",
                    //WebsiteURL="https://santicarreno9.github.io/PortfolioWebsite/",
                    //GitHubAccount="https://github.com/SantiCarreno9"
                }
            };

            foreach (PersonalInfo pInfo in personalInfo)
            {
                context.Add(pInfo);
            }

            context.SaveChanges();

            var profileEntries = new ProfileEntry[]
            {
                new ProfileEntry
                {
                    UserId = userId,
                    Category=EntryCategory.Education,
                    Title= "Game Programming",
                    Organization= "Centennial College",
                    Location= "Toronto, Ontario, Canada",
                    StartDate= new DateTime(2023,9,5),
                    EndDate=null,
                    IsCurrent=true
                },
                new ProfileEntry
                {
                    UserId = userId,
                    Category=EntryCategory.Education,
                    Title= "Bachelor of Mechatronics Engineering",
                    Organization= "Universidad Militar Nueva Granada",
                    Location= "Bogotá, Colombia",
                    StartDate= new DateTime(2015,1,1),
                    EndDate=new DateTime(2021,3,25),
                    IsCurrent=false
                },
                new ProfileEntry
                {
                    UserId = userId,
                    Category=EntryCategory.WorkExperience,
                    Title= "VR Developer",
                    Organization= "Somnium Space",
                    Location= "Prague, Czech Republic",
                    StartDate= new DateTime(2022,8,15),
                    EndDate=new DateTime(2023,9,1),
                    IsCurrent=false
                },
                new ProfileEntry
                {
                    UserId = userId,
                    Category=EntryCategory.WorkExperience,
                    Title= "Unity Developer",
                    Organization= "Consultoria GP S.A.S",
                    Location= "Medellín, Colombia",
                    StartDate= new DateTime(2021,11,1),
                    EndDate=new DateTime(2022,08,12),
                    IsCurrent=false
                },
                new ProfileEntry
                {
                    UserId = userId,
                    Category=EntryCategory.WorkExperience,
                    Title= "VR Developer",
                    Organization= "MPL eLearning XR Services",
                    Location= "Bogotá, Colombia",
                    StartDate= new DateTime(2021,2,1),
                    EndDate=new DateTime(2021,8,1),
                    IsCurrent=false
                }
            };

            foreach (ProfileEntry profileEntry in profileEntries)
            {
                context.Add(profileEntry);
            }

            context.SaveChanges();
        }

        //public static void Initialize(context)
        //{
        //    //context.Database.EnsureCreated();

        //    // Look for any students.
        //    if (context.Students.Any())
        //    {
        //        return;   // DB has been seeded
        //    }

        //    var students = new Student[]
        //    {
        //        new Student { FirstMidName = "Carson",   LastName = "Alexander",
        //            EnrollmentDate = DateTime.Parse("2010-09-01") },
        //        new Student { FirstMidName = "Meredith", LastName = "Alonso",
        //            EnrollmentDate = DateTime.Parse("2012-09-01") },
        //        new Student { FirstMidName = "Arturo",   LastName = "Anand",
        //            EnrollmentDate = DateTime.Parse("2013-09-01") },
        //        new Student { FirstMidName = "Gytis",    LastName = "Barzdukas",
        //            EnrollmentDate = DateTime.Parse("2012-09-01") },
        //        new Student { FirstMidName = "Yan",      LastName = "Li",
        //            EnrollmentDate = DateTime.Parse("2012-09-01") },
        //        new Student { FirstMidName = "Peggy",    LastName = "Justice",
        //            EnrollmentDate = DateTime.Parse("2011-09-01") },
        //        new Student { FirstMidName = "Laura",    LastName = "Norman",
        //            EnrollmentDate = DateTime.Parse("2013-09-01") },
        //        new Student { FirstMidName = "Nino",     LastName = "Olivetto",
        //            EnrollmentDate = DateTime.Parse("2005-09-01") }
        //    };

        //    foreach (Student s in students)
        //    {
        //        context.Students.Add(s);
        //    }
        //    context.SaveChanges();

        //    var instructors = new Instructor[]
        //    {
        //        new Instructor { FirstMidName = "Kim",     LastName = "Abercrombie",
        //            HireDate = DateTime.Parse("1995-03-11") },
        //        new Instructor { FirstMidName = "Fadi",    LastName = "Fakhouri",
        //            HireDate = DateTime.Parse("2002-07-06") },
        //        new Instructor { FirstMidName = "Roger",   LastName = "Harui",
        //            HireDate = DateTime.Parse("1998-07-01") },
        //        new Instructor { FirstMidName = "Candace", LastName = "Kapoor",
        //            HireDate = DateTime.Parse("2001-01-15") },
        //        new Instructor { FirstMidName = "Roger",   LastName = "Zheng",
        //            HireDate = DateTime.Parse("2004-02-12") }
        //    };

        //    foreach (Instructor i in instructors)
        //    {
        //        context.Instructors.Add(i);
        //    }
        //    context.SaveChanges();

        //    var departments = new Department[]
        //    {
        //        new Department { Name = "English",     Budget = 350000,
        //            StartDate = DateTime.Parse("2007-09-01"),
        //            InstructorID  = instructors.Single( i => i.LastName == "Abercrombie").ID },
        //        new Department { Name = "Mathematics", Budget = 100000,
        //            StartDate = DateTime.Parse("2007-09-01"),
        //            InstructorID  = instructors.Single( i => i.LastName == "Fakhouri").ID },
        //        new Department { Name = "Engineering", Budget = 350000,
        //            StartDate = DateTime.Parse("2007-09-01"),
        //            InstructorID  = instructors.Single( i => i.LastName == "Harui").ID },
        //        new Department { Name = "Economics",   Budget = 100000,
        //            StartDate = DateTime.Parse("2007-09-01"),
        //            InstructorID  = instructors.Single( i => i.LastName == "Kapoor").ID }
        //    };

        //    foreach (Department d in departments)
        //    {
        //        context.Departments.Add(d);
        //    }
        //    context.SaveChanges();

        //    var courses = new Course[]
        //    {
        //        new Course {CourseID = 1050, Title = "Chemistry",      Credits = 3,
        //            DepartmentID = departments.Single( s => s.Name == "Engineering").DepartmentID
        //        },
        //        new Course {CourseID = 4022, Title = "Microeconomics", Credits = 3,
        //            DepartmentID = departments.Single( s => s.Name == "Economics").DepartmentID
        //        },
        //        new Course {CourseID = 4041, Title = "Macroeconomics", Credits = 3,
        //            DepartmentID = departments.Single( s => s.Name == "Economics").DepartmentID
        //        },
        //        new Course {CourseID = 1045, Title = "Calculus",       Credits = 4,
        //            DepartmentID = departments.Single( s => s.Name == "Mathematics").DepartmentID
        //        },
        //        new Course {CourseID = 3141, Title = "Trigonometry",   Credits = 4,
        //            DepartmentID = departments.Single( s => s.Name == "Mathematics").DepartmentID
        //        },
        //        new Course {CourseID = 2021, Title = "Composition",    Credits = 3,
        //            DepartmentID = departments.Single( s => s.Name == "English").DepartmentID
        //        },
        //        new Course {CourseID = 2042, Title = "Literature",     Credits = 4,
        //            DepartmentID = departments.Single( s => s.Name == "English").DepartmentID
        //        },
        //    };

        //    foreach (Course c in courses)
        //    {
        //        context.Courses.Add(c);
        //    }
        //    context.SaveChanges();

        //    var officeAssignments = new OfficeAssignment[]
        //    {
        //        new OfficeAssignment {
        //            InstructorID = instructors.Single( i => i.LastName == "Fakhouri").ID,
        //            Location = "Smith 17" },
        //        new OfficeAssignment {
        //            InstructorID = instructors.Single( i => i.LastName == "Harui").ID,
        //            Location = "Gowan 27" },
        //        new OfficeAssignment {
        //            InstructorID = instructors.Single( i => i.LastName == "Kapoor").ID,
        //            Location = "Thompson 304" },
        //    };

        //    foreach (OfficeAssignment o in officeAssignments)
        //    {
        //        context.OfficeAssignments.Add(o);
        //    }
        //    context.SaveChanges();

        //    var courseInstructors = new CourseAssignment[]
        //    {
        //        new CourseAssignment {
        //            CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID,
        //            InstructorID = instructors.Single(i => i.LastName == "Kapoor").ID
        //            },
        //        new CourseAssignment {
        //            CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID,
        //            InstructorID = instructors.Single(i => i.LastName == "Harui").ID
        //            },
        //        new CourseAssignment {
        //            CourseID = courses.Single(c => c.Title == "Microeconomics" ).CourseID,
        //            InstructorID = instructors.Single(i => i.LastName == "Zheng").ID
        //            },
        //        new CourseAssignment {
        //            CourseID = courses.Single(c => c.Title == "Macroeconomics" ).CourseID,
        //            InstructorID = instructors.Single(i => i.LastName == "Zheng").ID
        //            },
        //        new CourseAssignment {
        //            CourseID = courses.Single(c => c.Title == "Calculus" ).CourseID,
        //            InstructorID = instructors.Single(i => i.LastName == "Fakhouri").ID
        //            },
        //        new CourseAssignment {
        //            CourseID = courses.Single(c => c.Title == "Trigonometry" ).CourseID,
        //            InstructorID = instructors.Single(i => i.LastName == "Harui").ID
        //            },
        //        new CourseAssignment {
        //            CourseID = courses.Single(c => c.Title == "Composition" ).CourseID,
        //            InstructorID = instructors.Single(i => i.LastName == "Abercrombie").ID
        //            },
        //        new CourseAssignment {
        //            CourseID = courses.Single(c => c.Title == "Literature" ).CourseID,
        //            InstructorID = instructors.Single(i => i.LastName == "Abercrombie").ID
        //            },
        //    };

        //    foreach (CourseAssignment ci in courseInstructors)
        //    {
        //        context.CourseAssignments.Add(ci);
        //    }
        //    context.SaveChanges();

        //    var enrollments = new Enrollment[]
        //    {
        //        new Enrollment {
        //            StudentID = students.Single(s => s.LastName == "Alexander").ID,
        //            CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID,
        //            Grade = Grade.A
        //        },
        //            new Enrollment {
        //            StudentID = students.Single(s => s.LastName == "Alexander").ID,
        //            CourseID = courses.Single(c => c.Title == "Microeconomics" ).CourseID,
        //            Grade = Grade.C
        //            },
        //            new Enrollment {
        //            StudentID = students.Single(s => s.LastName == "Alexander").ID,
        //            CourseID = courses.Single(c => c.Title == "Macroeconomics" ).CourseID,
        //            Grade = Grade.B
        //            },
        //            new Enrollment {
        //                StudentID = students.Single(s => s.LastName == "Alonso").ID,
        //            CourseID = courses.Single(c => c.Title == "Calculus" ).CourseID,
        //            Grade = Grade.B
        //            },
        //            new Enrollment {
        //                StudentID = students.Single(s => s.LastName == "Alonso").ID,
        //            CourseID = courses.Single(c => c.Title == "Trigonometry" ).CourseID,
        //            Grade = Grade.B
        //            },
        //            new Enrollment {
        //            StudentID = students.Single(s => s.LastName == "Alonso").ID,
        //            CourseID = courses.Single(c => c.Title == "Composition" ).CourseID,
        //            Grade = Grade.B
        //            },
        //            new Enrollment {
        //            StudentID = students.Single(s => s.LastName == "Anand").ID,
        //            CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID
        //            },
        //            new Enrollment {
        //            StudentID = students.Single(s => s.LastName == "Anand").ID,
        //            CourseID = courses.Single(c => c.Title == "Microeconomics").CourseID,
        //            Grade = Grade.B
        //            },
        //        new Enrollment {
        //            StudentID = students.Single(s => s.LastName == "Barzdukas").ID,
        //            CourseID = courses.Single(c => c.Title == "Chemistry").CourseID,
        //            Grade = Grade.B
        //            },
        //            new Enrollment {
        //            StudentID = students.Single(s => s.LastName == "Li").ID,
        //            CourseID = courses.Single(c => c.Title == "Composition").CourseID,
        //            Grade = Grade.B
        //            },
        //            new Enrollment {
        //            StudentID = students.Single(s => s.LastName == "Justice").ID,
        //            CourseID = courses.Single(c => c.Title == "Literature").CourseID,
        //            Grade = Grade.B
        //            }
        //    };

        //    foreach (Enrollment e in enrollments)
        //    {
        //        var enrollmentInDataBase = context.Enrollments.Where(
        //            s =>
        //                    s.Student.ID == e.StudentID &&
        //                    s.Course.CourseID == e.CourseID).SingleOrDefault();
        //        if (enrollmentInDataBase == null)
        //        {
        //            context.Enrollments.Add(e);
        //        }
        //    }
        //    context.SaveChanges();
        //}
    }
}