using System.Data;
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

namespace WpfApp1
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
        List<NhanVien> dsNhanVien = new List<NhanVien>();

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {

            if (isKiemTra())
            {
                //lenh Nhap
                //Tao doi tuong lop NhanVien
                NhanVien nvMoi = new NhanVien();
                //Gan gia tri nguoi dung nhap vao
                nvMoi.MaNV = int.Parse(txtMa.Text);
                nvMoi.HoTen = txtHoTen.Text;
                nvMoi.NgaySinh = Convert.ToDateTime(dpNgaySinh.SelectedDate);
                if (radNam.IsChecked == true)
                {
                    nvMoi.GioiTinh = "Nam";
                }
                else
                {
                    nvMoi.GioiTinh = "Nu";
                }
                nvMoi.PhongBan = cboPhongBan.Text;
                nvMoi.HeSoLuong = double.Parse(txtHeSoLuong.Text);
                dsNhanVien.Add(nvMoi);
                //Hien thi ds trong DataGrid
                dtgNhanVien.ItemsSource = null;
                dtgNhanVien.ItemsSource = dsNhanVien; 
            }
        }

        private void btnWindow2_Click(object sender, RoutedEventArgs e)
        {
            int tuoiMax = dsNhanVien[0].Tuoi;
            for ( int i  = 1; i < dsNhanVien.Count; i++ )
            {
                if (dsNhanVien[i].Tuoi > tuoiMax )
                {
                    tuoiMax = dsNhanVien [i].Tuoi;
                }
            }
            List<NhanVien> dsTuoiMax = new List<NhanVien>();
            foreach ( var items in dsNhanVien)
            {
                if ( items.Tuoi == tuoiMax)
                {
                    dsTuoiMax.Add(items);
                }
            }
            Window2 myWindow = new Window2();
            myWindow.dtgTuoiMax.ItemsSource = dsTuoiMax;
            myWindow.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpNgaySinh.SelectedDate = DateTime.Now;
        }
        private bool isKiemTra()
        {
            //Phai nhap du truong du lieu
            if(txtMa.Text == "")
            {
                MessageBox.Show("Ban chua nhap MaNV","Loi du lieu",MessageBoxButton.OK, MessageBoxImage.Error);
                txtMa.Focus(); //Con tro nhap du lieu o txtMa
                return false;
            }
            if (txtHoTen.Text == "")
            {
                MessageBox.Show("Ban chua nhap HoTen", "Loi du lieu", MessageBoxButton.OK, MessageBoxImage.Error);
                txtHoTen.Focus(); //Con tro nhap du lieu o txtMa
                return false;
            }
            if (txtHeSoLuong.Text == "")
            {
                MessageBox.Show("Ban chua nhap He so luong", "Loi du lieu", MessageBoxButton.OK, MessageBoxImage.Error);
                txtHeSoLuong.Focus(); //Con tro nhap du lieu o txtMa
                return false;
            }
            try
            {
                double.Parse(txtHeSoLuong.Text);
            }
            catch(Exception){
                MessageBox.Show("He so luong la so thuc", "Loi du lieu", MessageBoxButton.OK, MessageBoxImage.Error);
                txtHeSoLuong.SelectAll();
                txtHeSoLuong.Focus();
                return false;
            }

            int tuoi = DateTime.Now.Year - dpNgaySinh.SelectedDate.Value.Year;
            if (tuoi < 18)
            {
                MessageBox.Show("Tuoi phai lon hon 18", "Loi du lieu", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}