from random import random
import math
from time import clock
from multiprocessing import Process, Queue

def is_intersection(x, y):
    return math.sqrt(x * x + y * y) <= 1


def randomize():
    return random() * 2 - 1


def monte_carlo(queue, iterations):
    num_of_intersections = 0
    for i in range(iterations):
        if (is_intersection(randomize(), randomize())):
            num_of_intersections += 1
    queue.put(num_of_intersections) 


def create_processes(num_of_proccesses, num_of_iteratoins):
    processes = []
    queue = Queue()
    points_to_process = num_of_iteratoins / num_of_proccesses
    for i in range(num_of_proccesses):
        if (i == 0):
             p = Process(target = monte_carlo, args=(queue, points_to_process + num_of_iteratoins % num_of_proccesses))
        else:                 
            p = Process(target = monte_carlo, args=(queue, points_to_process))
        processes.append(p)
        p.start()        
    for i in range(num_of_proccesses):
        processes[i].join()
    intersections = 0.0
    while (not queue.empty()):
        intersections += queue.get()
    print intersections / num_of_iteratoins * 4

def main():
    num_of_processes = input('Enter number of proccesses: ')
    num_of_iteratoins = input('Enter number of iterations: ')
    start_time = clock()
    create_processes(num_of_processes, num_of_iteratoins)
    finish_time = clock()
    print 'Total time - ',finish_time - start_time

if __name__ == "__main__":
    main()
    
