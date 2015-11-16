import string
import threading

buf = []
numOflines = 100
charsInLine = 5000

def MakeBuf():
    global buf
    for letter in string.ascii_lowercase[::-1]:
        for i in range(numOflines):
            buf.append(letter)

def GetLetter():
    global buf
    if (len(buf) > 0):        
        return buf.pop()
    else:
        return None
    

def WriterEvents(eventWait, eventSet):
     while (True):
        letter = GetLetter()
        if (letter == None):
            return
        eventWait.wait()
        eventWait.clear()
        for j in range(charsInLine):
            print(letter, end="")
        print()
        eventSet.set()

def Events(numThreads):
    threadArr = []
    eventArr = []
    
    for i in range(numThreads):
        event = threading.Event()
        eventArr.append(event)        
    eventArr[0].set()
    
    for i in range(numThreads):        
        t = threading.Thread(target=WriterEvents, args=(eventArr[i], eventArr[(i + 1) % numThreads]))
        t.start()
        threadArr.append(t)
        
    for i in range(len(threadArr)):
        threadArr[i].join() 

        
def WriterNoSync():
    while (True):
        letter = GetLetter()
        if (letter == None):
            return
        for j in range(charsInLine):
            print(letter, end="")
        print()

def NoSync(numThreads):
    threadArr = []
    for i in range(numThreads):        
        t = threading.Thread(target=WriterNoSync)
        t.start()
        threadArr.append(t)
    for i in range(len(threadArr)):
        threadArr[i].join() 



def WriterLock(lock):
    while (True):
        letter = GetLetter()
        if (letter == None):
            return
        lock.acquire()
        for j in range(charsInLine):
            print(letter, end="")
        print()
        lock.release()

def Lock(numThreads):
    threadArr = []
    lock = threading.Lock()
    for i in range(numThreads):        
        t = threading.Thread(target=WriterLock, args=(lock,))
        t.start()
        threadArr.append(t)
    for i in range(len(threadArr)):
        threadArr[i].join()
    lock = threading.Lock()
    

def WriterSemafore(semaphore):
    while (True):
        letter = GetLetter()
        if (letter == None):
            return
        semaphore.acquire()
        for j in range(charsInLine):
            print(letter, end="")
        print()
        semaphore.release()
                      

def Semafore(numThreads):
    threadArr = []
    semaphore = threading.Semaphore()
    for i in range(numThreads):        
        t = threading.Thread(target=WriterSemafore, args=(semaphore,))
        t.start()
        threadArr.append(t)
    for i in range(len(threadArr)):
        threadArr[i].join()
        

def main():
    MakeBuf()
    type = 0
    print('1 - NoSync')
    print('2 - Lock')
    print('3 - Semafore')
    print('4 - Event')
    while (type > 4 or type < 1):        
        type = int(input('Choose type of sync: '))
    numThreads = int(input('Enter number of threads: '))
    if (type == 1):
        NoSync(numThreads)
    elif (type == 2):
        Lock(numThreads)
    elif (type == 3):
        Semafore(numThreads)
    elif (type == 4):
        Events(numThreads)
        
if __name__ == "__main__":
    main()
