import string
import threading

buf = []
NUM_OF_LINES = 10
CHARS_IN_LINE = 50

def make_buf():
    global buf
    for letter in string.ascii_lowercase[::-1]:
        for i in range(NUM_OF_LINES):
            buf.append(letter)

def get_letter():
    global buf
    if (len(buf) > 0):        
        return buf.pop()
    else:
        return None
    

def writer_events(eventWait, eventSet):
     while True:
        letter = get_letter()
        if (letter == None):
            return
        eventWait.wait()
        eventWait.clear()
        for j in range(CHARS_IN_LINE):
            print(letter, end="")
        print()
        eventSet.set()

def events(num_of_threads):
    arr_of_thread = []
    eventArr = []
    
    for i in range(num_of_threads):
        event = threading.Event()
        eventArr.append(event)        
    eventArr[0].set()
    
    for i in range(num_of_threads):        
        t = threading.Thread(target=writer_events, args=(eventArr[i], eventArr[(i + 1) % num_of_threads]))
        t.start()
        arr_of_thread.append(t)
        
    for i in range(len(arr_of_thread)):
        arr_of_thread[i].join() 

        
def writer_no_sync():
    while (True):
        letter = get_letter()
        if (letter == None):
            return
        for j in range(CHARS_IN_LINE):
            print(letter, end="")
        print()

def no_sync(num_of_threads):
    arr_of_thread = []
    for i in range(num_of_threads):        
        t = threading.Thread(target=writer_no_sync)
        t.start()
        arr_of_thread.append(t)
    for i in range(len(arr_of_thread)):
        arr_of_thread[i].join() 



def writer_lock(lock):
    while (True):
        letter = get_letter()
        if (letter == None):
            return
        lock.acquire()
        for j in range(CHARS_IN_LINE):
            print(letter, end="")
        print()
        lock.release()

def lock(num_of_threads):
    arr_of_thread = []
    lock = threading.Lock()
    for i in range(num_of_threads):        
        t = threading.Thread(target=writer_lock, args=(lock,))
        t.start()
        arr_of_thread.append(t)
    for i in range(len(arr_of_thread)):
        arr_of_thread[i].join()
    lock = threading.Lock()
    

def writer_semaphore(semaphore):
    while (True):
        letter = get_letter()
        if (letter == None):
            return
        semaphore.acquire()
        for j in range(CHARS_IN_LINE):
            print(letter, end="")
        print()
        semaphore.release()
                      

def semaphore(num_of_threads):
    arr_of_thread = []
    semaphore = threading.Semaphore()
    for i in range(num_of_threads):        
        t = threading.Thread(target=writer_semaphore, args=(semaphore,))
        t.start()
        arr_of_thread.append(t)
    for i in range(len(arr_of_thread)):
        arr_of_thread[i].join()
        

def main():
    make_buf()
    type = 0
    print('1 - NoSync')
    print('2 - Lock')
    print('3 - Semafore')
    print('4 - Event')
    while (type > 4 or type < 1):        
        type = int(input('Choose type of sync: '))
    num_of_threads = int(input('Enter number of threads: '))
    if (type == 1):
        no_sync(num_of_threads)
    elif (type == 2):
        lock(num_of_threads)
    elif (type == 3):
        semaphore(num_of_threads)
    elif (type == 4):
        events(num_of_threads)
        
if __name__ == "__main__":
    main()
