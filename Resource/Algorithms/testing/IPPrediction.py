import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from scipy.stats import skew as sk
from mpl_toolkits.mplot3d import Axes3D
from sklearn.cluster import MeanShift
from sklearn.datasets.samples_generator import make_blobs






# centers = [[1,1], [5,5]]
# X , _ = make_blobs(n_samples= 30, centers = centers, cluster_std= 1)
# plt.scatter(X[:,0] , X[:,1])
# print(plt.show())
#
# ms = MeanShift()
# ms.fit(X)
# labels = ms.labels_
# cluster_centers = ms.cluster_centers_
#
# n_clusters_ = len(np.unique(labels))
# print("Number of estimated clusters : " ,  n_clusters_)
# colors = 10 * ['r.', 'g.' , 'b.' , 'c.', 'k.', 'y.', 'm.']
#
# print(colors)
# print(labels)
#
# for i in range (len(X)):
#     plt.plot(X[i][0], X[i][1], colors[labels[i]] , markersize = 10)
#
# plt.scatter(cluster_centers[:,0] , cluster_centers[:,1],
#             marker = "x" , s = 150
#             )
#
# print(plt.show())
data = np.array([
    # [0,0,6],
    # [1,1,6],
    # [2,2,7],
    # [3,3,5],
    # [4,4,6],
    # [5,5,5]

    [0,0,11],
    [1,1,12],
    [2,2,7],
    [3,3,5],
    [4,4,6],
    [5,5,6],
    [6,6,3],
    [7,7,3]
])

data1 = np.array([
    [2],
    [1],
    [1],
    [4],
    [4]
])


queryKey = [7,3, 1]
df = pd.DataFrame(data)
df.column = ['x','y' , 'z']

total = 0
for i in range (len(data)):
    total += df.ix[i][2]

chance = 1 / len(data)
print(7/(11+ 12 + 7 + 5+ 6+ 5))
setValue = []
for i in range(len(data)):
    probability = df.ix[i][2] / total
    if(probability > chance-0.05):
        setValue.append([df.ix[i][0], df.ix[i][1]])
print(setValue)
print(df.skew())
skewness = sk(data, axis=0 , bias=False )
skewed = bool
if( -0.5 < skewness[2] < 0.5 ):
    print("The data is approximately normally distributed with skewness of " , skewness[2])
    skewed = False
else:
    print("The data is skewed with skewness of " , skewness[2])
    skewed = True

xArray = []
yArray = []
zArray = []
exactArray = []
for i in range (len(data)):
    xArray.append(df.ix[i][0])
    yArray.append(df.ix[i][1])
    zArray.append(df.ix[i][2])
    exactArray.append([df.ix[i][0] , df.ix[i][1]])
totalCount = 0

xExact = []
yExact = []


for i in range ((len(setValue))):
    xExact.append(setValue[i][0])
    yExact.append(setValue[i][1])

print(xExact)

if(skewed == True):
    print("Is skewed")
    array = [queryKey[0] , queryKey[1]]
    if(array in setValue):
        print("Record has appeared many times before")
        totalCount += 0
    else:
        print("Record hasn't appeared frequently")
        print("Checking with the less frequent records")
        if(array in exactArray):
            print("Record has appeared before, but not as frequently")
            totalCount += 0.10
        else:
           print("Exact Record has't occured before")
           print("Checking frequent IP and MAC addresses")
           if(queryKey[0] in xExact):
               print("IP has appeared frequently")
               print("Checking MAC address")
               if(queryKey[1] in yExact):
                   print("IP and MAC address has appeared frequently")
                   totalCount += 0.25
               else:
                   print("MAC address hasn't appeared frequently")
                   print("Checking with less frequent mac address")
                   if(queryKey[1] in yArray):
                       print("IP appears frequently but MAC address has appeared but not so frequently")
                       totalCount += 0.25
                   else:
                       print("IP has appeared frequently but MAC address has not appeared before")
                       totalCount += 0.5
           else:
               print("IP hasn't appeared frequently")
               print("Checking MAC address")
               if(queryKey[1] in yExact):
                   if(queryKey[0] in xArray):
                       print("IP has not appeared frequently but MAC address have")
                       totalCount += 0.25
                   else:
                       print("IP has not appeared before but MAC address have")
                       totalCount += 0.5
               else:
                   print("IP and MAC address has not appeared frequently")
                   print("Checking with less frequent data")
                   if(queryKey[0] in xArray):
                       print("IP has appeared before but not so frequent")
                       print("Checking MAC address")
                       if(queryKey[1] in yArray):
                            print("IP and MAC address has appeared but not so frequently")
                            totalCount += 0.25
                       else:
                           print("IP has appeared sometimes but MAC address has not appeared")
                           totalCount += 0.25
                   else:
                       print("IP has not appeared frequently")
                       print("Checking MAC address")
                       if(queryKey[1] in yArray):
                           print("IP and MAC address has not appeared frequently")
                           totalCount += 0.25
                       else:
                           print("IP and MAC address has never appeared before")
                           totalCount += 1
elif (skewed != True):
    print("Is not skewed")
    array = [queryKey[0] , queryKey[1]]
    if(array in exactArray):
        print("Record has appeared before")
        totalCount += 0
    else:
        if(queryKey[0] in xArray):
           print("IP is matched")
           print("Checking MAC address")
           if(queryKey[1] in yArray):
               print("Both IP and MAC address has occured before")
               totalCount += 0.25
           else:
               print("IP has appeared but MAC address hasn't appeared before")
               totalCount += 0.5
        else:
            print("IP is not matched")
            print("Checking MAC address")
            if(queryKey[1] in yArray):
                print("IP has not appeared but MAC address has appeared before")
                totalCount += 0.5
            else:
                print("IP and MAC address has both not appeared before")
                totalCount += 1.0



# if(skewed == True):
#         if(queryKey[0] in setValue[i][0]):
#             if(queryKey[1] in setValue[1]):
#                 totalCount += -1
#             else:
#                 totalCount += 0.5
#         else:
#             if(queryKey[1] in setValue[1]):
#                 totalCount += 0.5
#             else:
#                 if(queryKey[0] in data[0]):
#                     if(queryKey[0] [1]):
#                         totalCount += -1
#                     else:
#                         totalCount += 1
#                 else:
#                     if(queryKey[1] in data[1]):
#                         totalCount += 1
#                     else:
#                         totalCount += 2
# else:
#     if(queryKey[0] in data[0]):
#         if(queryKey[1] in data[1]):
#             totalCount += -1
#         else:
#             totalCount += 0.5
#     else:
#         if(queryKey[1] in data[1]):
#             totalCount += 0.5
#         else:
#             totalCount += 1
#

print(totalCount)





fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')
for i in range (len(data)):
    ax.scatter(df.ix[i][0], df.ix[i][1], df.ix[i][2] , c = 'g' , marker='o')
ax.scatter(queryKey[0], queryKey[1], queryKey[2] , c = 'y', marker = 'o')
ax.set_xlabel("Unique IP ")
ax.set_ylabel("Unique MAC")
ax.set_zlabel("Count")
counterX = len(data)

ax.set_xticks(np.arange(min(xArray), max(xArray)+ 1, 1))
ax.set_yticks(np.arange(min(yArray), max(yArray)+ 1, 1))
ax.set_zticks(np.arange(min(zArray), max(zArray)+ 1, 1))
print(plt.show())