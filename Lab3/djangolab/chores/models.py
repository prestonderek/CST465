from django.db import models

# Create your models here.

class ChoreLaborer:
    def __init__(self, name="", age=0, difficulty=0):
        self.name = name
        self.age = age
        self._difficulty = 0
        self.difficulty = difficulty  # Use property setter
    
    @property
    def difficulty(self):
        return self._difficulty
    
    @difficulty.setter
    def difficulty(self, value):
        if value < 0:
            self._difficulty = 0
        elif value > 10:
            self._difficulty = 10
        else:
            self._difficulty = value
    
    def __str__(self):
        return self.name

class ChoreWorkforce:
    def __init__(self):
        self.laborers = []
    
    def add_laborer(self, laborer):
        self.laborers.append(laborer)
