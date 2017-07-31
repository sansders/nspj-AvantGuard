import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
import matplotlib.pyplot as plt
import sys
from scipy.stats import skew as sk
from mpl_toolkits.mplot3d import Axes3D
from matplotlib.backends.backend_pdf import PdfPages

def classfierNormal(skewed, exactMatch , exactNot , ipmatch , macmatch , ipnot , macnot):
    if (skewed == False and
                concat in exactMatch):
        print("Exact Match Found Same Day Set")
        riskLevel = 0

    if (skewed == False and
                concat not in exactMatch and
                concat in exactNot):
        print("Exact Match Found in Other set")
        riskLevel = 0.1

    if (skewed == False and
                concat not in exactMatch and
                concat not in exactNot and
                queryIP in ipmatch and
                queryMAC in macmatch):
        print("No exact matches")
        print("IP matches same day set")
        print("Mac matches same day set")
        ipRisk = 0.3
        macRisk = 0.3
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == False and
                concat not in exactMatch and
                concat not in exactNot and
                queryIP in ipmatch and
                queryMAC not in macmatch and
                queryMAC in macnot):
        print("No exact matches")
        print("IP matches same day set")
        print("Mac don't match same day set")
        print("Mac match other day set")
        ipRisk = 0.3
        macRisk = 0.6
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == False and
                concat not in exactMatch and
                concat not in exactNot and
                queryIP in ipmatch and
                queryMAC not in macmatch and
                queryMAC not in macnot):
        print("No exact matches")
        print("IP matches same day set")
        print("Mac don't match same day set")
        print("Mac don't match other day set")
        ipRisk = 0.3
        macRisk = 1.0
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == False and
                concat not in exactMatch and
                concat not in exactNot and
                queryIP not in ipmatch and
                queryIP in ipnot and
                queryMAC in macmatch):
        print("No exact matches")
        print("IP don't matches same day set")
        print("IP matches other day set")
        print("Mac match same day set")
        ipRisk = 0.5
        macRisk = 0.3
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == False and
                concat not in exactMatch and
                concat not in exactNot and
                queryIP not in ipmatch and
                queryIP in ipnot and
                queryMAC not in macmatch and
                queryMAC in macnot):
        print("No exact matches")
        print("IP don't matches same day set")
        print("IP matches other day set")
        print("Mac don't match same day set")
        print("Mac match other day set")
        ipRisk = 0.5
        macRisk = 0.6
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == False and
                concat not in exactMatch and
                concat not in exactNot and
                queryIP not in ipmatch and
                queryIP in ipnot and
                queryMAC not in macmatch and
                queryMAC not in macnot):
        print("No exact matches")
        print("IP don't matches same day set")
        print("IP matches other day set")
        print("Mac don't match same day set")
        print("Mac don't match other day set")
        ipRisk = 0.5
        macRisk = 1.0
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == False and
                concat not in exactMatch and
                concat not in exactNot and
                queryIP not in ipmatch and
                queryIP not in ipnot and
                queryMAC in macmatch):
        print("No exact matches")
        print("IP don't matches same day set")
        print("IP don't matches other day set")
        print("Mac match same day set")
        ipRisk = 0.9
        macRisk = 0.3
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == False and
                concat not in exactMatch and
                concat not in exactNot and
                queryIP not in ipmatch and
                queryIP not in ipnot and
                queryMAC not in macmatch and
                queryMAC in macnot):
        print("No exact matches")
        print("IP don't matches same day set")
        print("IP don't matches other day set")
        print("Mac don't match same day set")
        print("MAC match other day set")
        ipRisk = 0.9
        macRisk = 0.6
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == False and
                concat not in exactMatch and
                concat not in exactNot and
                queryIP not in ipmatch and
                queryIP not in ipnot and
                queryMAC not in macmatch and
                queryMAC not in macnot):
        print("No exact matches")
        print("IP don't matches same day set")
        print("IP don't matches other day set")
        print("Mac don't match same day set")
        print("MAC don't other day set")
        ipRisk = 0.9
        macRisk = 1.0
        riskLevel = calculateRisk(ipRisk, macRisk)
    return riskLevel

def classfierSkewed(skewed, dayprioritySet , dayleftoverSet , otherPrioritySet , otherleftoverSet , daypriorityip ,
                    dayprioritymac , dayleftoverip , dayleftovermac, otherpriorityip, otherprioritymac , otherleftoverip , otherleftovermac , concat):
    if (skewed == True and
                concat in dayprioritySet):
        print("Exact Match Found In Day Priority Set (1st Tier)")
        riskLevel = 0

    if (skewed == True and
                concat not in dayprioritySet and
                concat in dayleftoverSet):
        print("Exact Match Found in Day Leftover Set (2nd Tier)")
        riskLevel = 0.1

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat in otherPrioritySet):
        print("Exact Match Found in Other Priority Set (3rd Tier)")
        riskLevel = 0.1

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat in otherleftoverSet):
        print("Exact Match Found in Other leftover set (4th Tier)")
        riskLevel = 0.2

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP in daypriorityip and
                queryMAC in dayprioritymac):
        print("No exact matches")
        print("IP matches day priority ip set")
        print("MAC matches day priority mac set")
        ipRisk = 0.3
        macRisk = 0.3
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP in daypriorityip and
                queryMAC not in dayprioritymac and
                queryMAC in dayleftovermac):
        print("No exact matches")
        print("IP matches day priority ip set")
        print("MAC does not match day priority mac set")
        print("MAC matches day leftover mac set")
        ipRisk = 0.3
        macRisk = 0.5
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP in daypriorityip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC in otherprioritymac):
        print("No exact matches")
        print("IP matches day priority ip set")
        print("MAC does not match day priority mac set")
        print("MAC does not match day leftover mac set")
        print("MAC matches other priority mac set")
        ipRisk = 0.3
        macRisk = 0.6
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP in daypriorityip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC not in otherprioritymac and
                queryMAC in otherleftovermac):
        print("No exact matches")
        print("IP matches day priority ip set")
        print("MAC does not match day priority mac set")
        print("MAC does not match day leftover mac set")
        print("MAC does not match other priority mac set")
        print("MAC matches other leftover priority set")
        ipRisk = 0.3
        macRisk = 0.7
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP in daypriorityip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC not in otherprioritymac and
                queryMAC not in otherleftovermac):
        print("No exact matches")
        print("IP matches day priority ip set")
        print("MAC does not match day priority mac set")
        print("MAC does not match day leftover mac set")
        print("MAC does not match other priority mac set")
        print("MAC does not match other leftover mac set")
        ipRisk = 0.3
        macRisk = 1.0
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP in dayleftoverip and
                queryMAC in dayprioritymac):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP matches day leftover ip set")
        print("MAC matches day priority mac set")
        ipRisk = 0.5
        macRisk = 0.3
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP in dayleftoverip and
                queryMAC not in dayprioritymac and
                queryMAC in dayleftovermac):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP matches day leftover ip set")
        print("MAC does not match day priority mac set")
        print("MAC matches day leftover mac set")
        ipRisk = 0.5
        macRisk = 0.5
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP in dayleftoverip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC in otherprioritymac):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP matches day leftover ip set")
        print("MAC does not match day priority mac set")
        print("MAC does not match day leftover mac set")
        print("MAC matches other priority mac set")
        ipRisk = 0.5
        macRisk = 0.6
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP in dayleftoverip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC not in otherprioritymac and
                queryMAC in otherleftovermac):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP matches day leftover ip set")
        print("MAC does not match day priority mac set")
        print("MAC does not match day leftover mac set")
        print("MAC does not match other priority mac set")
        print("MAC matches other leftover mac set")
        ipRisk = 0.5
        macRisk = 0.7
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP in dayleftoverip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC not in otherprioritymac and
                queryMAC not in otherleftovermac):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP matches day leftover ip set")
        print("MAC does not match day priority mac set")
        print("MAC does not match day leftover mac set")
        print("MAC does not match other priority mac set")
        print("MAC does not match other leftover mac set")
        ipRisk = 0.5
        macRisk = 1.0
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP in otherpriorityip and
                queryMAC in dayprioritymac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP matches other priority ip set")
        print("MAC matches day priority mac set")
        ipRisk = 0.5
        macRisk = 0.3
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP in otherpriorityip and
                queryMAC not in dayprioritymac and
                queryMAC in dayleftovermac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP matches other priority ip set")
        print("MAC does not match day priority mac set ")
        print("MAC matches day leftover mac set")
        ipRisk = 0.5
        macRisk = 0.5
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP in otherpriorityip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC in otherprioritymac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP matches other priority ip set")
        print("MAC does not match day priority mac set ")
        print("MAC does not match day leftover mac set")
        print("MAC matches other priority mac set")
        ipRisk = 0.5
        macRisk = 0.6
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP in otherpriorityip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC not in otherprioritymac and
                queryMAC in otherleftovermac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP matches other priority ip set")
        print("MAC does not match day priority mac set ")
        print("MAC does not match day leftover mac set")
        print("MAC does not match other priority mac set")
        print("MAC matches other leftover mac set")
        ipRisk = 0.5
        macRisk = 0.7
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP in otherpriorityip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC not in otherprioritymac and
                queryMAC not in otherleftovermac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP matches other priority ip set")
        print("MAC does not match day priority mac set ")
        print("MAC does not match day leftover mac set")
        print("MAC does not match other priority mac set")
        print("MAC does not mtatches other leftover mac set")
        ipRisk = 0.5
        macRisk = 1.0
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP not in otherpriorityip and
                queryIP in otherleftoverip and
                queryMAC in dayprioritymac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP does not matches other priority ip set")
        print("IP matches other leftover ip set")
        print("MAC matches day priority mac set")
        ipRisk = 0.7
        macRisk = 0.3
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP not in otherpriorityip and
                queryIP in otherleftoverip and
                queryMAC not in dayprioritymac and
                queryMAC in dayleftovermac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP does not matches other priority ip set")
        print("IP matches other leftover ip set")
        print("MAC does not matches day priority mac set")
        print("MAC matches day leftover mac set")
        ipRisk = 0.7
        macRisk = 0.5
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP not in otherpriorityip and
                queryIP in otherleftoverip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC in otherprioritymac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP does not matches other priority ip set")
        print("IP matches other leftover ip set")
        print("MAC does not matches day priority mac set")
        print("MAC does not matches day leftover mac set")
        print("MAC matches other priority mac set")
        ipRisk = 0.7
        macRisk = 0.6
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP not in otherpriorityip and
                queryIP in otherleftoverip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC not in otherprioritymac and
                queryMAC in otherleftovermac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP does not matches other priority ip set")
        print("IP matches other leftover ip set")
        print("MAC does not matches day priority mac set")
        print("MAC does not matches day leftover mac set")
        print("MAC does not matches other priority mac set")
        print("MAC matches other leftover mac set")
        ipRisk = 0.7
        macRisk = 0.7
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP not in otherpriorityip and
                queryIP in otherleftoverip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC not in otherprioritymac and
                queryMAC not in otherleftovermac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP does not match other priority ip set")
        print("IP matches other leftover ip set")
        print("MAC does not matches day priority mac set")
        print("MAC does not matches day leftover mac set")
        print("MAC does not matches other priority mac set")
        print("MAC does not matches other leftover mac set")
        ipRisk = 0.7
        macRisk = 1.0
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP not in otherpriorityip and
                queryIP not in otherleftoverip and
                queryMAC in dayprioritymac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP does not match other priority ip set")
        print("IP does not match other leftover ip set")
        print("MAC matches day priority mac set")
        ipRisk = 0.9
        macRisk = 0.3
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP not in otherpriorityip and
                queryIP not in otherleftoverip and
                queryMAC not in dayprioritymac and
                queryMAC in dayleftovermac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP does not match other priority ip set")
        print("IP does not match other leftover ip set")
        print("MAC does not matches day priority mac set")
        print("MAC matches day leftover mac set")
        ipRisk = 0.9
        macRisk = 0.5
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP not in otherpriorityip and
                queryIP not in otherleftoverip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC in otherprioritymac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP does not match other priority ip set")
        print("IP does not match other leftover ip set")
        print("MAC does not matches day priority mac set")
        print("MAC does not matches day leftover mac set")
        print("MAC matches other priority mac set")
        ipRisk = 0.9
        macRisk = 0.6
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP not in otherpriorityip and
                queryIP not in otherleftoverip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC not in otherprioritymac and
                queryMAC in otherleftovermac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP does not match other priority ip set")
        print("IP does not match other leftover ip set")
        print("MAC does not matches day priority mac set")
        print("MAC does not matches day leftover mac set")
        print("MAC does not matches other priority mac set")
        print("MAC matches other leftover mac set")
        ipRisk = 0.9
        macRisk = 0.7
        riskLevel = calculateRisk(ipRisk, macRisk)

    if (skewed == True and
                concat not in dayprioritySet and
                concat not in dayleftoverSet and
                concat not in otherPrioritySet and
                concat not in otherleftoverSet and
                queryIP not in daypriorityip and
                queryIP not in dayleftoverip and
                queryIP not in otherpriorityip and
                queryIP not in otherleftoverip and
                queryMAC not in dayprioritymac and
                queryMAC not in dayleftovermac and
                queryMAC not in otherprioritymac and
                queryMAC not in otherleftovermac
        ):
        print("No exact matches")
        print("IP does not match day priority ip set")
        print("IP does not match day leftover ip set")
        print("IP does not match other priority ip set")
        print("IP does not match other leftover ip set")
        print("MAC does not matches day priority mac set")
        print("MAC does not matches day leftover mac set")
        print("MAC does not matches other priority mac set")
        print("MAC does not matches other leftover mac set")
        ipRisk = 0.9
        macRisk = 1.0
        riskLevel = calculateRisk(ipRisk, macRisk)
    return riskLevel

def multipage(filename, figs=None, dpi=200):
    pp = PdfPages(filename)
    if figs is None:
        figs = [plt.figure(n) for n in plt.get_fignums()]
    for fig in figs:
        fig.savefig(pp, format='pdf')
    pp.close()

def getIPFromSet(dataset):
    array = []
    for i in range (len(dataset)):
        value = dataset[i][0]
        array.append(int(value))
    return array

def getMACFromSet(dataset):
    array = []
    for i in range(len(dataset)):
        value = dataset[i][1]
        array.append(int(value))
    return array;

def calculateRisk(ipPercent , macPercent):
    value1 = (ipPercent /100) * 40
    value2 = (macPercent / 100) * 60
    risk = value1 + value2
    return risk

def evaulateRiskLevel(riskLevel):
    stringdata = "nothing"
    if(riskLevel < 0.5):
        stringdata = "low risk"
    elif(riskLevel < 0.75):
        stringdata = "medium risk"
    elif(riskLevel >= 0.75):
        stringdata = "high risk"
    return stringdata

#data = np.array([
#    # [0, 0, 4, 11],
#    # [1, 0, 1, 1],
#    # [0, 3, 3, 1],
#    # [2, 2, 4, 1],
#    # [2, 4, 6, 10],

#    # not skewed
#    [0,0,4,1],
#    [1,0,4,1],
#    [0,1,4,1],
#    [2,2,4,1],
#    [3,3,2,1],
#    # skewed
#    # [0,0,11],
#    # [1,1,12],
#    # [2,2,7],
#    # [3,3,5],
#    # [4,4,6],
#    # [5,5,6],
#    # [6,6,3],
#    # [7,7,3]
#])

def convertInputData():
    data = [] 
    for i in range (4, len(sys.argv) , 4):
       value1 = int(sys.argv[i]) 
       value2 = int(sys.argv[i+1])
       value3 = int(sys.argv[i+2])
       value4 = int(sys.argv[i+3])
       array = [value1, value2 , value3 , value4]
       data.append(array)
    return data


data = convertInputData()

inputIP = int(sys.argv[1])
inputMAC = int(sys.argv[2])
inputDAY = int(sys.argv[3])
queryKey = [inputIP , inputMAC , inputDAY , 1]
print(queryKey , "Hello")

df = pd.DataFrame(data)
df.column = ['x','y' , 'z' , 'z2']

total = 0
for i in range (len(data)):
    total += df.ix[i][3]

chance = 1 / len(data)
setValue = []
commonArray = []
for i in range(len(data)):
    probability = df.ix[i][3] / total
    if(probability > chance-0.05):
        setValue.append([df.ix[i][0], df.ix[i][1], df.ix[i][2], df.ix[i][3]])
    else:
        commonArray.append([df.ix[i][0], df.ix[i][1], df.ix[i][2], df.ix[i][3]])
print(setValue)
print(df.skew())
skewness = sk(data, axis=0 , bias=False )
skewed = bool
if( -0.5 < skewness[3] < 0.5 ):
    print("The data is approximately normally distributed with skewness of " , skewness[3])
    skewed = False
else:
    print("The data is skewed with skewness of " , skewness[3])
    skewed = True

matchedData = []
unmatchedData = []
for i in range (len(data)):
    if(df.ix[i][2] == queryKey[2]):
        matchedData.append([df.ix[i][0] , df.ix[i][1] , df.ix[i][2] , df.ix[i][3]])
    else:
        unmatchedData.append([df.ix[i][0] , df.ix[i][1] , df.ix[i][2] , df.ix[i][3]])

ipmatch = []
macmatch = []
ipnot = []
macnot = []
exactMatch = []
exactNot = []

for i in range (len(matchedData)):
    ipmatch.append(matchedData[i][0])
    macmatch.append(matchedData[i][1])
    exactMatch.append(str(matchedData[i][0]) + str(matchedData[i][1]))

for i in range (len(unmatchedData)):
    ipnot.append(unmatchedData[i][0])
    macnot.append(unmatchedData[i][1])
    exactNot.append(str(unmatchedData[i][0]) + str(unmatchedData[i][1]))

dayprioritySet = []
dayleftoverSet = []

otherPrioritySet = []
otherleftoverSet = []

for i in range (len(matchedData)):
    for j in range (len(setValue)):
        condition1 = str(matchedData[i][0]) + str(matchedData[i][1]) + str(matchedData[i][2]) + str(matchedData[i][3])
        condition2 = str(setValue[j][0]) + str(setValue[j][1]) + str(setValue[j][2]) + str(setValue[j][3])
        if(condition1 == condition2):
            dayprioritySet.append(str(setValue[j][0]) + str(setValue[j][1]))

for i in range (len(matchedData)):
    array = [matchedData[i][0] , matchedData[i][1] , matchedData[i][2] , matchedData[i][3]]
    if(array not in setValue):
        dayleftoverSet.append(str(matchedData[i][0]) + str(matchedData[i][1]))

for i in range (len(unmatchedData)):
    for j in range(len(setValue)):
        condition1 = str(unmatchedData[i][0]) + str(unmatchedData[i][1]) + str(unmatchedData[i][2]) + str(unmatchedData[i][3])
        condition2 = str(setValue[j][0]) + str(setValue[j][1]) + str(setValue[j][2]) + str(setValue[j][3])
        if (condition1 == condition2):
            array = [str(setValue[j][0]), str(setValue[j][1])]
            otherPrioritySet.append(str(setValue[j][0])+ str(setValue[j][1]))

for i in range(len(unmatchedData)):
    array = [unmatchedData[i][0] , unmatchedData[i][1] , unmatchedData[i][2] , unmatchedData[i][3]]
    if(array not in setValue):
        otherleftoverSet.append(str(unmatchedData[i][0]) + str(unmatchedData[i][1]))

print(dayprioritySet)
print(dayleftoverSet)
print(otherPrioritySet)
print(otherleftoverSet)
print(matchedData)


daypriorityip =  getIPFromSet(dayprioritySet)
dayprioritymac = getMACFromSet(dayprioritySet)
dayleftoverip = getIPFromSet(dayleftoverSet)
dayleftovermac = getMACFromSet(dayleftoverSet)
otherpriorityip = getIPFromSet(otherPrioritySet)
otherprioritymac = getMACFromSet(otherPrioritySet)
otherleftoverip = getIPFromSet(otherleftoverSet)
otherleftovermac = getMACFromSet(otherleftoverSet)

print(ipnot)
print(otherleftovermac)


queryIP = queryKey[0]
queryMAC = queryKey[1]
riskLevel = 0
concat = str(queryIP) + str(queryMAC)
if(skewed == True):
    riskLevel = classfierSkewed(skewed, dayprioritySet , dayleftoverSet , otherPrioritySet , otherleftoverSet , daypriorityip ,
                    dayprioritymac , dayleftoverip , dayleftovermac, otherpriorityip, otherprioritymac , otherleftoverip , otherleftovermac , concat)


if(skewed == False):
    riskLevel = classfierNormal(skewed, exactMatch , exactNot , ipmatch , macmatch , ipnot , macnot )




data1 = np.insert(data, len(data), [queryKey[0],queryKey[1], queryKey[2] , queryKey[3]] , axis= 0 )
df = pd.DataFrame(data1)
df.column = ['x','y' , 'z' , 'z2']

fig1 = plt.figure("IP and MAC address Link")
ax = fig1.add_subplot(111)
fig2 = plt.figure("Day each set appears")
ax2 = fig2.add_subplot(111)
fig3 = plt.figure("Count of each set")
ax3 = fig3.add_subplot(111)
mylabels = []
mytickers = []
for i in range (len(data1)):
    if(i == len(data1) - 1):
        mylabels.append("IP: " + str(data1[i][0]) + " / MAC :" + str(data1[i][1]))
        mytickers.append("Query Vector")
    else:
        mylabels.append("IP: " + str(data1[i][0]) + " / MAC :" + str(data1[i][1]))
        mytickers.append("Set " + str(i))
for i in range (len(data1)):

        ax.scatter(df.ix[i][0] , df.ix[i][1] , s=100)
        ax2.scatter(i, df.ix[i][2], s=100)
        ax2.set_xticklabels(mytickers, fontsize=10)
        ax3.bar(i , df.ix[i][3])
        ax3.set_xticklabels(mytickers , fontsize=10)

ipcount = 0
maccount= 0
for i in range (len(data1)):
    if(data1[i][0] > ipcount):
        ipcount = data1[i][0]

    if(data1[i][1] > maccount):
        maccount = data1[i][1]


ax.set_xlabel("Unique IP")
ax.set_ylabel("Unique MAC")
ax.set_xticks(np.arange(-1 , ipcount + 1, 1))
ax.set_yticks(np.arange(-1, maccount + 1 , 1))


ax2.set_xlabel("Set Number")
ax2.set_ylabel("Days it appeared")
ax2.set_xticks(np.arange(0, len(mytickers), 1))

ax3.set_xlabel("Set Number")
ax3.set_ylabel("Count")
ax3.set_xticks(np.arange(0, len(mytickers), 1))

totalcount = 0
for i in range (len(data1)):
    if(data1[i][3] > totalcount):
        totalcount = data1[i][3]

ax3.set_yticks(np.arange(0, totalcount + 1, 1))

ax.legend(labels=mytickers)
ax2.legend(labels=mylabels)
ax3.legend(labels=mylabels)
multipage("predictionImages.pdf")

#print(plt.show())

print(riskLevel)
riskStatement = evaulateRiskLevel(riskLevel)
print("There is a " + str(riskStatement))