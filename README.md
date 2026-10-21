# 🛍️ XuanDucShop

<p align="center">
  <strong>HỆ THỐNG QUẢN LÝ CỬA HÀNG XuanDucShop</strong><br/>
  Ứng dụng web xây dựng với ASP.NET Core MVC và .NET 10
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet&logoColor=white" alt=".NET 10"/>
  <img src="https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?style=flat-square&logo=dotnet&logoColor=white" alt="ASP.NET Core MVC"/>
  <img src="https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat-square&logo=bootstrap&logoColor=white" alt="Bootstrap"/>
  <img src="https://img.shields.io/badge/C%23-Ngôn%20ngữ%20lập%20trình-239120?style=flat-square&logo=csharp&logoColor=white" alt="C#"/>
</p>

---

## 📌 1. Giới thiệu

**XuanDucShop** là ứng dụng web hỗ trợ quản lý các hoạt động cơ bản của cửa hàng. Dự án được phát triển bằng ASP.NET Core MVC, sử dụng giao diện Bootstrap và tổ chức mã nguồn theo mô hình Model–View–Controller.

Mục tiêu của dự án là xây dựng một hệ thống có giao diện trực quan, dễ sử dụng và hỗ trợ quản lý dữ liệu thông qua các thao tác CRUD.

## ✨ 2. Chức năng chính

| Chức năng | Mô tả |
|---|---|
| 📊 Tổng quan | Hiển thị thông tin tổng hợp của cửa hàng. |
| 🗂️ Danh mục | Thêm, xem, chỉnh sửa và xóa danh mục sản phẩm. |
| 📦 Sản phẩm | Quản lý thông tin sản phẩm và danh mục liên quan. |
| 👥 Nhân viên | Quản lý thông tin nhân viên. |
| 🧾 Hóa đơn | Quản lý hóa đơn và các chi tiết hóa đơn. |
| 🛡️ Kiểm tra dữ liệu | Kiểm tra dữ liệu đầu vào và các mối quan hệ giữa đối tượng. |
| 📱 Giao diện thích ứng | Hỗ trợ hiển thị trên nhiều kích thước màn hình. |

## 🧰 3. Công nghệ sử dụng

- ⚙️ **ASP.NET Core MVC:** Xây dựng cấu trúc ứng dụng web.
- 💻 **C#:** Ngôn ngữ lập trình chính.
- 🔷 **.NET 10:** Nền tảng chạy ứng dụng.
- 🎨 **Bootstrap 5:** Xây dựng giao diện.
- 🧠 **StoreData:** Quản lý dữ liệu trong bộ nhớ.

## 🚀 4. Hướng dẫn cài đặt và chạy dự án

### Bước 1. Chuẩn bị môi trường

Cài đặt các công cụ sau trước khi chạy dự án:

- **.NET 10 SDK:** [Tải tại đây](https://dotnet.microsoft.com/download/dotnet/10.0).
- **Git:** [Tải tại đây](https://git-scm.com/downloads).
- **Visual Studio 2022** phiên bản hỗ trợ .NET 10, hoặc Visual Studio Code nếu muốn chỉnh sửa mã nguồn bằng trình soạn thảo nhẹ hơn.

Nếu sử dụng Visual Studio, hãy kiểm tra trong Visual Studio Installer rằng đã cài đặt thành phần **ASP.NET and web development** và phiên bản SDK phù hợp.

### Bước 2. Kiểm tra công cụ

Mở **Terminal**, **PowerShell** hoặc **Command Prompt**, sau đó chạy:

```bash
dotnet --info
git --version
```

- Lệnh `dotnet --info` hiển thị thông tin SDK và môi trường .NET.
- Lệnh `git --version` xác nhận Git đã được cài đặt.

Nếu hệ thống không nhận diện lệnh `dotnet` hoặc `git`, hãy kiểm tra lại quá trình cài đặt và biến môi trường `PATH`, sau đó mở terminal mới.

### Bước 3. Tải mã nguồn từ GitHub

Chọn thư mục muốn lưu dự án, mở terminal tại thư mục đó và chạy:

```bash
git clone https://github.com/xduc199/XuanDucShop.git
```

Sau khi tải hoàn tất, truy cập thư mục dự án:

```bash
cd XuanDucShop
```

Kiểm tra các tệp và thư mục:

```bash
dir
```

Trên Linux hoặc macOS, có thể sử dụng `ls` thay cho `dir`.

### Bước 4. Kiểm tra tệp dự án

Xác định thư mục chứa tệp `.csproj`. Tệp này chứa cấu hình dự án .NET và các thông tin cần thiết để biên dịch.

Nếu tệp `.csproj` nằm ngay trong thư mục `XuanDucShop`, tiếp tục các bước tiếp theo tại thư mục hiện tại.

Nếu tệp nằm trong thư mục con, hãy chuyển đến đúng thư mục trước khi chạy các lệnh `dotnet`.

### Bước 5. Khôi phục các gói phụ thuộc

Chạy lệnh:

```bash
dotnet restore
```

Lệnh này khôi phục các gói NuGet mà dự án cần sử dụng.

Chờ đến khi quá trình hoàn tất. Nếu xuất hiện lỗi kết nối hoặc không tải được gói, hãy kiểm tra kết nối Internet, cấu hình NuGet và phiên bản SDK.

### Bước 6. Biên dịch dự án

Chạy:

```bash
dotnet build
```

Nếu biên dịch thành công, terminal sẽ hiển thị thông báo tương tự:

```text
Build succeeded.
```

Nếu xuất hiện lỗi, hãy đọc thông báo lỗi đầu tiên để xác định tệp và dòng mã cần sửa trước khi tiếp tục.

### Bước 7. Khởi chạy ứng dụng

Chạy lệnh:

```bash
dotnet run
```

Terminal sẽ hiển thị địa chỉ mà máy chủ web đang sử dụng, chẳng hạn `http://localhost:xxxx` hoặc `https://localhost:xxxx`.

**Lưu ý:** Hãy sử dụng đúng địa chỉ và cổng được hiển thị trong terminal, không mặc định rằng ứng dụng luôn chạy ở một cổng cố định.

### Bước 8. Truy cập giao diện

1. Giữ terminal đang chạy ứng dụng.
2. Mở trình duyệt như Chrome, Edge hoặc Firefox.
3. Nhập địa chỉ được hiển thị ở Bước 7.
4. Kiểm tra trang tổng quan và các mục quản lý danh mục, sản phẩm, nhân viên, hóa đơn.

Nếu muốn dừng ứng dụng, quay lại terminal và nhấn `Ctrl + C`.

### Bước 9. Chạy dự án bằng Visual Studio

Nếu muốn chạy trực tiếp trong Visual Studio:

1. Mở Visual Studio.
2. Chọn **Open a project or solution**.
3. Mở tệp `.sln` nếu dự án có cung cấp; nếu không, mở tệp `.csproj`.
4. Chờ Visual Studio tải dự án và khôi phục các gói phụ thuộc.
5. Chọn cấu hình chạy phù hợp.
6. Nhấn `F5` để chạy kèm gỡ lỗi hoặc `Ctrl + F5` để chạy không gỡ lỗi.
7. Trình duyệt sẽ mở ứng dụng nếu cấu hình khởi chạy được thiết lập phù hợp.

## 📁 5. Cấu trúc dự án

```text
XuanDucShop/
├── Controllers/       # Tiếp nhận và xử lý yêu cầu
├── Models/            # Các mô hình dữ liệu
│   └── ViewModels/    # Dữ liệu phục vụ giao diện
├── Services/          # Xử lý nghiệp vụ và quản lý dữ liệu
├── Views/             # Giao diện Razor
├── wwwroot/           # CSS, JavaScript và tài nguyên tĩnh
├── Program.cs         # Cấu hình và khởi chạy ứng dụng
└── README.md          # Tài liệu hướng dẫn dự án
```

## 🧪 6. Kiểm tra hoạt động

Sau khi chạy ứng dụng, có thể kiểm tra lần lượt:

- [ ] Trang tổng quan tải thành công.
- [ ] Các trang danh mục và sản phẩm hiển thị đúng.
- [ ] Có thể thử thêm, xem, chỉnh sửa và xóa dữ liệu.
- [ ] Các biểu mẫu từ chối dữ liệu không hợp lệ theo quy tắc đã triển khai.
- [ ] Các trang nhân viên và hóa đơn hoạt động bình thường.
- [ ] Giao diện hiển thị phù hợp khi thay đổi kích thước trình duyệt.

Đây là danh sách kiểm tra thủ công; không đồng nghĩa dự án đã có bộ kiểm thử tự động.

## 🛠️ 7. Xử lý lỗi thường gặp

| Lỗi | Cách xử lý |
|---|---|
| Không nhận diện lệnh `dotnet` | Cài .NET 10 SDK, kiểm tra `PATH` và mở lại terminal. |
| Không nhận diện lệnh `git` | Cài Git và kiểm tra biến môi trường. |
| Không tìm thấy tệp `.csproj` | Kiểm tra lại thư mục hiện tại và chuyển đến thư mục chứa dự án. |
| `dotnet restore` thất bại | Kiểm tra Internet, nguồn NuGet và phiên bản SDK. |
| `dotnet build` thất bại | Đọc thông báo lỗi, xác định tệp và dòng mã liên quan. |
| Cổng đang được sử dụng | Kiểm tra tiến trình đang chạy hoặc điều chỉnh cấu hình cổng phù hợp. |
| Trang web không truy cập được | Kiểm tra terminal, địa chỉ khởi chạy và thông báo lỗi của ứng dụng. |

## ⚠️ 8. Lưu ý về dữ liệu

Phiên bản hiện tại sử dụng **StoreData để lưu dữ liệu trong bộ nhớ**. Vì vậy:

- Dữ liệu có thể bị mất khi ứng dụng khởi động lại.
- Dữ liệu không được lưu bền vững vào cơ sở dữ liệu.
- Không nên sử dụng dữ liệu thử nghiệm này như dữ liệu vận hành thực tế.

Các tính năng như cơ sở dữ liệu SQL Server, xác thực người dùng và kiểm thử tự động có thể được bổ sung trong những phiên bản tiếp theo.

## 🗺️ 9. Định hướng phát triển

- [ ] Tích hợp Entity Framework Core và SQL Server.
- [ ] Xây dựng chức năng đăng nhập và phân quyền.
- [ ] Bổ sung tìm kiếm, lọc và phân trang.
- [ ] Bổ sung kiểm thử tự động.
- [ ] Hoàn thiện báo cáo và thống kê cửa hàng.
