using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PRN_SafeDrive_Aplication.Models;

namespace PRN_SafeDrive_Aplication.BLL
{
    public class CourseService
    {
        public List<Course> GetCoursesByTeacherId(int teacherId)
        {
            using (var db = new Prn1Context())
            {
                return db.Courses
                         .Where(c => c.TeacherId == teacherId)
                         .ToList();
            }
        }

        public List<Course> GetCoursesByEmail(string email)
        {
            using (var context = new Prn1Context())
            {
                // Chuẩn hóa email
                string normalizedEmail = email.Trim().ToLower();

                // Tìm user theo email
                var user = context.Users
                    .FirstOrDefault(u => u.Email.Trim().ToLower() == normalizedEmail);

                if (user == null)
                    return new List<Course>();

                // Lấy các đăng ký của user
                var registrations = context.Registrations
                    .Where(r => r.UserId == user.UserId)
                    .Select(r => r.CourseId)
                    .ToList();

                if (registrations.Count == 0)
                    return new List<Course>();

                // Lấy danh sách khóa học từ CourseId
                var courses = context.Courses
                    .Where(c => registrations.Contains(c.CourseId))
                    .ToList();

                return courses;
            }
        }


    }
}
