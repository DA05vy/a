import pulp
#Dữ liệu
supply = [260, 305, 255, 200]
demand = [260, 385, 390, 65]
cost = [
    [5,3,4,3],
    [4,7,2,1],
    [2,5,1,6],
    [7,4,3,5]
]

#Chỉ số
suppliers = range(4)
customers = range(4)

#Tạo bài toán
prob = pulp.LpProblem("Transport_Problem", pulp.LpMinimize)

#Biến quyết định
x = [[pulp.LpVariable(f"x_{i}_{j}", lowBound=0, cat="Continuous") for j in customers ] for i in suppliers]

#Hàm mục tiêu
prob += pulp.lpSum(cost[i][j]*x[i][j] for i in suppliers for j in customers )

#RÀng buộc cung
for i in suppliers:
    prob += pulp.lpSum(x[i][j] for j in customers ) == supply[i]

#Ràng buộc cầu
for j in customers:
    prob += pulp.lpSum(x[i][j] for i in suppliers ) == demand[j]

#Giải bài toán
prob.solve()

#In kết quả
print("Tong chi phi toi uu: ", pulp.value(prob.objective))
for i in suppliers:
    for j in customers:
        if x[i][j].varValue > 0:
            print ( f"x[{i+1}][{j+1}] = {x[i][j].varValue}")

