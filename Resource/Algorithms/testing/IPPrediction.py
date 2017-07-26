import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D

data = np.array([
    [0,0,2],
    [1,1,1],
    [2,2,1],
    [3,3,1]
])


queryKey = [3,-1]
df = pd.DataFrame(data)
df.column = ['x','y' , 'z']

fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')

for i in range (len(data)):
    ax.scatter(df.ix[i][0], df.ix[i][1], df.ix[i][2] , c = 'r' , marker='o')


ax.set_xlabel("Unique IP ")
ax.set_ylabel("Unique MAC")
ax.set_zlabel("Count")
print(plt.show())
num_rows , num_cols = data.shape


print(num_rows)

# for i in range (num_rows):
#     plt.scatter(df.ix[i][0] , df.ix[i][1] , c='g')
# plt.scatter(queryKey[0] , queryKey[1] , c='y')

# I want to check what is the di
