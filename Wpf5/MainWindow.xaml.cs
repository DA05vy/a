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
using WpfApp5.Models;

namespace WpfApp5
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
        QlbanHangContext db = new QlbanHangContext();

        private void HienThi()
        {
            var query = from sp in db.SanPhams
                        orderby sp.SoLuong * sp.DonGia ascending
                        select new
                        {
                            sp.MaSp,
                            sp.TenSp,
                            sp.MaLoai,
                            sp.SoLuong,
                            sp.DonGia,
                            ThanhTien = sp.SoLuong * sp.DonGia
                        };
            dtgSanPham.ItemsSource = query.ToList();
        }
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (isCheck())
            {
                SanPham sp = new SanPham();
                sp.MaSp = txtMa.Text;
                sp.TenSp = txtTen.Text;
                sp.MaLoai = cboLoai.SelectedValue.ToString(); 
                sp.SoLuong = int.Parse(txtSoLuong.Text);
                sp.DonGia = int.Parse(txtDonGia.Text);

                db.SanPhams.Add(sp);
                db.SaveChanges();
                HienThi();
            }
        }

        private bool isCheck()
        {
            if ( !int.TryParse( txtSoLuong.Text, out int soLuong ) || soLuong <= 0 )
            {
                MessageBox.Show("So Luong phai la so nguyen lon hon 0", "Loi", MessageBoxButton.OK, MessageBoxImage.Error );
                return false;
            }
            if (!int.TryParse(txtDonGia.Text, out int donGia) || donGia <= 0)
            {
                MessageBox.Show("Don Gia phai la so nguyen lon hon 0", "Loi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string maSp = txtMa.Text.Trim();
            bool exist = db.SanPhams.Any( sp =>  sp.MaSp == maSp );
            if ( exist == true )
            {
                MessageBox.Show("Da ton tai ma san pham nay", "Loi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            var querySua = from sp in db.SanPhams
                           where sp.MaSp == txtMa.Text
                           select sp;
            SanPham spSua = querySua.FirstOrDefault();

            if ( spSua != null)
            {
                if (isCheck2() == true)
                {
                    spSua.TenSp = txtTen.Text;
                    spSua.MaLoai = cboLoai.SelectedValue.ToString();
                    spSua.SoLuong = int.Parse(txtSoLuong.Text);
                    spSua.DonGia = int.Parse(txtDonGia.Text);
                }
            }
            db.SaveChanges();
            HienThi();
            
        }

        private bool isCheck2()
        {
            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("So Luong phai la so nguyen lon hon 0", "Loi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!int.TryParse(txtDonGia.Text, out int donGia) || donGia <= 0)
            {
                MessageBox.Show("Don Gia phai la so nguyen lon hon 0", "Loi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            var queryXoa = from sp in db.SanPhams
                           where sp.MaSp == txtMa.Text
                           select sp;
            SanPham spXoa = queryXoa.FirstOrDefault();
            if (spXoa != null)
            {
                MessageBoxResult t1 = MessageBox.Show("Ban co muon xoa san pham?", "Thong bao", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if ( t1 == MessageBoxResult.Yes)
                {
                    db.SanPhams.Remove(spXoa);
                    db.SaveChanges();
                }
            } else
            {
                MessageBox.Show("Khong co San pham de xoa", "Thong bao", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            HienThi();
        }

        private void btnTim_Click(object sender, RoutedEventArgs e)
        {
            var queryTim = from sp in db.SanPhams
                           join lsp in db.LoaiSanPhams
                           on sp.MaLoai equals lsp.MaLoai
                           group sp by new { lsp.MaLoai, lsp.TenLoai } into nhom
                           select new
                           {
                               MaLoai = nhom.Key.MaLoai,
                               TenLoai = nhom.Key.TenLoai,
                               TongTien = nhom.Sum(sp => sp.DonGia * sp.SoLuong)
                           };
            Window1 myWin = new Window1();
            myWin.dtgTim.ItemsSource = queryTim.ToList();
            myWin.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HienThi();

            var queryCbo = from lsp in db.LoaiSanPhams
                           select lsp;
            cboLoai.ItemsSource = queryCbo.ToList();
            cboLoai.DisplayMemberPath = "TenLoai";
            cboLoai.SelectedValuePath = "MaLoai";
            cboLoai.SelectedIndex = 0;

        }
    }
}