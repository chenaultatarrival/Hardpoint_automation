import numpy as np


class Point:
    def __init__(self, x, y, z):
        self.x, self.y, self.z = x, y, z

    #def __str__(self):
        #return "{}, {}, {}".format(self.x, self.y, self.z)
        
        #? --- WALID --- this didn't work because __str__ returned a non-string
        #return [self.x, self.y, self.z]

    def __neg__(self):
        return Point(-self.x, -self.y, -self.z)

    def __add__(self, point):
        return Point(self.x + point.x, self.y + point.y, self.z + point.z)

    def __sub__(self, point):
        return self + -point


class Coordinate_system:
    def __init__(self, p1, p2, p3) -> list:
        # self.origin, self.x, self.y = p1, p2, p3
        coordinate_matrix = np.array(p1, p2, p3)

    #def __str__(self):
    #    return "{}, {}, {}".format(self.origin, self.x, self.y)


M1 = np.array([[5, -10, 15], [3, -6, 9], [-4, 8, 12]])
print(M1)

p1 = Point(0, 0, 1)
p2 = Point(0, 1, 0)
p3 = Point(1, 0, 0)
print(p1 + p2 + p3)

C1 = Coordinate_system(p1=p1, p2=p2, p3=p3)
print(C1)


# ? --- Walid --- why is "main()" not recognised when I'm using the Conda interpreter, likewise, numpy isn't recognised when I'm using the vanilla Python 3.10.7 interpreter?
# if __name__ == '__main__':
#    main()
