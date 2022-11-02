import numpy as np


class Point:
    def __init__(self, x, y, z):
        self.point = np.array([x, y, z])

    def get_point(self) -> np.ndarray:
        return self.point

    def set_point(self, x, y, z):
        self.point = np.array([x, y, z])

    def to_string(self) -> str:
        return np.array2string(self.point)


class CoordinateSystem:
    def __init__(self, p1, p2, p3):
        self.coordinate_system = np.array([p1, p2, p3])

    def get_coordinate_system(self) -> np.ndarray:
        return self.coordinate_system

    def set_coordinate_system(self, p1, p2, p3):
        self.coordinate_system = np.array([p1, p2, p3])

    def to_string(self) -> str:
        return [point.to_string() for point in self.coordinate_system]


m1 = np.array([[5, -10, 15], [3, -6, 9], [-4, 8, 12]])
print(m1)

p1 = Point(x=0, y=0, z=1)
p2 = Point(0, 1, 0)

p2.set_point(x=0, y=2, z=0)
p3 = Point(1, 0, 0)

print(p1.get_point())

c1 = CoordinateSystem(p1=p1, p2=p2, p3=p3)
print(c1.get_coordinate_system())

print(c1.to_string())
