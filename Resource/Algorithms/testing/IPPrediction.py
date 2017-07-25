import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

data = np.array([
    [0,5],
    [1,2],
    [2,1]
])

queryKey = 4
df = pd.DataFrame(data)
df.column = ['x','y']
plt.xlabel("Key Value (Each Key Represents An IP address")
plt.ylabel("Number of times this value appeared")

num_rows , num_cols = data.shape

print(num_rows)
for i in range (num_rows):
    plt.scatter(df.ix[i][0] , df.ix[i][1] , c='y')

riskLevel = 1
for i in range(num_rows):
    if(queryKey == df.ix[i][0]):
        print(str(queryKey) + " has appeared before")
        riskLevel = 0

print(plt.show())
# I want to check what is the di
