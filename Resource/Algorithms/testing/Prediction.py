

import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import sys

def calNeigh(value,df,num_rows):
    numset = []
    for i in value[1][0]:
        numset.append(i)

    neighbourSet = []
    for a in numset:
        for i in range(num_rows):
            if (i == a):
                neighbourSet.append(df.ix[i])
    return neighbourSet;


def changeNeigCol(value, df, num_rows, plt):
    numset = []
    for i in value[1][0]:
        numset.append(i)

    for a in numset:
        for i in range(num_rows):
            if (i == a):
                plt.scatter(df.ix[i]['x'], df.ix[i]['y'], s=150, c='g')

    return plt

def calculateKneigh(xValue, yValue, trainingData):
    nearest1 = NearestNeighbors(n_neighbors = 5)
    nearest1.fit(trainingData)
    test = np.array([xValue, yValue,0])
    test1 = test.reshape(1,-1)
    value = nearest.kneighbors(test1,5)
    return value[1][0]

arg1 = sys.argv[1]
arg2 = sys.argv[2]

xTest = float(arg1)
yTest = float(arg2)

plt.xlim([0,24])
plt.xlabel('Hour user logs in')
plt.ylabel('Day of the week user logs in')

print("Generating data set")
data = np.array([
    [13.3, 3,'0'],
    [14.1,3,'0'],
    [15,2,'0'],
    [13.2,4,'0'],
    [12.5,5,'0'],
    [15.3,7,'0'],
    [14.3,1,'0'],
    [14.2,2,'0'],
    [12.5,3,'0'],
    [13,3,'0'],
    [12.3,4,'0'],
    [12.4,3,'0'],
    [13.3,1,'0'],
    [13.4,2,'0'],
    [13.2,3,'0'],
    [13.5,4,'0'],
    [13.36,5,'0'],
    [13.32,6,'0'],
    [13.39,2,'0'],
    [13.46,1,'0'],
    [13.21,2,'0'],
    [12.56,3,'0'],
    [13.12,4,'0'],
    [13.05,1,'0'],
    [12.31,2,'0'],
    [12.12,3,'0'],
    [12.1,6,'0'],
    [13.56,4,'0'],
    [11.45,3,'0'],
    [14.47,2,'0'],
    [14.49,6,'0'],
    [14.32,4,'0'],
    [15,7,'0'],
    [15.32,6,'0'],
    [15.56,7,'0'],
    [15.43,6,'0'],
    [15.25,7,'0'],
    [15.32,7,'0']


])

query = [xTest,yTest]
df = pd.DataFrame(data)
df.columns = ['x','y', 'type']

num_rows , num_cols = data.shape


for i in range(num_rows):
    if(df.ix[i]['type'] == '0'):
        plt.scatter(df.ix[i]['x'] , df.ix[i]['y'], s=150, c='r')
    else:
        plt.scatter(df.ix[i]['x'], df.ix[i]['y'], s=150, c='b')
    plt.scatter(query[0] , query[1] , s=200 , c='y')
#print(plt.show())

import math
print("Calculating distance from query vector and normal vector")
dis = []
for i in range(num_rows):
    firstPoint = float(df.ix[i]['x'])
    secondPoint = float(df.ix[i]['y'])
    dis.append((firstPoint - query[0]) ** 2 + (secondPoint - query[1]) ** 2)

df['dis'] = dis

#print(df.sort_values('dis'))

from sklearn.neighbors import NearestNeighbors
nearest = NearestNeighbors(n_neighbors=5)
nearest.fit(data)
test = np.array([xTest,yTest,0])
test1 = test.reshape(1,-1)
value = nearest.kneighbors(test1,5)
neighbourSet = calNeigh(value=value, df=df, num_rows=num_rows)
plt = changeNeigCol(value=value, df=df , num_rows=num_rows , plt=plt)

trueValue = value[1][0]
#print(df.ix[trueValue])

neighbourNeighbour = []
print(neighbourSet)

data1 = np.array([
    [13.3, 3,'0'],
    [14.1,3,'0'],
    [15,2,'0'],
    [13.2,4,'0'],
    [12.5,5,'0'],
    [15.3,7,'0'],
    [14.3,1,'0'],
    [14.2,2,'0'],
    [12.5,3,'0'],
    [13,3,'0'],
    [12.3,4,'0'],
    [12.4,3,'0'],
    [13.3,1,'0'],
    [13.4,2,'0'],
    [13.2,3,'0'],
    [13.5,4,'0'],
    [13.36,5,'0'],
    [13.32,6,'0'],
    [13.39,2,'0'],
    [13.46,1,'0'],
    [13.21,2,'0'],
    [12.56,3,'0'],
    [13.12,4,'0'],
    [13.05,1,'0'],
    [12.31,2,'0'],
    [12.12,3,'0'],
    [12.1,6,'0'],
    [13.56,4,'0'],
    [11.45,3,'0'],
    [14.47,2,'0'],
    [14.49,6,'0'],
    [14.32,4,'0'],
    [15,7,'0'],
    [15.32,6,'0'],
    [15.56,7,'0'],
    [15.43,6,'0'],
    [15.25,7,'0'],
    [15.32,7,'0'],
    [xTest,yTest,'0']

])

newdf = pd.DataFrame(data1)
newdf.columns = ['x','y', 'type']

for i in neighbourSet:
    x1 = i['x']
    y1 = i['y']
    newValue = calculateKneigh(xValue=x1, yValue = y1, trainingData=data1)
    neighbourNeighbour.append(newValue)
    print(newdf.ix[newValue])









count = 0

for i in neighbourNeighbour:
    for a in i:
        bestStart = xTest - 1
        bestEnd = xTest + 1

        secondStart = xTest - 2
        secondEnd = xTest + 2

        thirdStart = xTest -3
        thirdEnd = xTest + 3

        worstStart = xTest -4
        worstEnd = xTest + 4
        print(float(newdf.ix[a][0]))
        print(xTest)
        # Check to see if the neighbour is in the +-1 range of the "Query Vector"
        if(float(newdf.ix[a][0]) > bestStart and float(newdf.ix[a][0]) < bestEnd):
            count += 1
        elif(float(newdf.ix[a][0]) > secondStart and float(newdf.ix[a][0]) < secondEnd):
            count += 0.7
        elif (float(newdf.ix[a][0]) > thirdStart and float(newdf.ix[a][0]) < thirdEnd):
            count += 0.5
        elif (float(newdf.ix[a][0]) > worstStart and float(newdf.ix[a][0]) < worstEnd):
            count += 0.3



print(str(count))
probability = ""
if(float(count) >= 20):
    probability = "Extremly low possibility that x at " + str(xTest) + " and y at " + str(yTest) + " is an anomaly"
elif(float(count) >= 15):
    probability = "Low possibility that x at " + str(xTest) + " and y at " + str(yTest) + " isan anomaly"
elif(float(count) >= 8):
    probability = "Medium possibility that x at " + str(xTest) + " and y at " + str(yTest) + " is an anomaly"
elif(float(count) >= 3):
    probability = "High possibility that x at " + str(xTest) + " and y at " + str(yTest) + " is an anomaly"
elif(float(count) < 3):
    probability = "Extremly high possibility that x at " + str(xTest) + " and y at " + str(yTest) + " is an anomaly"
print(probability)
plt.show()






