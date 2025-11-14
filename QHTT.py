from pulp import *

# 1. Khởi tạo Mô hình Bài toán
# Tên bài toán và mục tiêu (LpMaximize hoặc LpMinimize)
model = LpProblem("Toi_Da_Hoa_Z", LpMinimize)

# 2. Khai báo Biến Quyết định
# lowBound=0 là điều kiện mặc định >= 0
x1 = LpVariable("x1", lowBound=0)
x2 = LpVariable("x2", lowBound=0)
x3 = LpVariable("x3", lowBound=0)

# 3. Định nghĩa Hàm Mục tiêu
# Phép cộng và nhân được PuLP nạp chồng (overload)
model += -4 * x1 - x2 + 2 * x3, "Ham_Muc_Tieu_Z"

# 4. Định nghĩa các Ràng buộc
# PuLP trực tiếp hiểu các toán tử <=, >=, ==
model += -6 * x1 + x2 - 2 * x3 <= 24, "Rong_Buoc_1"
model += -2 * x1 - 7 * x2 - 2 * x3 >= -39, "Rong_Buoc_2"
model += 5 * x1 + x2 + 2 * x3 <= 26, "Rong_Buoc_3"

# 5. Giải Bài toán
print("--- Bắt đầu Giải bằng PuLP ---")
model.solve()

# 6. Hiển thị Trạng thái và Kết quả
#print(f"Trạng thái Giải: {LpStatus[model.status]}")

if LpStatus[model.status] == 'Optimal':
    print("\n--- Kết quả Tối ưu ---")

    # In giá trị tối ưu của các biến
    print(f"* x1 = {value(x1):.4f}")
    print(f"* x2 = {value(x2):.4f}")
    print(f"* x3 = {value(x3):.4f}")

    # In giá trị tối đa của Hàm Mục tiêu
    print(f"\n**Giá trị TỐI ĐA của Z là: {value(model.objective):.4f}**")
else:
    print("Không tìm thấy giải pháp tối ưu.")
