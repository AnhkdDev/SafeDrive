using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PRN_SafeDrive_Aplication.Models;

namespace PRN_SafeDrive_Aplication.Police
{
    /// <summary>
    /// Interaction logic for ThongKePassUserControl.xaml
    /// </summary>
    public partial class ThongKePassUserControl : UserControl
    {
        public ThongKePassUserControl()
        {
            InitializeComponent();
            LoadPassRateData();
        }

        private void LoadPassRateData()
        {
            // Giả sử có lớp Prn1Context và bảng Result, Course
            using (var db = new Prn1Context()) //hết nối csdl
            {
                var query = from c in db.Courses //Bắt đầu từ bảng Courses – tức là danh sách các khóa học.
                            join e in db.Exams on c.CourseId equals e.CourseId
                            //Nối bảng Exams với Courses qua CourseId.
                            //Ý nghĩa: Lấy ra tất cả các bài thi(Exams) của từng khóa học(Course).
                            join r in db.Results on e.ExamId equals r.ExamId
                            // Nối tiếp tục bảng Results (kết quả thi) với Exams qua ExamId.
                            //Ý nghĩa: Lấy danh sách kết quả thi của từng bài thi trong từng khóa học.
                            group r by new { c.CourseId, c.CourseName } into g
                            //Nhóm các kết quả thi r theo từng khóa học(CourseId, CourseName).
                            //Ý tưởng: gom tất cả các kết quả liên quan đến một khóa học vào 1 nhóm g.
                            select new
                            {
                                CourseName = g.Key.CourseName,
                                Total = g.Count(),//	Tổng số bài thi của học viên trong khóa
                                PassCount = g.Count(x => x.PassStatus), //Số lượng học viên có PassStatus = true
                                PassRate = g.Count() == 0 ? 0 : (int)((double)g.Count(x => x.PassStatus) / g.Count() * 100)
                                //ktra nếu k có kết quả nào trong nhóm thì g.count = 0 tránh lỗi chia cho 0
                            };
                PassRateDataGrid.ItemsSource = query.ToList(); //hiển thị
            }
        }
    }
}
