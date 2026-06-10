# PersonalBlogApp

Dự án Blog cá nhân được phát triển bằng .NET 8 Razor Pages.

## Cấu hình Database

Dự án sử dụng SQL Server LocalDB. Dưới đây là mẫu chuỗi kết nối (Connection String) trong `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PersonalBlogAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

## Seed Data
Sau khi chạy ứng dụng lần đầu, cơ sở dữ liệu sẽ tự động được tạo và seed:
- **Admin User**: `admin@example.com` / `Admin@123`
- **Blogs**: 3 bài viết mẫu.
