using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollCallApp.Models
{
    public class RollCallContext : DbContext //kế thừa lại lớp dbcontext của entity framewwork
    {
        public RollCallContext()
        {

        }

        public RollCallContext(DbContextOptions<RollCallContext> options) : base(options) //options: truyền thông tin: connectionstring
        {

        }

        public virtual DbSet<AttendanceRollCall> AttendanceRollCalls { get; set; } //dbset: đại diện cho 1 entity (AttendanceRollCalls model), thực hiện CRUD
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<ProfileStudent> ProfileStudents { get; set; }
        public virtual DbSet<ProfileTeacher> ProfileTeachers { get; set; }
        public virtual DbSet<RegisterSubject> RegisterSubjects { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               // optionsBuilder.UseSqlServer(@"Server=tcp:sqlrollcall.database.windows.net,1433;Database=RollCallDB;User Id=thaoluc;Password=Zelda20297;");
                optionsBuilder.UseSqlServer(@"Server=tcp:sqlrollcall.database.windows.net,1433;Initial Catalog=RollCallDB;Persist Security Info=False;User ID=thaoluc;Password=Zelda20297;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
               // optionsBuilder.UseSqlServer(@"Server=DESKTOP-L5E5FH3\SQLEXPRESS;Database=RollCallDB;User ID=sa; Password=123456;");
            }
        }
        //tao db o day
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AttendanceRollCall>(entity =>
            {
                entity.ToTable("ATTENDANCE_ROLL_CALL");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CheckAttendance)
                    .HasColumnName("checkAttendance")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DateCheck)
                    .HasColumnName("dateCheck")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RegisterId)
                    .HasColumnName("registerID")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("CONTACT");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasMaxLength(255);

                entity.Property(e => e.StudentId)
                    .HasColumnName("StudentID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SubjectID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("NOTIFICATION");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasMaxLength(255);

                entity.Property(e => e.DateCreate)
                    .HasColumnName("dateCreate")
                    .HasColumnType("datetime");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("subjectID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TeacherId)
                    .HasColumnName("teacherID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ProfileStudent>(entity =>
            {
                entity.HasKey(e => e.ProfileId);

                entity.ToTable("PROFILE_STUDENT");

                entity.Property(e => e.ProfileId)
                    .HasColumnName("profileID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ClassId)
                    .HasColumnName("classID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("departmentID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProfileTeacher>(entity =>
            {
                entity.HasKey(e => e.ProfileId);

                entity.ToTable("PROFILE_TEACHER");

                entity.Property(e => e.ProfileId)
                    .HasColumnName("profileID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("departmentID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Specialize)
                    .HasColumnName("specialize")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RegisterSubject>(entity =>
            {
                entity.HasKey(e => e.RegisterId);

                entity.ToTable("REGISTER_SUBJECT");

                entity.Property(e => e.RegisterId)
                    .HasColumnName("registerID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.DateEnd)
                    .HasColumnName("dateEnd")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DateStart)
                    .HasColumnName("dateStart")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId)
                    .HasColumnName("studentID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId)
                    .HasColumnName("subjectID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TeacherId)
                    .HasColumnName("teacherID")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("STUDENT");

                entity.Property(e => e.StudentId)
                    .HasColumnName("studentID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.FaceId)
                    .HasColumnName("faceID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PersonId)
                    .HasColumnName("personID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileId)
                    .HasColumnName("profileID")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("SUBJECT");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("subjectID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();


                entity.Property(e => e.SubjectName)
                    .HasColumnName("subjectName")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("TEACHER");

                entity.Property(e => e.TeacherId)
                    .HasColumnName("teacherID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ProfileId)
                    .HasColumnName("profileID")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}

