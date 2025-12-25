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
using WpfApp2.Models;

namespace WpfApp2
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

        //tạo đối tượng dbcontext
        QlbanHangContext db = new QlbanHangContext();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiDuLieu();
            //lấy dữ liệu cho combo box
            var queryCombo = from lsp in db.LoaiSanPhams
                             select lsp;
            cboLoai.ItemsSource = queryCombo.ToList();
            cboLoai.DisplayMemberPath = "TenLoai"; //Hiển thị tên loại trong cboBox
            cboLoai.SelectedValuePath = "MaLoai"; //Khi chọn cboBox trả về mã loại tương ứng
            cboLoai.SelectedIndex = 0;
        }

        private void HienThiDuLieu()
        {
            var sanPhamQuery = from sp in db.SanPhams
                               select new
                               {
                                   sp.MaSp,
                                   sp.TenSp,
                                   sp.MaLoaiNavigation.TenLoai,
                                   sp.DonGia,
                                   sp.SoLuong,
                                   ThanhTien = sp.DonGia * sp.SoLuong
                               };
            dtgSanPham.ItemsSource = sanPhamQuery.ToList();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (isCheck()==true)
            {
                //Tạo đối tượng sản phẩm mới
                SanPham spMoi = new SanPham();
                //gán giá trị các thuộc tính của đối tượng sp mới là giá trị nhập
                spMoi.MaSp = txtMa.Text;
                spMoi.TenSp = txtTen.Text;
                spMoi.MaLoai = cboLoai.SelectedValue.ToString();
                spMoi.DonGia = int.Parse(txtDonGia.Text);
                spMoi.SoLuong = int.Parse(txtSoLuong.Text);
                //Thêm vào tập hợp các đối tượng SP
                db.SanPhams.Add(spMoi);
                db.SaveChanges();
                //Hiển thị lại sau khi thêm
                HienThiDuLieu(); 
            }
        }

        private bool isCheck()
        {
            // Kiểm tra nhập toàn bộ
            //txtMaLoai.Text=="" ||
            if (txtMa.Text=="" || txtTen.Text=="" ||  txtSoLuong.Text=="" || txtDonGia.Text == "")
            {
                MessageBox.Show("Phải nhập tất cả các trường", "THÊM", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            //Sửa 1 bản ghi có mã nhập vào text box mã sản phẩm khi nhấn nút sửa
            var querySua = from sp in db.SanPhams
                           where sp.MaSp == txtMa.Text
                           select sp;
            SanPham spSua = querySua.FirstOrDefault();
            // nếu có sp mã trùng với yêu cầu
            if(spSua !=null)
            {
                if (isCheckDLSua()==true)
                {
                    //sủa t.tin sp tìm đc, ko sửa mã Sp
                    spSua.TenSp = txtTen.Text;
                    spSua.MaLoai = cboLoai.SelectedValue.ToString();
                    spSua.SoLuong = int.Parse(txtSoLuong.Text);
                    spSua.DonGia = int.Parse(txtDonGia.Text); 
                }
            }
            // lưu vào csdl
            db.SaveChanges();
            HienThiDuLieu();
        }

        private bool isCheckDLSua()
        {
            if( !int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên và >0", "SỬA",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!int.TryParse(txtDonGia.Text, out int donGia) || donGia <= 0)
            {
                MessageBox.Show("Đơn giá phải là số nguyên và >0", "SỬA",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            //Xóa sản phẩm có mã nhập vào text box mã sản phẩm
            var queryXoa = from sp in db.SanPhams
                           where sp.MaSp == txtMa.Text
                           select sp;
            SanPham spXoa = queryXoa.FirstOrDefault();
            // nếu có sp mã trùng với yêu cầu
            if (spXoa != null)
            {
                //xác nhận xóa
                MessageBoxResult tl = MessageBox.Show("Bạn muốn xóa sản phẩm", "XÓA",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if ( tl == MessageBoxResult.Yes)
                {
                    db.SanPhams.Remove(spXoa);
                    // lưu vào csdl
                    db.SaveChanges();
                    HienThiDuLieu();
                }
                else
                {
                    MessageBox.Show("Không có sản phẩm muốn xóa", "XÓA",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            // lưu vào csdl
            db.SaveChanges();
            HienThiDuLieu();
        }

        private void btnTim_Click(object sender, RoutedEventArgs e)
        {
            //Khi nhấn nút Tìm: Hiển thị thông tin bán hàng của các sản phẩm lên DataGrid trên cửa sổ khác,
            //gồm các cột: Mã sản phẩm, tên sản phẩm, tên loại sản phẩm, số lượng đã bán, tổng số tiền bán sản phẩm. 
            var queryTim = from sp in db.SanPhams       //lấy dl từ bảng SPham
                           join lsp in db.LoaiSanPhams  // Kết nối bảng LoạiSP
                           on sp.MaLoai equals lsp.MaLoai   // điều kiện nối 2 bảng
                           group sp by lsp.TenLoai into nhom        // nhóm Dl theo tên loại sp. Mỗi nhóm chứa sp cùng loại
                           select new {
                               TenLoai = nhom.Key,
                               TongSoLuong = nhom.Sum(sp => sp.SoLuong),
                               TongTien = nhom.Sum(sp => sp.DonGia*sp.SoLuong),
                           };
            // đưa sang cửa sổ khác 
            Window1 myWindow = new Window1();
            myWindow.dtgTim.ItemsSource = queryTim.ToList();
            myWindow.ShowDialog();

        }

        private void dtgSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //chọn một dòng trong DataGrid, thông tin của sản phẩm được chọn hiển thị
            if ( dtgSanPham.SelectedItem != null )
            {
                var dongChon = (dynamic)dtgSanPham.SelectedItem;
                txtMa.Text = dongChon.MaSp;
                txtTen.Text = dongChon.TenSp;
                cboLoai.Text = dongChon.TenLoai;
                txtDonGia.Text = dongChon.DonGia.ToString();
                txtSoLuong.Text = dongChon.SoLuong.ToString();


            }
        }
    }
}