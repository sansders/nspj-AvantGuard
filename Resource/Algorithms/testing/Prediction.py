

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
    test = np.array([xValue, yValue])
    test1 = test.reshape(1,-1)
    value = nearest.kneighbors(test1,5)
    return value[1][0]

def convertCommandLineArg():
    testing = []
    for i in range (3, totalSize):
        myvar = sys.argv[i]
        testing.append(float(myvar))   

    final = []

    for i in range (0, len(testing) , 2): 
        newValue = [testing[i] , testing[i+1]]
        final.append(newValue);

    datatest = np.empty([len(final), 2])
    for i in range (len(final)):
        datatest[i] = final[i]
    return datatest


totalSize = len(sys.argv)
arg1 = sys.argv[1]
arg2 = sys.argv[2]

xTest = float(arg1)
yTest = float(arg2)

plt.xlim([0,24])
plt.ylim([0,8])
plt.xlabel('Hour user logs in')
plt.ylabel('Day of the week user logs in')

#print("Generating data set")
#testing = np.empty(totalSize - 3)
data = convertCommandLineArg()

#newdata = np.insert(datatest, len(datatest), [xTest,yTest] , axis= 0 )

#print(newdata)
#data = np.array([
#    [13.3, 3 ],
#    [14.1,3 ],
#    [15,2 ],
#    [13.2,4 ],
#    [12.5,5 ],
#    [15.3,7 ],
#    [14.3,1 ],
#    [14.2,2 ],
#    [12.5,3 ],
#    [13,3 ],
#    [12.3,4 ],
#    [12.4,3 ],
#    [13.3,1 ],
#    [13.4,2 ],
#    [13.2,3 ],
#    [13.5,4 ],
#    [13.36,5 ],
#    [13.32,6 ],
#    [13.39,2 ],
#    [13.46,1 ],
#    [13.21,2 ],
#    [12.56,3 ],
#    [13.12,4 ],
#    [13.05,1 ],
#    [12.31,2 ],
#    [12.12,3 ],
#    [12.1,6 ],
#    [13.56,4 ],
#    [11.45,3 ],
#    [14.47,2 ],
#    [14.49,6 ],
#    [14.32,4 ],
#    [15,7 ],
#    [15.32,6 ],
#    [15.56,7 ],
#    [15.43,6 ],
#    [15.25,7 ],
#    [15.32,7 ]


#])

query = [xTest,yTest]
df = pd.DataFrame(data)
df.columns = ['x','y']

num_rows , num_cols = data.shape


for i in range(num_rows):
        plt.scatter(df.ix[i]['x'] , df.ix[i]['y'], s=150, c='r')
 

plt.scatter(query[0] , query[1] , s=200 , c='y')
#print(plt.show())

import math
#print("Calculating distance from query vector and normal vector")
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
test = np.array([xTest,yTest])
test1 = test.reshape(1,-1)
value = nearest.kneighbors(test1,5)
neighbourSet = calNeigh(value=value, df=df, num_rows=num_rows)
plt = changeNeigCol(value=value, df=df , num_rows=num_rows , plt=plt)

trueValue = value[1][0]
#print(df.ix[trueValue])

neighbourNeighbour = []
#print(neighbourSet)

data1 = np.insert(data, len(data), [xTest,yTest] , axis= 0 )
#data1 = np.array([
#    [13.3, 3 ],
#    [14.1,3 ],
#    [15,2 ],
#    [13.2,4 ],
#    [12.5,5 ],
#    [15.3,7 ],
#    [14.3,1 ],
#    [14.2,2 ],
#    [12.5,3 ],
#    [13,3 ],
#    [12.3,4 ],
#    [12.4,3 ],
#    [13.3,1 ],
#    [13.4,2 ],
#    [13.2,3 ],
#    [13.5,4 ],
#    [13.36,5 ],
#    [13.32,6 ],
#    [13.39,2 ],
#    [13.46,1 ],
#    [13.21,2 ],
#    [12.56,3 ],
#    [13.12,4 ],
#    [13.05,1 ],
#    [12.31,2 ],
#    [12.12,3 ],
#    [12.1,6 ],
#    [13.56,4 ],
#    [11.45,3 ],
#    [14.47,2 ],
#    [14.49,6 ],
#    [14.32,4],
#    [15,7],
#    [15.32,6],
#    [15.56,7],
#    [15.43,6],
#    [15.25,7],
#    [15.32,7],
#    [xTest,yTest]

#])

newdf = pd.DataFrame(data1)
newdf.columns = ['x','y']

for i in neighbourSet:
    x1 = i['x']
    y1 = i['y']
    newValue = calculateKneigh(xValue=x1, yValue = y1, trainingData=data1)
    neighbourNeighbour.append(newValue)
    #print(newdf.ix[newValue])









count = 0
neighDist = []
for i in neighbourNeighbour:
    for a in i:
        bestStart = xTest - 2
        bestEnd = xTest + 2

        secondStart = xTest - 3
        secondEnd = xTest + 3

        thirdStart = xTest -4
        thirdEnd = xTest + 4

        worstStart = xTest -5
        worstEnd = xTest + 5
        #print(float(newdf.ix[a][0]))
        #print(xTest)

        neighDist.append((newdf.ix[a][0] - query[0]) ** 2 + (newdf.ix[a][1] - query[1]) ** 2)
        # Check to see if the neighbour is in the +-1 range of the "Query Vector"
        #if(float(newdf.ix[a][0]) > bestStart and float(newdf.ix[a][0]) < bestEnd):
        #    count += 1
        #elif(float(newdf.ix[a][0]) > secondStart and float(newdf.ix[a][0]) < secondEnd):
        #    count += 0.7
        #elif (float(newdf.ix[a][0]) > thirdStart and float(newdf.ix[a][0]) < thirdEnd):
        #    count += 0.5
        #elif (float(newdf.ix[a][0]) > worstStart and float(newdf.ix[a][0]) < worstEnd):
        #    count += 0.3

total = 0
for i in neighDist:
    total += i

total = total / len(neighDist)
#print(total)
riskLevel = 0
probability = ""
if(float(total) <= 15):
    probability = "Low possibility that the entry at " + str(xTest) + " on day " + str(yTest) + " is an anomaly"
    riskLevel = 1
elif(float(total) <= 20):
    probability = "Low Medium possibility that the entry at " + str(xTest) + " on day " + str(yTest) + " is an anomaly"
    riskLevel = 2
elif(float(total) <= 25):
    probability = "Medium possibility that the entry at " + str(xTest) + " on day " + str(yTest) + " is an anomaly"
    riskLevel = 3
elif(float(total) <= 30):
    probability = "Medium High possibility that the entry at " + str(xTest) + " on day " + str(yTest) + " is an anomaly"
    riskLevel = 4
elif(float(total) > 30):
    probability = "High possibility that the entry at " + str(xTest) + " on day " + str(yTest) + " is an anomaly"
    riskLevel = 5
plt.show()
print(str(riskLevel) + "/" + str(probability))







