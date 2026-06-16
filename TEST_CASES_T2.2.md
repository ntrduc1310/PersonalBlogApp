# Test Cases - Task 2.2: Role-Based Authorization

**Mục đích:** Kiểm tra role-based authorization hoạt động đúng - Admin access được, User bị block, và người chưa đăng nhập redirect về login.

**Ngày test:** 2026-06-11  
**Người test:** Derek  
**Kết quả:**  PASS

---

## Tài khoản test

| Email | Password | Role |
|-------|----------|------|
| admin@example.com | Admin@123 | Admin |
| user@example.com | User@123 | User |
| (không đăng nhập) | - | None |

---

## Test Case

### TC-2.2.1: Chưa đăng nhập → Redirect về login

**Làm gì:**
1. Vào URL: `http://localhost:5000/Management/Users`
2. Không đăng nhập gì cả

**Kết quả:**
 PASS - Tự động redirect về `/Identity/Account/Login` với ReturnUrl

**Ghi chú:** 302 Redirect - OK

### TC-2.2.2: Admin → Access được

**Làm gì:**
1. Login: `admin@example.com` / `Admin@123`
2. Vào `/Management/Users`

**Kết quả:**
 PASS - Hiển thị trang: "Admin Dashboard - User Management" (200 OK)

**Ghi chú:** Admin có role đúng, access OK

### TC-2.2.3: User → Bị block

**Làm gì:**
1. Login: `user@example.com` / `User@123`
2. **Gõ URL trực tiếp** trong address bar: `http://localhost:5000/Management/Users`
   - *Ghi chú: User không thấy link "Admin" ở navbar (đó là đúng), nên phải gõ URL manually để test access control*

**Kết quả:**
PASS - Redirect về `/Identity/Account/AccessDenied` (403 Forbidden)
- Thấy lỗi: "You do not have access to this resource."

**Ghi chú:** User không có role Admin → bị block OK (server-side authorization chặn)

---

## Kết quả

**Test Cases Passed:** 3/3 (TC-2.2.1, TC-2.2.2, TC-2.2.3) 

**Acceptance Criteria:**
- [x] USER bị block từ `/Management/Users` (admin-only)
- [x] ADMIN access được tất cả routes  
- [x] Chưa đăng nhập → Redirect về login
- [x] `[Authorize(Roles="Admin")]` đã apply
- [x] Authorization policies đã config

---

## Kết luận

 **Task 2.2 DONE**

Tất cả 3 test case chính đã pass:
- Unauthenticated users redirect → login 
- Admin access → success 
- User access → blocked 

Authorization feature hoạt động đúng theo yêu cầu.

