using DemoBai11_12.MoHinhDuLieu;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoBai11_12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //tạo đối tượng dbContext 
        QlnhanSuContext db = new QlnhanSuContext();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //hiển thị bảng Nhân viên lên datagrid ngay khi hiển thị window
            HienThiDuLieu();

            //Lấy danh sách cho combo box
            var comboQuery = from pb in db.PhongBans
                             select pb;
            cboPhongBan.ItemsSource = comboQuery.ToList();
            //comboBox hiển thị tên phòng ban
            cboPhongBan.DisplayMemberPath = "TenPb";
            //Khi người dùng chọn 1 phần tử trong combo thì lấy mã phòng ban tương ứng
            cboPhongBan.SelectedValuePath = "MaPb";
        }

        private void HienThiDuLieu()
        {
            //-- lấy dữ liệu từ bảng
            var nhanVienQuery = from nv in db.NhanViens
                                select nv;
            //--hiển thị lên datagrid
            dtgNhanVien.ItemsSource = nhanVienQuery.ToList();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            //thêm 1 nhân viên vào bảng
            //-- tạo đối tượng nhân viên mới
            NhanVien nvMoi = new NhanVien();
            nvMoi.MaNv = txtMa.Text;
            nvMoi.HoTen= txtHoTen.Text;
            nvMoi.NgaySinh = dpNgaySinh.SelectedDate;
            if (radNam.IsChecked == true)
                nvMoi.Gioitinh = "Nam";
            else
                nvMoi.Gioitinh = "Nữ";
            nvMoi.MaPb = cboPhongBan.SelectedValue.ToString();
            //thêm vào collection 
            db.NhanViens.Add(nvMoi);
            //lưu thay đổi vào csdl
            db.SaveChanges();
            //hiển thị lại sau khi cập nhật
            HienThiDuLieu();
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            //lấy ra đối tượng muốn sửa
            var suaQuery = from nv in db.NhanViens
                           where nv.MaNv == txtMa.Text
                           select nv;
            NhanVien nvSua = suaQuery.FirstOrDefault();
            //sửa thông tin của đối tượng tìm được
            nvSua.HoTen = txtHoTen.Text;
            nvSua.NgaySinh = dpNgaySinh.SelectedDate;
            if (radNam.IsChecked == true)
                nvSua.Gioitinh = "Nam";
            else
                nvSua.Gioitinh = "Nữ";
            nvSua.MaPb = cboPhongBan.SelectedValue.ToString();
            //lưu thay đổi vào csdl
            db.SaveChanges();
            //hiển thị lại sau khi cập nhật
            HienThiDuLieu();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            //lấy ra đối tượng muốn xóa
            var xoaQuery = from nv in db.NhanViens
                           where nv.MaNv == txtMa.Text
                           select nv;
            NhanVien nvXoa = xoaQuery.FirstOrDefault();
            //Xóa khỏi collection 
            db.NhanViens.Remove(nvXoa);
            //lưu thay đổi vào csdl
            db.SaveChanges();
            //hiển thị lại sau khi cập nhật
            HienThiDuLieu();
        }
    }
}