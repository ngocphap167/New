//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FormChinh
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblHDNhap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblHDNhap()
        {
            this.tblChiTietHDNhaps = new HashSet<tblChiTietHDNhap>();
        }
    
        public string MaHDNhap { get; set; }
        public string Manhanvien { get; set; }
        public Nullable<System.DateTime> Ngaynhap { get; set; }
        public string maNCC { get; set; }
        public Nullable<double> Tongtien { get; set; }
    
        public virtual NHACUNGCAP NHACUNGCAP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblChiTietHDNhap> tblChiTietHDNhaps { get; set; }
        public virtual tblNhanVien tblNhanVien { get; set; }
    }
}
