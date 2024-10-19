using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO_QLBaoHanh;
using MongoDB.Bson;
using MongoDB.Driver;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QLBaoHanh
{
    internal class KetNoi
    {
        static IMongoDatabase database;
        static IMongoCollection<SanPham> sanPhamCollection;
        static IMongoCollection<KhachHang> khachHangCollection;
        static IMongoCollection<PhieuBaoHanh> phieuBaoHanhCollection;
        static IMongoCollection<PhieuSuaChua> PhieuSuaChuaCollection;
        static IMongoCollection<LichSuBaoHanh> LichSuBaoHanhCollection;
        static IMongoCollection<ChiTietPhieuSua> ChiTietPhieuSuaCollection;
        static IMongoCollection<LinhKien> LinhKienCollection;

        MongoClient client;
        string connect = "mongodb+srv://hoangvi:21072003@cluster0.ldt5o.mongodb.net/QlyBaoHanh?retryWrites=true&w=majority&appName=Cluster0";
        public KetNoi() 
        {
            //client = new MongoClient("mongodb://localhost:27017");
            client = new MongoClient(connect);
            database = client.GetDatabase("QlyBaoHanh");
            khachHangCollection = database.GetCollection<KhachHang>("KhachHang");
            sanPhamCollection = database.GetCollection<SanPham>("SanPham");
            phieuBaoHanhCollection = database.GetCollection<PhieuBaoHanh>("PhieuBaoHanh");
            PhieuSuaChuaCollection = database.GetCollection<PhieuSuaChua>("PhieuSuaChua");
            LichSuBaoHanhCollection = database.GetCollection<LichSuBaoHanh>("LichSuBaoHanh");
            ChiTietPhieuSuaCollection = database.GetCollection<ChiTietPhieuSua>("ChiTietPhieuSua");
            LinhKienCollection = database.GetCollection<LinhKien>("LinhKien");
        }

        // ------------------------------------Sản phẩm------------------------------------
        public DataTable getSanPham()
        {
            var docs = sanPhamCollection.Find(FilterDefinition<SanPham>.Empty).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã sản phẩm");
            dt.Columns.Add("Tên sản phẩm");
            dt.Columns.Add("Model");
            dt.Columns.Add("Số seri");
            dt.Columns.Add("Ngày mua");
            dt.Columns.Add("Thời gian bảo hành");
            dt.Columns.Add("Khách hàng");

            foreach (var doc in docs)
            {
                DataRow row = dt.NewRow();
                row["Mã sản phẩm"] = doc._id;
                row["Tên sản phẩm"] = doc.ten_san_pham;
                row["Model"] = doc.model;
                row["Số seri"] = doc.so_seri;
                row["Ngày mua"] = doc.ngay_mua;
                row["Thời gian bảo hành"] = doc.thoi_gian_bao_hanh;
                
                var khachHang = khachHangCollection.Find(kh => kh._id == doc.id_khach_hang).FirstOrDefault();
                row["Khách hàng"] = khachHang != null ? khachHang.ten_khach_hang : "Không xác định";
                dt.Rows.Add(row);
            }

            return dt;
        }

        internal object getSanPham(string ma)
        {
            
            var docs = sanPhamCollection.Find(Builders<SanPham>.Filter.Eq("id_khach_hang", ma)).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã sản phẩm");
            dt.Columns.Add("Tên sản phẩm");
            dt.Columns.Add("Model");
            dt.Columns.Add("Số seri");
            dt.Columns.Add("Ngày mua");
            dt.Columns.Add("Thời gian bảo hành");
            dt.Columns.Add("Khách hàng");

            foreach (var doc in docs)
            {
                DataRow row = dt.NewRow();
                row["Mã sản phẩm"] = doc._id;
                row["Tên sản phẩm"] = doc.ten_san_pham;
                row["Model"] = doc.model;
                row["Số seri"] = doc.so_seri;
                row["Ngày mua"] = doc.ngay_mua;
                row["Thời gian bảo hành"] = doc.thoi_gian_bao_hanh;

                var khachHang = khachHangCollection.Find(x => x._id == doc.id_khach_hang).FirstOrDefault();
                row["Khách hàng"] = khachHang != null ? khachHang.ten_khach_hang : "Không xác định";
                dt.Rows.Add(row);
            }

            return dt;
        }

        public bool checkSP(string ma)
        {
            var sp = sanPhamCollection.Find(Builders<SanPham>.Filter.Eq(s => s._id, ma)).FirstOrDefault();
            if (sp != null)
                return true;
            return false;
        }

        internal int ThemSP(SanPham sp)
        {
            if(sp._id != "" || sp.id_khach_hang != "" || sp.ten_san_pham != "" || sp.ngay_mua != null)
            {
                sanPhamCollection.InsertOne(sp);
                PhieuBaoHanh pbh = new PhieuBaoHanh
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    id_san_pham = sp._id,
                    ngay_cap = sp.ngay_mua,
                    ngay_het_han = DateTime.Now.AddMonths(sp.thoi_gian_bao_hanh),
                    trang_thai = "Còn hiệu lực"
                };
                phieuBaoHanhCollection.InsertOne(pbh);
                return 1;
            }
            return 0;
            
        }
        internal void SuaSP(SanPham sp)
        {
            var filter = Builders<SanPham>.Filter.Eq("_id", sp._id);
            var update = Builders<SanPham>.Update
           .Set("ten_san_pham", sp.ten_san_pham)
           .Set("model", sp.model)
           .Set("so_seri", sp.so_seri)
           .Set("ngay_mua", sp.ngay_mua)
           .Set("thoi_gian_bao_hanh", sp.thoi_gian_bao_hanh)
           .Set("id_khach_hang", sp.id_khach_hang);
            sanPhamCollection.UpdateOne(filter, update);
        }

        internal object TimTheoTenSP_DSSP(string text)
        {
            var filter = Builders<SanPham>.Filter.Regex(s => s.ten_san_pham, new BsonRegularExpression(text, "i"));
            var docs = sanPhamCollection.Find(filter).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã sản phẩm");
            dt.Columns.Add("Tên sản phẩm");
            dt.Columns.Add("Model");
            dt.Columns.Add("Số seri");
            dt.Columns.Add("Ngày mua");
            dt.Columns.Add("Thời gian bảo hành");
            dt.Columns.Add("Khách hàng");

            foreach (var doc in docs)
            {
                DataRow row = dt.NewRow();
                row["Mã sản phẩm"] = doc._id;
                row["Tên sản phẩm"] = doc.ten_san_pham;
                row["Model"] = doc.model;
                row["Số seri"] = doc.so_seri;
                row["Ngày mua"] = doc.ngay_mua;
                row["Thời gian bảo hành"] = doc.thoi_gian_bao_hanh;

                var khachHang = khachHangCollection.Find(x => x._id == doc.id_khach_hang).FirstOrDefault();
                row["Khách hàng"] = khachHang != null ? khachHang.ten_khach_hang : "Không xác định";
                dt.Rows.Add(row);
            }

            return docs;
        }

        //------------------------------------Khách hàng--------------------------------------------
        public DataTable getKhachHang()
        {
            var docs = khachHangCollection.Find(FilterDefinition<KhachHang>.Empty).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã khách hàng");
            dt.Columns.Add("Tên khách hàng");
            dt.Columns.Add("Địa chỉ");
            dt.Columns.Add("Số điện thoại");
            dt.Columns.Add("Email");

            foreach (var doc in docs)
            {
                DataRow row = dt.NewRow();
                row["Mã khách hàng"] = doc._id;
                row["Tên khách hàng"] = doc.ten_khach_hang;
                row["Địa chỉ"] = doc.dia_chi;
                row["Số điện thoại"] = doc.so_dien_thoai;
                row["Email"] = doc.email;

                dt.Rows.Add(row);
            }

            return dt;
        }

        internal object TimKiemKHTheoTen(string text)
        {
            var filter = Builders<KhachHang>.Filter.Regex(s => s.ten_khach_hang, new BsonRegularExpression(text, "i"));
            var docs = khachHangCollection.Find(filter).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã khách hàng");
            dt.Columns.Add("Tên khách hàng");
            dt.Columns.Add("Địa chỉ");
            dt.Columns.Add("Số điện thoại");
            dt.Columns.Add("Email");

            foreach (var doc in docs)
            {
                DataRow row = dt.NewRow();
                row["Mã khách hàng"] = doc._id;
                row["Tên khách hàng"] = doc.ten_khach_hang;
                row["Địa chỉ"] = doc.dia_chi;
                row["Số điện thoại"] = doc.so_dien_thoai;
                row["Email"] = doc.email;

                dt.Rows.Add(row);
            }

            return dt;
        }
        public bool checkKH(string ma)
        {
            var KH = khachHangCollection.Find(Builders<KhachHang>.Filter.Eq(s => s._id, ma)).FirstOrDefault();
            if (KH != null)
                return true;
            return false;
        }
        internal void ThemKH(string ma, string ten, string diachi, string sdt, string email)
        {
            KhachHang doc = new KhachHang
            {
                _id = ma,
                ten_khach_hang = ten,
                dia_chi = diachi,
                so_dien_thoai = sdt,
                email = email
            };
            khachHangCollection.InsertOne(doc);
        }

        internal void SuaKH(string ma, string ten, string diachi, string sdt, string email)
        {
            var filter = Builders<KhachHang>.Filter.Eq("_id", ma);
            var update = Builders<KhachHang>.Update
           .Set("ten_khach_hang", ten)
           .Set("dia_chi", diachi)
           .Set("so_dien_thoai", sdt)
           .Set("email", email);
            khachHangCollection.UpdateOne(filter, update);
        }
        // ------------------------------------Bảo hành--------------------------------------------
        public DataTable getPhieuBaoHanh()
        {
            var docs = phieuBaoHanhCollection.Find(FilterDefinition<PhieuBaoHanh>.Empty).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã phiếu");
            dt.Columns.Add("Tên sản phẩm");
            dt.Columns.Add("Ngày kích hoạt");
            dt.Columns.Add("Ngày hết hạn");
            dt.Columns.Add("Trạng thái");

            foreach (var doc in docs)
            {
                DataRow row = dt.NewRow();
                row["Mã phiếu"] = doc._id;

                var sp = sanPhamCollection.Find(s => s._id == doc.id_san_pham).FirstOrDefault();
                row["Tên sản phẩm"] = sp != null ? sp.ten_san_pham : "Không xác định";
                row["Ngày kích hoạt"] = doc.ngay_cap;
                row["Ngày hết hạn"] = doc.ngay_het_han;
                row["Trạng thái"] = doc.trang_thai;

                dt.Rows.Add(row);
            }

            return dt;
        }

        internal object TimTheoTenSP_DSPBH(string text)
        {
            var filter = Builders<SanPham>.Filter.Regex(s => s.ten_san_pham, new BsonRegularExpression(text, "i"));
            var sanPham = sanPhamCollection.Find(filter).FirstOrDefault();
            var docs = phieuBaoHanhCollection.Find(FilterDefinition<PhieuBaoHanh>.Empty).ToList();
            if (sanPham != null)
                docs = phieuBaoHanhCollection.Find(Builders<PhieuBaoHanh>.Filter.Eq(p => p.id_san_pham, sanPham._id)).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã phiếu");
            dt.Columns.Add("Tên sản phẩm");
            dt.Columns.Add("Ngày kích hoạt");
            dt.Columns.Add("Ngày hết hạn");
            dt.Columns.Add("Trạng thái");

            foreach (var doc in docs)
            {
                DataRow row = dt.NewRow();
                row["Mã phiếu"] = doc._id;

                var sp = sanPhamCollection.Find(s => s._id == doc.id_san_pham).FirstOrDefault();
                row["Tên sản phẩm"] = sp != null ? sp.ten_san_pham : "Không xác định";
                row["Ngày kích hoạt"] = doc.ngay_cap;
                row["Ngày hết hạn"] = doc.ngay_het_han;
                row["Trạng thái"] = doc.trang_thai;

                dt.Rows.Add(row);
            }

            return dt;
        }

        // ------------------------------------Phiếu sửa chữa--------------------------------------------

        public DataTable getPhieuSuaChua()
        {
            var docs = PhieuSuaChuaCollection.Find(FilterDefinition<PhieuSuaChua>.Empty).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã phiếu");
            dt.Columns.Add("Tên sản phẩm");
            dt.Columns.Add("Tên khách hàng");
            dt.Columns.Add("Ngày nhận");
            dt.Columns.Add("Ngày hẹn trả");
            dt.Columns.Add("Trạng thái");


            foreach (var doc in docs)
            {
                DataRow row = dt.NewRow();
                row["Mã phiếu"] = doc._id;

                var sp = sanPhamCollection.Find(s => s._id == doc.san_pham).FirstOrDefault();
                row["Tên sản phẩm"] = sp != null ? sp.ten_san_pham : "Không xác định";

                var kh = khachHangCollection.Find(k => k._id == sp.id_khach_hang).FirstOrDefault();
                row["Tên khách hàng"] = kh != null ? kh.ten_khach_hang : "Không xác định";

                row["Ngày nhận"] = doc.ngay_nhan;
                row["Ngày hẹn trả"] = doc.ngay_hen_tra;
                row["Trạng thái"] = doc.trang_thai;

                dt.Rows.Add(row);
            }

            return dt;
        }
        internal object getPhieuSuaChua(string tt)
        {
            var docs = PhieuSuaChuaCollection.Find(Builders<PhieuSuaChua>.Filter.Eq(p => p.trang_thai, tt)).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã phiếu");
            dt.Columns.Add("Tên sản phẩm");
            dt.Columns.Add("Tên khách hàng");
            dt.Columns.Add("Ngày nhận");
            dt.Columns.Add("Ngày hẹn trả");
            dt.Columns.Add("Trạng thái");


            foreach (var doc in docs)
            {
                DataRow row = dt.NewRow();
                row["Mã phiếu"] = doc._id;

                var sp = sanPhamCollection.Find(s => s._id == doc.san_pham).FirstOrDefault();
                row["Tên sản phẩm"] = sp != null ? sp.ten_san_pham : "Không xác định";

                var kh = khachHangCollection.Find(k => k._id == sp.id_khach_hang).FirstOrDefault();
                row["Tên khách hàng"] = kh != null ? kh.ten_khach_hang : "Không xác định";

                row["Ngày nhận"] = doc.ngay_nhan;
                row["Ngày hẹn trả"] = doc.ngay_hen_tra;
                row["Trạng thái"] = doc.trang_thai;

                dt.Rows.Add(row);
            }

            return dt;
        }

        internal object getChiTietPhieuSau(string maps)
        {
            var docs = ChiTietPhieuSuaCollection.Find(Builders<ChiTietPhieuSua>.Filter.Eq(p => p.id_phieu_sua, maps)).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã LK");
            dt.Columns.Add("Tên linh kiện");
            dt.Columns.Add("Đơn giá");
            dt.Columns.Add("Hãng SX");

            foreach (var doc in docs)
            {
                DataRow row = dt.NewRow();
                var lk = LinhKienCollection.Find(l => l._id.ToString() == doc.id_linh_kien).FirstOrDefault();

                row["Mã LK"] = lk._id.ToString();

                row["Tên linh kiện"] = lk != null ? lk.ten_lk : "Không xác định";
                row["Đơn giá"] = lk.don_gia;
                row["Hãng SX"] = lk.hang;
                dt.Rows.Add(row);
            }

            return dt;
        }
        internal PhieuSuaChua Get1PhieuSua(string ma)
        {
            var doc = PhieuSuaChuaCollection.Find(Builders<PhieuSuaChua>.Filter.Eq(p => p._id, ma)).FirstOrDefault();
            PhieuSuaChua phieuSuaChua = new PhieuSuaChua();
            phieuSuaChua = (PhieuSuaChua)doc;
            return phieuSuaChua;
        }

        internal bool suaPhieuSuaChua(string ma, string mota, string loai, int thanhtien, int giamgia)
        {
            if(ma != null)
            {
                var filter = Builders<PhieuSuaChua>.Filter.Eq("_id", ma);
                var update = Builders<PhieuSuaChua>.Update
               .Set("mo_ta", mota)
               .Set("loai_sua_chua", loai)
               .Set("gia_sua_chua", thanhtien)
               .Set("tien_giam", giamgia);
                PhieuSuaChuaCollection.UpdateOne(filter, update);
                return true;
            }
            return false;
        }

        internal int ThemPSC(PhieuSuaChua psc)
        {
            psc._id = ObjectId.GenerateNewId().ToString();
            psc.trang_thai = "Mới tiếp nhận";
            if (psc != null)
            {
                PhieuSuaChuaCollection.InsertOne(psc);
                return 1;
            }
            return 0;
        }
        internal int XoaPhieuSua(string ma)
        {
            var doc = ChiTietPhieuSuaCollection.Find(Builders<ChiTietPhieuSua>.Filter.Eq(p => p.id_phieu_sua, ma)).ToList();
            if(doc.Count == 0)
            {
                var filter = Builders<PhieuSuaChua>.Filter.Eq(p => p._id, ma);
                if (filter != null)
                {
                    PhieuSuaChuaCollection.DeleteOne(filter);
                    return 1;
                }
            }
            return 0;
        }

        internal int ThemChiTietPS(ChiTietPhieuSua ctps, int dongia)
        {
            var doc = PhieuSuaChuaCollection.Find(Builders<PhieuSuaChua>.Filter.Eq(p => p._id, ctps.id_phieu_sua)).FirstOrDefault();

            doc.gia_sua_chua += dongia;

            if(ctps != null)
            {
                ChiTietPhieuSuaCollection.InsertOne(ctps);
                var filter = Builders<PhieuSuaChua>.Filter.Eq("_id", ctps.id_phieu_sua);
                var update = Builders<PhieuSuaChua>.Update
               .Set("gia_sua_chua", doc.gia_sua_chua)
               .Set("trang_thai", "Đang xử lý");
                PhieuSuaChuaCollection.UpdateOne(filter, update);
                return 1;
            }
            return 0;
        }

        internal int XoaChiTietPhieuSua(string ma)
        {
            var doc = ChiTietPhieuSuaCollection.Find(Builders<ChiTietPhieuSua>.Filter.Eq(p => p.id_phieu_sua, ma)).ToList();
            if (doc.Count == 0)
                return 0;
            else
            {
                var filter = Builders<ChiTietPhieuSua>.Filter.Eq(p => p.id_phieu_sua, ma);
                if (filter != null)
                {
                    ChiTietPhieuSuaCollection.DeleteOne(filter);

                    var docPSC = PhieuSuaChuaCollection.Find(Builders<PhieuSuaChua>.Filter.Eq(p => p._id, ma)).FirstOrDefault();
                    docPSC.gia_sua_chua = 0;
                    docPSC.tien_giam = 0;
                    var filterpsc = Builders<PhieuSuaChua>.Filter.Eq("_id", ma);
                    var update = Builders<PhieuSuaChua>.Update
                       .Set("gia_sua_chua", docPSC.gia_sua_chua)
                       .Set("tien_giam", docPSC.tien_giam);
                    PhieuSuaChuaCollection.UpdateOne(filterpsc, update);

                    return 1;
                }
                return -1;
            }
        }

        // ------------------------------------lịch sử bảo hành--------------------------------------------

        public DataTable getLichSuBaoHanh()
        {
            var docs = LichSuBaoHanhCollection.Find(FilterDefinition<LichSuBaoHanh>.Empty).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã");
            dt.Columns.Add("Tên sản phẩm");
            dt.Columns.Add("Tên khách hàng");
            dt.Columns.Add("Mô tả");
            dt.Columns.Add("Loại sửa chữa");
            dt.Columns.Add("Ngày nhận");
            dt.Columns.Add("Ngày trả");

            foreach (var doc in docs)
            {
                DataRow row = dt.NewRow();
                row["Mã"] = doc._id;

                var sp = sanPhamCollection.Find(s => s._id == doc.id_san_pham).FirstOrDefault();
                row["Tên sản phẩm"] = sp != null ? sp.ten_san_pham : "Không xác định";

                var kh = khachHangCollection.Find(k => k._id == doc.id_khach_hang).FirstOrDefault();
                row["Tên khách hàng"] = kh != null ? kh.ten_khach_hang : "Không xác định";

                var psc = PhieuSuaChuaCollection.Find(p => p._id == doc.id_phieu_sua_chua).FirstOrDefault();
                row["Mô tả"] = psc != null ? psc.mo_ta : "Không xác định";
                row["Loại sửa chữa"] = psc != null ? psc.loai_sua_chua : "Không xác định";
                row["Ngày nhận"] = psc.ngay_nhan;
                row["Ngày trả"] = psc.ngay_hen_tra;

                dt.Rows.Add(row);
            }

            return dt;
        }

        internal object TimKiemLS_TheoTenKH(string text)
        {
            var filter = Builders<KhachHang>.Filter.Regex(s => s.ten_khach_hang, new BsonRegularExpression(text, "i"));
            var KH = khachHangCollection.Find(filter).FirstOrDefault();
            var docs = LichSuBaoHanhCollection.Find(FilterDefinition<LichSuBaoHanh>.Empty).ToList();
            if (KH != null)
                docs = LichSuBaoHanhCollection.Find(Builders<LichSuBaoHanh>.Filter.Eq(p => p.id_khach_hang, KH._id)).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã");
            dt.Columns.Add("Tên sản phẩm");
            dt.Columns.Add("Tên khách hàng");
            dt.Columns.Add("Mô tả");
            dt.Columns.Add("Loại sửa chữa");
            dt.Columns.Add("Ngày nhận");
            dt.Columns.Add("Ngày trả");

            foreach (var doc in docs)
            {
                DataRow row = dt.NewRow();
                row["Mã"] = doc._id;

                var sp = sanPhamCollection.Find(s => s._id == doc.id_san_pham).FirstOrDefault();
                row["Tên sản phẩm"] = sp != null ? sp.ten_san_pham : "Không xác định";

                var kh = khachHangCollection.Find(k => k._id == doc.id_khach_hang).FirstOrDefault();
                row["Tên khách hàng"] = kh != null ? kh.ten_khach_hang : "Không xác định";

                var psc = PhieuSuaChuaCollection.Find(p => p._id == doc.id_phieu_sua_chua).FirstOrDefault();
                row["Mô tả"] = psc != null ? psc.mo_ta : "Không xác định";
                row["Loại sửa chữa"] = psc != null ? psc.loai_sua_chua : "Không xác định";
                row["Ngày nhận"] = psc.ngay_nhan;
                row["Ngày trả"] = psc.ngay_hen_tra;

                dt.Rows.Add(row);
            }

            return dt;
        }

        internal int ThemLichSuBaoHanh(string ma)
        {
            PhieuSuaChua phieu = PhieuSuaChuaCollection.Find(Builders<PhieuSuaChua>.Filter.Eq(p => p._id, ma)).FirstOrDefault();
            SanPham sp = sanPhamCollection.Find(Builders<SanPham>.Filter.Eq(s => s._id, phieu.san_pham)).FirstOrDefault();
            KhachHang KH = khachHangCollection.Find(Builders<KhachHang>.Filter.Eq(t => t._id, sp.id_khach_hang)).FirstOrDefault();
            if (phieu != null)
            {
                if(phieu.trang_thai == "Mới tiếp nhận")
                {
                    return 0;
                }    
                var filter = Builders<PhieuSuaChua>.Filter.Eq("_id", ma);
                var update = Builders<PhieuSuaChua>.Update
               .Set("trang_thai", "Đã xong");
                PhieuSuaChuaCollection.UpdateOne(filter, update);

                LichSuBaoHanh ls = new LichSuBaoHanh
                {
                    _id = "LS_" + phieu._id ,
                    id_phieu_sua_chua = phieu._id,
                    id_san_pham = phieu.san_pham,
                    id_khach_hang = KH._id,
                };
                LichSuBaoHanhCollection.InsertOne(ls);
                return 1;
            }
            return 0;
        }

        // ------------------------------------Linh Kiện--------------------------------------------

        internal object getLinhKien()
        {
            var docs = LinhKienCollection.Find(FilterDefinition<LinhKien>.Empty).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã linh kiện");
            dt.Columns.Add("Tên linh kiện");
            dt.Columns.Add("Đơn giá");
            dt.Columns.Add("Hãng SX");

            foreach(var doc in docs)
            {
                DataRow row = dt.NewRow();
                row["Mã linh kiện"] = doc._id;
                row["Tên linh kiện"] = doc.ten_lk;
                row["Đơn giá"] = doc.don_gia;
                row["Hãng SX"] = doc.hang;
                dt.Rows.Add(row);
            }
            return dt;
        }

        internal LinhKien get1LinhKien(string ma)
        {
            var doc = LinhKienCollection.Find(Builders<LinhKien>.Filter.Eq(p => p._id, ma)).FirstOrDefault();
            LinhKien linh = new LinhKien();
            linh = (LinhKien)doc;
            return linh;
        }

        
    }
}
